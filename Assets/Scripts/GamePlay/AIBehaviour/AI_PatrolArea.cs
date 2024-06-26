using UnityEngine;

namespace SpaceShooter
{
    public class AI_PatrolArea : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        public float Radius => m_Radius;

        private static readonly Color GizmoColor = new Color(1, 0, 0, 0.2f);

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = GizmoColor;
            Gizmos.DrawSphere(transform.position, m_Radius);
        }

#endif

    }
}