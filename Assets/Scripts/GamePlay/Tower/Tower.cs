using UnityEngine;
using SpaceShooter;
using Common;
using System.Linq;

namespace TowerDefence
{
    public class Tower : Entity
    {
        private static readonly Color GizmoColor = new Color(0, 1, 1, 0.2f);
        
        [SerializeField] private float m_Radius;

        private Turret[] m_Turrets;
        private Turret m_FirstTurret;
        private Destructible m_SelectedTarget = null;
        public Destructible SelectedTarget => m_SelectedTarget;

        private Transform m_PlayerCamp;
        public float Radius => m_Radius;

        private Vector2 m_TargetPoint;
        public Vector2 TargetPoint => m_TargetPoint;

        private void Start()
        {
            m_Turrets = GetComponentsInChildren<Turret>();
            m_PlayerCamp = GameObject.FindGameObjectWithTag("Player").transform;

            if (m_Turrets.Length > 0)
                m_FirstTurret = m_Turrets[0];
        }

        private void Update()
        {
            if (m_SelectedTarget != null)
            {
                if (CanAttackTarget())
                {
                    Vector2 targetVector = m_SelectedTarget.GetComponentInChildren<Collider2D>().transform.position - m_FirstTurret.transform.position;

                    if (targetVector.magnitude <= m_Radius)
                    {
                        m_TargetPoint = MakeLead(targetVector, m_FirstTurret.TurretProperties.ProjectilePrefab.GetComponent<Projectile>().Velocity,
                                                 m_FirstTurret.transform.localPosition, m_SelectedTarget);
                        m_FirstTurret.transform.up = m_TargetPoint;
                        m_FirstTurret.Fire();
                    }
                    else
                    {
                        m_SelectedTarget = null;
                    }
                }
            }
            else
            {
                var enter = Physics2D.OverlapCircle(transform.position, m_Radius);

                if (enter)
                    m_SelectedTarget = enter.transform.root.GetComponent<Destructible>();
            }
        }

        /// <summary>
        /// Получение нового типа башни, до которого можно улучшить текущую.
        /// </summary>
        public void Use(TowerAsset towerAsset)
        {
            GetComponentInChildren<SpriteRenderer>().sprite = towerAsset.Sprite;
            GetComponentInChildren<Turret>().TurretProperties = towerAsset.TurretProperties;

            GetComponentInChildren<BuildPoint>().SetBuildableTowers(towerAsset.m_UpgradesTo);
        }

        // Упреждение цели для турели
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

            var enemyType = m_SelectedTarget.GetComponent<Enemy>().UType;
            return m_Turrets.Any(turret =>
                turret.TowerAsset.Type == TowerAsset.TargetType.All ||
                (enemyType == Enemy.UnitType.Ground && turret.TowerAsset.Type == TowerAsset.TargetType.Ground) ||
                (enemyType == Enemy.UnitType.Air && turret.TowerAsset.Type == TowerAsset.TargetType.Air));
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