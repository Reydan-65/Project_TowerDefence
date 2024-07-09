using SpaceShooter;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefence
{
    public class BackToMainMenuButton : MonoBehaviour
    {
        [SerializeField] private Transform m_Transition;

        private SceneTransitionManager m_SceneTransitionManager;

        private void Start()
        {
            m_SceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
        }
        public void EX_LoadMainMenuScene()
        {
            m_SceneTransitionManager.LoadScene("mainMenu");
            //SceneManager.LoadScene(0);
        }
    }
}