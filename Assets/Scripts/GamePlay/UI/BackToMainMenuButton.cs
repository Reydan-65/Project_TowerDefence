using SpaceShooter;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefence
{
    public class BackToMainMenuButton : MonoBehaviour
    {
        public void EX_LoadMainMenuScene()
        {
            SceneManager.LoadScene(0);
        }
    }
}