using SpaceShooter;
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
            FileHandler.Reset(MapCompletion.filename);
            FileHandler.Reset(Upgrades.filename);

            Sound.Click.PlaySound();
            m_SceneTransitionManager.LoadScene("levelMap");
        }

        public void EX_Cancel()
        {
            Sound.Click.PlaySound();
            StartCoroutine(WaitTimer());
        }

        private IEnumerator WaitTimer()
        {
            yield return new WaitForSeconds(0.3f);
            gameObject.SetActive(false);
        }
    }
}