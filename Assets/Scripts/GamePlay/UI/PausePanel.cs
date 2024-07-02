using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Common;

namespace SpaceShooter
{
    public class PausePanel : MonoBehaviour
    {
        [SerializeField] private GameObject m_Panel;

        private bool OnPause = false;

        private void Start()
        {
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
            //PlayClickSound();
            SceneManager.LoadScene(1);
            Time.timeScale = 1.0f;

            //StartCoroutine(OnLoadMenu());
        }

        public void EX_LoadMainMenu()
        {
            OnPause = false;
            //PlayClickSound();
            SceneManager.LoadScene(0);
            Time.timeScale = 1.0f;

            //StartCoroutine(OnLoadMenu());
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