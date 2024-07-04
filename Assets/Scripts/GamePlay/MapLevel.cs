using UnityEngine;
using UnityEngine.SceneManagement;
using SpaceShooter;
using UnityEngine.UI;

namespace TowerDefence
{
    public class MapLevel : MonoBehaviour
    {
        private LevelProperties m_LevelProperties;
        [SerializeField] private RectTransform m_ResultPanel;
        [SerializeField] private Image[] m_ResultImage;

        [SerializeField] private int m_LevelIndex;
        
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

            m_ResultPanel.gameObject.SetActive(levelScore > 0);

            for (int i = 0; i < levelScore; i++)
            {
                m_ResultImage[i].color = Color.white;
            }
        }
    }
}