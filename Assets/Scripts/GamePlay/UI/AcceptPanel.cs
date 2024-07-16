using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefence
{
    public class AcceptPanel : MonoBehaviour
    {
        private SceneTransitionManager m_SceneTransitionManager;

        private void Start()
        {
            m_SceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
            gameObject.SetActive(false);
        }

        public void EX_Accept()
        {
            TDButton.PlayClickSound();
            FileHandler.Reset(MapCompletion.filename);
            FileHandler.Reset(Upgrades.filename);

            m_SceneTransitionManager.LoadScene("levelMap");
        }

        public void EX_Cancel()
        {
            TDButton.PlayClickSound();
            StartCoroutine(WaitTimer());
        }

        private IEnumerator WaitTimer()
        {
            yield return new WaitForSeconds(0.3f);
            gameObject.SetActive(false);
        }
    }
}