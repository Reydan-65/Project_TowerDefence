using UnityEngine;
using SpaceShooter;
using Common;

namespace TowerDefence
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private CircleArea m_StartArea;
        [SerializeField] private AI_PatrolArea[] m_PatrolAreas;

        public CircleArea StartArea => m_StartArea;
        public int Lenght { get => m_PatrolAreas.Length; }
        public AI_PatrolArea this[int i] { get => m_PatrolAreas[i]; }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            if (m_PatrolAreas != null)
            {
                Gizmos.color = Color.green;

                for (int i = 0; i < m_PatrolAreas.Length; i++)
                {
                    if (i >= m_PatrolAreas.Length - 1) return;

                    Gizmos.DrawLine(m_PatrolAreas[i].transform.position, m_PatrolAreas[i + 1].transform.position);
                }

                //if (m_PatrolAreas.Length > 1)
                //{
                //    Gizmos.DrawLine(m_PatrolAreas[m_PatrolAreas.Length - 1].transform.position, m_PatrolAreas[0].transform.position);
                //}
            }
        }

#endif

    }
}