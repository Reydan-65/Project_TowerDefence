using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

namespace TowerDefence
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button m_ContinueButton;
        [SerializeField] private AcceptPanel m_AcceptPanel;

        private SceneTransitionManager m_SceneTransitionManager;

        private void Awake()
        {
            m_SceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
        }

        private void Start()
        {
            m_ContinueButton.interactable = FileHandler.HasFile(MapCompletion.filename);
        }

        public void EX_StartNewGame()
        {
            Sound.Click.Play();

            if (FileHandler.HasFile(MapCompletion.filename))
                m_AcceptPanel.gameObject.SetActive(true);
            else
            {
                m_SceneTransitionManager.LoadScene("levelMap");
            }
        }

        public void EX_ContinueGame()
        {
            Sound.Click.Play();
            m_SceneTransitionManager.LoadScene("levelMap");
        }

        public void EX_Quit()
        {
            Sound.Click.Play();

            StartCoroutine(DelayTime());
        }

        private IEnumerator DelayTime()
        {
            yield return new WaitForSeconds(0.3f);
            Application.Quit();
        }
    }
}