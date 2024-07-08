using SpaceShooter;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefence
{
    public class BackToMainMenuButton : MonoBehaviour
    {
        [SerializeField] private Transform m_Transition;
        public void EX_LoadMainMenuScene()
        {
            SceneManager.LoadScene(0);
        }
    }
}