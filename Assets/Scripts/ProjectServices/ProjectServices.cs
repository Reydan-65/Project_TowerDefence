using Common;
using TowerDefence;
using UnityEngine;

namespace SpaceShooter
{
    public class ProjectServices : MonoBehaviour
    {
        [SerializeField] private LevelSequencesController m_LevelSequencesController;
        [SerializeField] private MapCompletion m_MapCompletion;
        [SerializeField] private Upgrades m_Upgrades;
        [SerializeField] private SoundManager m_SoundManager;

        private void Awake()
        {
            m_LevelSequencesController.Init();
            m_SoundManager.Init();

            if (m_MapCompletion != null)
                m_MapCompletion.Init();

            if (m_Upgrades != null)
                m_Upgrades.Init();
        }
    }
}