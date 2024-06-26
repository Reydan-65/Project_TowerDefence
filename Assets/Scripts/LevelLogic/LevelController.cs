using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Common;
using TowerDefence;

namespace SpaceShooter
{
    public class LevelController : SingletonBase<LevelController>
    {
        private const string MainMenuSceneName = "main_menu";

        [SerializeField] private LevelCondition[] m_Conditions;

        //public event UnityAction LevelPassed;
        public event UnityAction LevelLost;

        private bool m_IsLevelCompleted;
        private float m_LevelTime;

        private LevelSequencesController m_LevelSequencesController;
        private LevelProperties m_CurrentLevelProperties;
        private AudioSource m_AudioSource;

        public AudioSource AudioSource { get => m_AudioSource; set => m_AudioSource = value; }

        public float LevelTime => m_LevelTime;

        private void Start()
        {
            Time.timeScale = 1.0f;
            m_LevelTime = 0;

            m_LevelSequencesController = LevelSequencesController.Instance;
            m_CurrentLevelProperties = m_LevelSequencesController.GetCurrentLoadedLevel();

            //FollowCamera camera = FindAnyObjectByType<FollowCamera>();

            //m_AudioSource = camera.GetComponent<AudioSource>();

            //SoundManager.Instance.PlayOneShot(SoundManager.Instance.AudioProperties.SoundtrackClips, 1,
            //             m_AudioSource, SoundManager.Instance.AudioProperties.MusicVolume);
        }

        private void FixedUpdate()
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
            LevelLost.Invoke();
            Time.timeScale = 0f;
        }

        private void Pass()
        {
            if (m_LevelSequencesController.CurrentLevelIsLast() == false)
            {
                string nextLevelSceneName = m_LevelSequencesController.GetNextLevelProperties(m_CurrentLevelProperties).SceneName;

                SceneManager.LoadScene(nextLevelSceneName);
            }
            else
                SceneManager.LoadScene(0);

            //LevelPassed.Invoke();
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
    }
}