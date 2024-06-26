using UnityEngine;
using SpaceShooter;
using Common;

namespace TowerDefence
{
    public class Tower : Entity
    {
        [SerializeField] private float m_Radius;

        private Turret[] m_Turrets;
        private Destructible m_SelectedTarget = null;
        public Destructible SelectedTarget => m_SelectedTarget;

        private Transform m_PlayerCamp;
        public float Radius => m_Radius;

        private static readonly Color GizmoColor = new Color(0, 1, 1, 0.2f);

        private void Start()
        {
            m_Turrets = GetComponentsInChildren<Turret>();
            m_PlayerCamp = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        private void Update()
        {
            if (m_SelectedTarget != null)
            {
                if (CanAttackTarget())
                {
                    Vector2 targetVector = m_SelectedTarget.GetComponentInChildren<Collider2D>().transform.position - GetComponentInChildren<Turret>().transform.position;

                    if (targetVector.magnitude <= m_Radius)
                    {
                        foreach (var turret in m_Turrets)
                        {
                            turret.transform.up = MakeLead(targetVector, GetComponentInChildren<Turret>().TowerAsset.ProjectilePrefab.GetComponent<Projectile>().Velocity,
                                                           GetComponentInChildren<Turret>().transform.localPosition, m_SelectedTarget);
                            turret.Fire();
                        }
                    }
                    else
                        m_SelectedTarget = null;
                }
                else
                {
                    m_SelectedTarget = null;
                }
            }
            else
            {
                var enter = Physics2D.OverlapCircle(transform.position, m_Radius);

                if (enter)
                    m_SelectedTarget = enter.transform.root.GetComponent<Destructible>();
            }
        }

        // ”преждение цели дл€ турели
        private Vector2 MakeLead(Vector2 targetPos, float projectileSpeed, Vector2 shootPosition, Destructible target)
        {
            Vector2 relativePosition = targetPos - shootPosition;

            float distance = relativePosition.magnitude;
            float timeToIntercept = distance / projectileSpeed;

            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();

            Vector2 targetPoint = targetPos + (rb.velocity * timeToIntercept);

            return targetPoint;
        }

        public bool CanAttackTarget()
        {
            if (m_SelectedTarget == null) return false;

            foreach (var turret in m_Turrets)
            {
                if (m_SelectedTarget.GetComponent<Enemy>().Type == Enemy.UnitType.Ground)
                {
                    if (turret.TowerAsset.Type == TowerAsset.TargetType.All ||
                        turret.TowerAsset.Type == TowerAsset.TargetType.Ground) return true;
                }
                else if (m_SelectedTarget.GetComponent<Enemy>().Type == Enemy.UnitType.Air)
                {
                    if (turret.TowerAsset.Type == TowerAsset.TargetType.All ||
                            turret.TowerAsset.Type == TowerAsset.TargetType.Air) return true;
                }
            }

            return false;
        }


#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = GizmoColor;
            Gizmos.DrawWireSphere(transform.position, m_Radius);
        }

#endif
    }
}