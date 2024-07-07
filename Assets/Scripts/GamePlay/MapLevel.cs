using UnityEngine;
using UnityEngine.SceneManagement;
using SpaceShooter;
using UnityEngine.UI;

namespace TowerDefence
{
    public class MapLevel : MonoBehaviour
    {
        private LevelProperties m_LevelProperties;
        private BranchLevelProperties m_BranchLevelProperties;

        [SerializeField] private RectTransform m_ResultPanel;
        [SerializeField] private Image[] m_ResultImage;

        public RectTransform ResultPanel { get { return m_ResultPanel; } }
        public Image[] ResultImage => m_ResultImage;

        [SerializeField] private int m_LevelIndex;
        public int LevelIndex => m_LevelIndex;
        [SerializeField] private int m_BranchLevelIndex;
        public int BranchLevelIndex => m_BranchLevelIndex;

        //Если панель результата уровня активна, уровень считается пройденым...
        public bool IsComplete
        {
            get
            {
                return gameObject.activeSelf &&
                       m_ResultPanel.gameObject.activeSelf;
            }
        }

        private void Awake()
        {
            if (m_LevelIndex >= 0)
                m_LevelProperties = LevelSequencesController.Instance.LevelSequences.LevelsProperties[m_LevelIndex];

            if (m_BranchLevelIndex >= 0)
                m_BranchLevelProperties = LevelSequencesController.Instance.LevelSequences.BranchLevelsProperties[m_BranchLevelIndex];
            m_ResultPanel.gameObject.SetActive(false);
        }

        public void EX_LoadLevel()
        {
            if (m_LevelIndex >= 0)
                SceneManager.LoadScene(m_LevelProperties.SceneName);

            if (m_BranchLevelIndex >= 0)
                SceneManager.LoadScene(m_BranchLevelProperties.SceneName);
        }

        public void SetLevelData(string episodeName, int levelScore)
        {
            if (m_LevelProperties != null)
                m_LevelProperties.SceneName = episodeName;

            if (m_BranchLevelProperties != null)
                m_BranchLevelProperties.SceneName = episodeName;

            m_ResultPanel.gameObject.SetActive(levelScore > 0);

            for (int i = 0; i < levelScore; i++)
            {
                m_ResultImage[i].color = Color.white;
            }
        }
    }
}