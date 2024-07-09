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
            FileHandler.Reset(MapCompletion.filename);
            FileHandler.Reset(Upgrades.filename);

            m_SceneTransitionManager.LoadScene("levelMap");
            //SceneManager.LoadScene(1);
        }

        public void EX_Cancel()
        {
            gameObject.SetActive(false);
        }
    }
}