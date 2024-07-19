using TowerDefence;
using UnityEngine;

namespace SpaceShooter
{
    public class ProjectServices : MonoBehaviour
    {
        [SerializeField] private LevelSequencesController m_LevelSequencesController;
        [SerializeField] private MapCompletion m_MapCompletion;
        [SerializeField] private Upgrades m_Upgrades;
        [SerializeField] private SoundPlayer m_SoundPlayer;

        private void Awake()
        {
            m_LevelSequencesController.Init();
            m_SoundPlayer.Init();

            if (m_MapCompletion != null)
                m_MapCompletion.Init();

            if (m_Upgrades != null)
                m_Upgrades.Init();
        }
    }
}