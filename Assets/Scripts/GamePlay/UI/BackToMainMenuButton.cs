using System.Collections;
using UnityEngine;

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
            TDButton.PlayClickSound();
            StartCoroutine(DelayTime());
        }

        private IEnumerator DelayTime()
        {
            yield return new WaitForSeconds(0.3f);
            m_SceneTransitionManager.LoadScene("mainMenu");
            //SceneManager.LoadScene(0);
        }
    }
}