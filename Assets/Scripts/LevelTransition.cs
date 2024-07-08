using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefence
{
    public class LevelTransition : MonoBehaviour
    {
        [SerializeField] private string m_SceneName;

        public void ChangeScene()
        {
            SceneManager.LoadScene(m_SceneName);
        }
    }
}