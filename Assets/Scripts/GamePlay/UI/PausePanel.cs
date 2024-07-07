using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Common;
using TowerDefence;

namespace SpaceShooter
{
    public class PausePanel : MonoBehaviour
    {
        [SerializeField] private GameObject m_Panel;

        private EnemyWaveManager m_EnemyWaveManager;

        private bool OnPause = false;

        private void Start()
        {
            m_EnemyWaveManager = FindObjectOfType<EnemyWaveManager>();

            OnPause = false;
            gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }

        public void EX_ShowPause()
        {
            if (OnPause == false)
            {
                gameObject.SetActive(true);
                OnPause = true;
                //PlayClickSound();

                Time.timeScale = 0f;
            }
            else
            {
                gameObject.SetActive(false);
                OnPause = false;
                //PlayClickSound();

                Time.timeScale = 1.0f;
            }
        }

        public void EX_HidePause()
        {
            gameObject.SetActive(false);
            OnPause = false;
            //PlayClickSound();

            Time.timeScale = 1.0f;
        }

        public void EX_LoadLevelMap()
        {
            OnPause = false;
            OnLeaveLevelSceneUnsubscribe();
            //PlayClickSound();
            SceneManager.LoadScene(1);
            Time.timeScale = 1.0f;

            //StartCoroutine(OnLoadMenu());
        }

        public void EX_LoadMainMenu()
        {
            OnPause = false;
            OnLeaveLevelSceneUnsubscribe();
            //PlayClickSound();
            SceneManager.LoadScene(0);
            Time.timeScale = 1.0f;

            //StartCoroutine(OnLoadMenu());
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

        /*
        private void PlayClickSound()
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.AudioProperties.ClickClips, 0,
                                              LevelController.Instance.AudioSource, SoundManager.Instance.AudioProperties.SoundsVolume);
        }

        private IEnumerator OnLoadMenu()
        {
            yield return new WaitForSeconds(SoundManager.Instance.AudioProperties.ClickClips[0].length);

            m_Panel.SetActive(false);
            SceneManager.LoadScene(0);
        }
        */
    }
}