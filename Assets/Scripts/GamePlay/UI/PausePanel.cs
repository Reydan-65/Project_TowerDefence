using UnityEngine;
using System.Collections;
using TowerDefence;

namespace SpaceShooter
{
    public class PausePanel : MonoBehaviour
    {
        public static bool isPaused = false;

        [SerializeField] private GameObject m_Panel;

        private EnemyWaveManager m_EnemyWaveManager;
        private SceneTransitionManager m_SceneTransitionManager;

        private void Start()
        {
            m_EnemyWaveManager = FindObjectOfType<EnemyWaveManager>();
            m_SceneTransitionManager = FindObjectOfType<SceneTransitionManager>();

            isPaused = false;
            gameObject.SetActive(false);
        }

        public void EX_PauseGame()
        {
            isPaused = true;

            LevelController.StopLevelActivity();

            TDButton.PlayClickSound();
            ShowPause();
        }

        public void EX_ResumeGame()
        {
            TDButton.PlayClickSound();
            StartCoroutine(DelayTime());
        }

        private IEnumerator DelayTime()
        {
            yield return new WaitForSeconds(0.35f);

            isPaused = false;

            LevelController.ReturnLevelActivity();

            HidePause();
        }

        public void EX_LoadLevelMap()
        {
            isPaused = false;
            OnLeaveLevelSceneUnsubscribe();
            TDButton.PlayClickSound();
            LevelController.ReturnLevelActivity();
            m_SceneTransitionManager.LoadScene("levelMap");
            //SceneManager.LoadScene(1);

            //StartCoroutine(OnLoadMenu());
        }

        public void EX_LoadMainMenu()
        {
            isPaused = false;
            OnLeaveLevelSceneUnsubscribe();
            TDButton.PlayClickSound();
            LevelController.ReturnLevelActivity();
            m_SceneTransitionManager.LoadScene("mainMenu");
            //SceneManager.LoadScene(0);

            //StartCoroutine(OnLoadMenu());
        }

        private void ShowPause()
        {
            gameObject.SetActive(true);
            isPaused = true;
        }

        private void HidePause()
        {
            gameObject.SetActive(false);
            isPaused = false;
        }

        //Когда покидаем сцену уровня,
        //отписываемся от подсчёта противников,
        //если остались противники, уничтожаем их.
        private void OnLeaveLevelSceneUnsubscribe()
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();

            foreach (Enemy enemy in enemies)
            {
                enemy.OnEnemyDestroy -= m_EnemyWaveManager.RecordEnemyDead;
                Destroy(enemy);
            }
        }

        //private IEnumerator OnLoadMenu()
        //{
        //    yield return new WaitForSeconds(SoundManager.Instance.AudioProperties.ClickClips[0].length);

        //    m_Panel.SetActive(false);
        //    SceneManager.LoadScene(0);
        //}

    }
}