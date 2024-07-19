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
            Sound.Click.Play();

            isPaused = true;
            LevelController.StopLevelActivity();
            ShowPause();
        }

        public void EX_ResumeGame()
        {
            Sound.Click.Play();

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
            Sound.Click.Play();

            isPaused = false;
            OnLeaveLevelSceneUnsubscribe();
            m_SceneTransitionManager.LoadScene("levelMap");
        }

        public void EX_LoadMainMenu()
        {
            Sound.Click.Play();

            isPaused = false;
            OnLeaveLevelSceneUnsubscribe();
            m_SceneTransitionManager.LoadScene("mainMenu");
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

        //����� �������� ����� ������,
        //������������ �� �������� �����������,
        //���� �������� ����������, ���������� ��.
        private void OnLeaveLevelSceneUnsubscribe()
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();

            foreach (Enemy enemy in enemies)
            {
                enemy.OnEnemyDestroy -= m_EnemyWaveManager.RecordEnemyDead;
                Destroy(enemy);
            }
        }
    }
}