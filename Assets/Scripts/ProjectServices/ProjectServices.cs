using UnityEngine;

namespace SpaceShooter
{
    public class ProjectServices : MonoBehaviour
    {
        [SerializeField] private LevelSequencesController m_LevelSequencesController;
        
        private void Awake()
        {
            m_LevelSequencesController.Init();    
        }
    }
}