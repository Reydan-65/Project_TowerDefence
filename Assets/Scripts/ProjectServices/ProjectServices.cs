using TowerDefence;
using UnityEngine;

namespace SpaceShooter
{
    public class ProjectServices : MonoBehaviour
    {
        [SerializeField] private LevelSequencesController m_LevelSequencesController;
        private MapCompletion m_MapCompletion;

        private void Awake()
        {
            m_LevelSequencesController.Init();

            m_MapCompletion = FindAnyObjectByType<MapCompletion>();
            m_MapCompletion.Init();
        }
    }
}