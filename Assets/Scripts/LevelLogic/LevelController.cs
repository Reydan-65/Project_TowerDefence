using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TowerDefence;

namespace SpaceShooter
{
    public class LevelController : SingletonBase<LevelController>
    {
        private const string MainMenuSceneName = "main_menu";
        private const string LevelMapSceneName = "levelMap";

        [SerializeField] private LevelCondition[] m_Conditions;

        public event UnityAction LevelPassed;
        public event UnityAction LevelLost;

        private bool m_IsLevelCompleted;
        private float m_LevelTime;

        [SerializeField] private float m_ReferenceTime;

        private LevelSequencesController m_LevelSequencesController;
        private LevelProperties m_CurrentLevelProperties;
        private AudioSource m_AudioSource;

        public AudioSource AudioSource { get => m_AudioSource; set => m_AudioSource = value; }

        public float LevelTime => m_LevelTime;
        public float ReferenceTime => m_ReferenceTime;

        public LevelProperties CurrentLevelProperties => m_CurrentLevelProperties;

        private int m_LevelScore = 3;

        private void Start()
        {
            Time.timeScale = 1.0f;
            m_LevelTime = 0;

            m_ReferenceTime += Time.time;

            m_LevelSequencesController = LevelSequencesController.Instance;
            m_CurrentLevelProperties = m_LevelSequencesController.GetCurrentLoadedLevel();

            // —нижение очков уровн€ при первом получении урона
            void LifeScoreChange(int _)
            {
                m_LevelScore -= 1;
                TD_Player.OnLivesUpdate -= LifeScoreChange;
            }
            TD_Player.OnLivesUpdate += LifeScoreChange;


            //FollowCamera camera = FindAnyObjectByType<FollowCamera>();

            //m_AudioSource = camera.GetComponent<AudioSource>();

            //SoundManager.Instance.PlayOneShot(SoundManager.Instance.AudioProperties.SoundtrackClips, 1,
            //             m_AudioSource, SoundManager.Instance.AudioProperties.MusicVolume);
        }

        private void FixedUpdate()
        {
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("levelMap"))
            {
                if (m_IsLevelCompleted == false)
                {
                    m_LevelTime += Time.deltaTime;
                    CheckLevelConditions();
                }

                if (Player.Instance.CurrentNumLives == 0)
                {
                    Lose();
                }
            }
        }

        private void CheckLevelConditions()
        {
            int numCompleted = 0;

            for (int i = 0; i < m_Conditions.Length; i++)
            {
                if (m_Conditions[i].IsCompleted == true)
                {
                    numCompleted++;
                }
            }

            if (numCompleted == m_Conditions.Length)
            {
                m_IsLevelCompleted = true;

                Pass();
            }
        }

        private void Lose()
        {
            StopLevelActivity();

            LevelLost.Invoke();
            //Time.timeScale = 0f;
        }

        private static void StopLevelActivity()
        {
            BuyControl buyControl = FindAnyObjectByType<BuyControl>();

            if (buyControl != null)
                buyControl.gameObject.SetActive(false);

            foreach (var enemy in FindObjectsOfType<Enemy>())
            {
                enemy.GetComponent<SpaceShip>().enabled = false;
                enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }

            void DisableAll<T>() where T : MonoBehaviour
            {
                foreach (var obj in FindObjectsOfType<T>())
                {
                    obj.enabled = false;
                }
            }

            DisableAll<Spawner>();
            DisableAll<Projectile>();
            DisableAll<Tower>();
        }

        private void Pass()
        {
            if (m_ReferenceTime <= Time.time)
                m_LevelScore -= 1;

            //if (TD_Player.Instance.CurrentNumLives < TD_Player.Instance.NumLives)
            //    m_LevelScore -= 1;

            MapCompletion.Instance.SaveLevelResult(m_LevelScore);
            StopLevelActivity();
            print(m_LevelScore);

            LevelPassed.Invoke();
            //Time.timeScale = 0f;
        }

        public void LoadNextLevel()
        {
            if (m_LevelSequencesController.CurrentLevelIsLast() == false)
            {
                string nextLevelSceneName = m_LevelSequencesController.GetNextLevelProperties(m_CurrentLevelProperties).SceneName;

                SceneManager.LoadScene(nextLevelSceneName);
            }
            else
                SceneManager.LoadScene(MainMenuSceneName);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(m_CurrentLevelProperties.SceneName);
        }

        public void ReturnLevelMap()
        {
            SceneManager.LoadScene(LevelMapSceneName);
        }
    }
}