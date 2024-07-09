using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
            if (FileHandler.HasFile(MapCompletion.filename))
                m_AcceptPanel.gameObject.SetActive(true);
            else
            {
                m_SceneTransitionManager.LoadScene("levelMap");
                //SceneManager.LoadScene(1);
            }
        }

        public void EX_ContinueGame()
        {
            m_SceneTransitionManager.LoadScene("levelMap");
            //SceneManager.LoadScene(1);
        }

        public void EX_Quit()
        {
            Application.Quit();
        }
    }
}