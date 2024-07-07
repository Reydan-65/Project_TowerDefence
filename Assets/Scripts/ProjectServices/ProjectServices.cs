using TowerDefence;
using UnityEngine;

namespace SpaceShooter
{
    public class ProjectServices : MonoBehaviour
    {
        [SerializeField] private LevelSequencesController m_LevelSequencesController;
        private MapCompletion m_MapCompletion;
        private Upgrades m_Upgrades;

        private void Awake()
        {
            m_LevelSequencesController.Init();

            m_MapCompletion = FindObjectOfType<MapCompletion>();

            if (m_MapCompletion != null)
                m_MapCompletion.Init();

            m_Upgrades = FindObjectOfType<Upgrades>();

            if (m_Upgrades != null)
                m_Upgrades.Init();
        }
    }
}