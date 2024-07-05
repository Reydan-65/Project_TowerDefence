using UnityEngine;
using UnityEngine.Events;
using SpaceShooter;

namespace TowerDefence
{
    public class TD_PatrolController : AI_Controller
    {
        private Path m_Path;
        private int pathIndex;

        [SerializeField] private UnityEvent OnEndPath;

        public void SetPath(Path newPath)
        {
            m_Path = newPath;
            pathIndex = 0;
            SetPatrolBehaviour(m_Path[pathIndex]);
        }

        protected override void GetNewPatrolPoint()
        {
            pathIndex += 1;

            if (m_Path.Lenght > pathIndex)
            {
                SetPatrolBehaviour(m_Path[pathIndex]);
            }
            else
            {
                OnEndPath.Invoke();
                //TD_Player.Instance.ReduceEnemiesLast();
                Destroy(gameObject);
            }
        }
    }
}