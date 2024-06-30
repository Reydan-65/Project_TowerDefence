using UnityEngine;
using UnityEngine.SceneManagement;
using SpaceShooter;
using TMPro;

namespace TowerDefence
{
    public class MapLevel : MonoBehaviour
    {
        private LevelProperties m_LevelProperties;

        [SerializeField] private int m_LevelIndex;
        [SerializeField] private TextMeshProUGUI m_Text;
        
        public int LevelIndex => m_LevelIndex;

        private void Awake()
        {
            m_LevelProperties = LevelSequencesController.Instance.LevelSequences.LevelsProperties[m_LevelIndex];
        }

        public void EX_LoadLevel()
        {
            SceneManager.LoadScene(m_LevelProperties.SceneName);
        }

        public void SetLevelData(string episodeName, int levelScore)
        {
            m_LevelProperties.SceneName = episodeName;
            m_Text.text = $"{levelScore} / 3";
        }
    }
}