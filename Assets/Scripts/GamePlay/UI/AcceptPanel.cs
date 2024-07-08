using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefence
{
    public class AcceptPanel : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void EX_Accept()
        {
            FileHandler.Reset(MapCompletion.filename);
            FileHandler.Reset(Upgrades.filename);

            SceneManager.LoadScene(1);
        }

        public void EX_Cancel()
        {
            gameObject.SetActive(false);
        }
    }
}