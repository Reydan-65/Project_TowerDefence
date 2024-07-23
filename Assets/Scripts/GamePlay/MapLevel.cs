using UnityEngine;
using SpaceShooter;
using UnityEngine.UI;

namespace TowerDefence
{
    public class MapLevel : MonoBehaviour
    {
        private LevelProperties m_LevelProperties;
        private BranchLevelProperties m_BranchLevelProperties;
        private SceneTransitionManager m_SceneTransitionManager;

        [SerializeField] private RectTransform m_ResultPanel;
        [SerializeField] private Image[] m_ResultImage;

        public RectTransform ResultPanel { get { return m_ResultPanel; } }
        public Image[] ResultImage => m_ResultImage;

        private int m_LevelIndex;
        private int m_BranchLevelIndex;

        //Если панель результата уровня активна, уровень считается пройденым...
        public bool IsComplete
        {
            get
            {
                return gameObject.activeSelf && m_ResultPanel.gameObject.activeSelf;
            }
        }

        private void Awake()
        {
            SetScene();

            m_SceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
            m_ResultPanel.gameObject.SetActive(false);
        }

        //Назначаем каждому уровню соответствующую его типу и индексу сцену.
        private void SetScene()
        {
            int index = -1;

            m_LevelIndex = index;
            m_BranchLevelIndex = index;

            if (TryGetComponent(out MapBranchLevel branch) == false)
                m_LevelIndex = transform.GetSiblingIndex();
            else
                m_BranchLevelIndex = transform.GetSiblingIndex();

            if (m_LevelIndex >= 0)
                m_LevelProperties = LevelSequencesController.Instance.LevelSequences.LevelsProperties[m_LevelIndex];
            else if (m_BranchLevelIndex >= 0)
                m_BranchLevelProperties = LevelSequencesController.Instance.LevelSequences.BranchLevelsProperties[m_BranchLevelIndex];
        }

        public void EX_LoadLevel()
        {
            if (m_LevelIndex >= 0)
                m_SceneTransitionManager.LoadScene(m_LevelProperties.SceneName);

            if (m_BranchLevelIndex >= 0)
                m_SceneTransitionManager.LoadScene(m_BranchLevelProperties.SceneName);
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