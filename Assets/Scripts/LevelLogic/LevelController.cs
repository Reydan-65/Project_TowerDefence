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
        private BranchLevelProperties m_CurrentBranchLevelProperties;
        private SceneTransitionManager m_SceneTransitionManager;

        private float m_StartReferenceTime;

        public float LevelTime => m_LevelTime;
        public float ReferenceTime => m_ReferenceTime;
        public float StartReferenceTime => m_StartReferenceTime;

        public LevelProperties CurrentLevelProperties => m_CurrentLevelProperties;
        public BranchLevelProperties CurrentBranchLevelProperties => m_CurrentBranchLevelProperties;

        private int m_LevelScore = 3;

        private void Start()
        {
            m_LevelTime = 0;
            m_StartReferenceTime = m_ReferenceTime;

            m_LevelSequencesController = LevelSequencesController.Instance;
            m_SceneTransitionManager = FindObjectOfType<SceneTransitionManager>();

            if (m_LevelSequencesController.GetCurrentLoadedLevel() != null)
                m_CurrentLevelProperties = m_LevelSequencesController.GetCurrentLoadedLevel();

            if (m_LevelSequencesController.GetCurrentLoadedBranchLevel() != null)
                m_CurrentBranchLevelProperties = m_LevelSequencesController.GetCurrentLoadedBranchLevel();

            // Снижение очков уровня при первом получении урона
            void LifeScoreChange(int _)
            {
                m_LevelScore -= 1;
                TD_Player.Instance.OnLivesUpdate -= LifeScoreChange;
            }

            TD_Player.Instance.OnLivesUpdate += LifeScoreChange;
        }

        private void FixedUpdate()
        {
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName(LevelMapSceneName))
            {
                if (PausePanel.isPaused) return;

                if (m_IsLevelCompleted == false)
                {
                    m_LevelTime += Time.deltaTime;

                    CheckLevelConditions();
                }

                if (Player.Instance.CurrentNumLives == 0)
                    Lose();
            }
        }
        private void OnDestroy()
        {
            ClearScene();
        }

        /// <summary>
        /// Очистка.
        /// </summary>
        private void ClearScene()
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();

            foreach (var enemy in enemies)
            {
                if (enemy != null) Destroy(enemy);
            }
        }

        private void CheckLevelConditions()
        {
            int numCompleted = 0;

            for (int i = 0; i < m_Conditions.Length; i++)
            {
                if (m_Conditions[i].IsCompleted == true)
                    numCompleted++;
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
        }

        private void Pass()
        {
            if (m_ReferenceTime <= m_LevelTime)
                m_LevelScore -= 1;

            MapCompletion.Instance.SaveLevelResult(m_LevelScore);

            StopLevelActivity();

            LevelPassed.Invoke();
        }

        public static void StopLevelActivity()
        {
            BuyControl buyControl = FindAnyObjectByType<BuyControl>();

            if (buyControl != null)
                buyControl.gameObject.SetActive(false);

            EnemyWaveManager enemyWaveManager = FindAnyObjectByType<EnemyWaveManager>();

            if (enemyWaveManager != null)
                enemyWaveManager.PauseSpawnEnemiesCoroutine();

            foreach (var enemy in FindObjectsOfType<Enemy>())
            {
                enemy.GetComponent<SpaceShip>().enabled = false;
                enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }

            void DisableAll<T>() where T : MonoBehaviour
            {
                foreach (var obj in FindObjectsOfType<T>())
                    obj.enabled = false;
            }

            DisableAll<Projectile>();
            DisableAll<Tower>();
            DisableAll<NextWave_GUI>();
            DisableAll<TextUpdate>();
            DisableAll<Abilities>();
            DisableAll<TD_Player>();
            DisableAll<EnemyWaveManager>();
        }

        public static void ReturnLevelActivity()
        {
            BuyControl buyControl = FindAnyObjectByType<BuyControl>();

            if (buyControl != null)
                buyControl.gameObject.SetActive(true);

            foreach (var enemy in FindObjectsOfType<Enemy>())
            {
                enemy.GetComponent<SpaceShip>().enabled = true;
                enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            }

            void EnableAll<T>() where T : MonoBehaviour
            {
                foreach (var obj in FindObjectsOfType<T>())
                    obj.enabled = true;
            }

            EnableAll<Projectile>();
            EnableAll<Tower>();
            EnableAll<NextWave_GUI>();
            EnableAll<TextUpdate>();
            EnableAll<Abilities>();
            EnableAll<TD_Player>();
            EnableAll<EnemyWaveManager>();

            EnemyWaveManager enemyWaveManager = FindAnyObjectByType<EnemyWaveManager>();

            if (enemyWaveManager != null)
                enemyWaveManager.ResumeSpawnEnemiesCoroutine();
        }

        public void ReturnLevelMap()
        {
            m_SceneTransitionManager.LoadScene(LevelMapSceneName);
        }
    }
}