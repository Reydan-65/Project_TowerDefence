using UnityEngine;
using Common;

namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]
    public class AI_Controller : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            PatrolArea,
            PatrolRoute
        }

        [SerializeField] private AIBehaviour m_AIBehaviour;
        public AIBehaviour _AIBehaviour => m_AIBehaviour;

        [Space(10)]
        [Header("Patrol Properties")]
        [SerializeField] private AI_PatrolArea m_PointPatrol;
        [SerializeField] private Transform[] m_PatrolRoutePoint;
        public AI_PatrolArea PointPatrol { get => m_PointPatrol; set => m_PointPatrol = value; }

        private int currentPatrolIndex = 0;

        [Space(10)]
        [Header("Navigation Properties")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear;
        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;

        [Space(10)]
        [Header("Pathfind")]
        [SerializeField] private float m_RandomSelectMovePointTime;
        [SerializeField] private float m_EvadeRayLenght;
        [SerializeField] private float m_EvadeMinRadius;

        private Vector3 m_MovePosition;

        [Space(10)]
        [Header("Projectile")]
        [SerializeField] private GameObject m_Projectile;

        [Space(10)]
        [Header("Target detection and chasing")]
        [SerializeField] private float m_DetectionRadiusWithAngle;
        [SerializeField] private float m_DetectionRadiusWithoutAngle;
        [Range(0f, 360f)]
        [SerializeField] private float m_DetectionAngle;
        [SerializeField] private float m_AttackDistance;
        [SerializeField] private float m_FindNewTargetTime;

        private Destructible m_SelectedTarget;
        public Destructible SelectedTarget => m_SelectedTarget;

        [Space(10)]
        [Header("Fire Rate")]
        [SerializeField] private float m_PrimaryTurretShootDelay;
        [SerializeField] private float m_SecondaryTurretShootDelay;

        // �������
        private Timer m_RandomizeDirectionTimer;
        private Timer m_PrimaryTurretFireTimer;
        private Timer m_SecondaryTurretFireTimer;
        private Timer m_FindNewTargetTimer;

        // �������� ��������� ������
        private float m_FinishTimerEarly = 0f;

        /// <summary>
        /// ������� AI.
        /// �������, ��� ������� AI ��� ������ Spawner'��
        /// </summary>
        private SpaceShip m_SpaceShip;
        private bool m_AISpawnedShip = false;
        public bool AISpawnedShip { get => m_AISpawnedShip; set => m_AISpawnedShip = value; }

        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();

            m_MovePosition = transform.position;

            InitTimers();
        }

        private void FixedUpdate()
        {
            UpdateTimers();
            UpdateAI();
        }

        private void UpdateAI()
        {
            if (m_AIBehaviour == AIBehaviour.Null) return;

            if (m_AIBehaviour == AIBehaviour.PatrolArea ||
                m_AIBehaviour == AIBehaviour.PatrolRoute)
                UpdateBehaviourPatrol();
        }

        public void UpdateBehaviourPatrol()
        {
            if (m_SpaceShip.IsDead == true) return;

            Action_FindNewMovePosition();
            //Action_EvadeCollision();
            Action_ControlShip();
            Action_FindNewAttackTarget();
            Action_Fire();
        }

        #region AI ACTIONS

        private void Action_FindNewMovePosition()
        {
            if (m_SelectedTarget != null)
                m_MovePosition = m_SelectedTarget.transform.position;
            else
            {
                if (m_AIBehaviour == AIBehaviour.PatrolArea) PatrolArea();
                if (m_AIBehaviour == AIBehaviour.PatrolRoute) PatrolRoute();
            }
        }

        private void Action_EvadeCollision()
        {
            if (m_SelectedTarget != null) return;

            RaycastHit2D hitCollision = Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLenght);

            Vector2 targetPosition = m_MovePosition;

            if (m_AIBehaviour == AIBehaviour.PatrolArea)
            {
                if (hitCollision)
                {
                    float time = m_RandomSelectMovePointTime;

                    targetPosition = transform.position + transform.right * 1.0f;
                    m_RandomSelectMovePointTime = m_FinishTimerEarly;
                    m_RandomSelectMovePointTime = time;
                }

                m_MovePosition = targetPosition;
            }
        }

        private void Action_ControlShip()
        {
            /*
            RaycastHit2D hitForward = Physics2D.Raycast(transform.position, transform.up, m_EvadeMinRadius);
            RaycastHit2D hitBackward = Physics2D.Raycast(transform.position, transform.up, -m_EvadeMinRadius);

            if (hitForward)
            {
                if (m_SelectedTarget != null)
                {
                    if (m_NavigationLinear > 0)
                    {
                        if (hitBackward == false)
                            m_SpaceShip.ThrustControl = -m_NavigationLinear;
                        else
                            m_SpaceShip.ThrustControl = m_NavigationLinear;
                    }
                }
            }
            else
                */
            m_SpaceShip.ThrustControl = m_NavigationLinear;
            /*
            if (m_SelectedTarget != null)
            {
                if (m_Projectile.GetComponent<Projectile>() == true)
                    m_MovePosition = MakeLead(m_SelectedTarget.transform.localPosition,
                                              m_Projectile.GetComponent<Projectile>().Velocity,
                                              transform.localPosition, m_SelectedTarget);
            }
            */
            m_SpaceShip.TorqueControl = ComputeAliginTorqueNormalized(m_MovePosition, m_SpaceShip.transform) * m_NavigationAngular;
        }

        private void Action_FindNewAttackTarget()
        {
            if (m_FindNewTargetTimer.IsFinished == true)
            {
                m_SelectedTarget = FindNearestDestructibleTarget();

                m_FindNewTargetTimer.Start(m_PrimaryTurretShootDelay);
            }
        }

        private void Action_Fire()
        {
            if (m_SelectedTarget != null)
            {
                Vector2 dist = transform.position - m_SelectedTarget.transform.position;

                float angle = Vector3.Angle(m_SpaceShip.transform.up, m_SelectedTarget.transform.position - m_SpaceShip.transform.position);

                if (dist.magnitude < m_AttackDistance && angle < 0f)
                {
                    if (m_PrimaryTurretFireTimer.IsFinished == true)
                    {
                        if (m_SelectedTarget.TryGetComponent(out SpaceShip ship) == true)
                            if (ship.IsDead == true) return;

                        //TurretProperties properties = m_SpaceShip.transform.root.GetChild(2).GetComponent<Turret>().TurretProperties;

                        //if (m_SpaceShip.PrimaryEnergy >= properties.EnergyUsage * 2)
                        //{
                        m_SpaceShip.Fire(TurretMode.Primary);

                        m_PrimaryTurretFireTimer.Start(m_PrimaryTurretShootDelay);
                        //}
                    }

                    /*
                    if (m_SecondaryTurretFireTimer.IsFinished == true)
                    {
                        if (m_SelectedTarget.TryGetComponent(out SpaceShip ship) == true)
                            if (ship.IsDead == true) return;

                        m_SpaceShip.Fire(TurretMode.Secondary);
                        //m_SpaceShip.AddAmmo(2);  // ��������� ������ ����� ����������
                        m_SecondaryTurretFireTimer.Start(m_SecondaryTurretShootDelay);
                    }
                    */
                }
            }
        }

        #endregion

        #region AREA_PATROL
        /// <summary>
        /// �������� ��������� �� �� � ���� ��������������
        /// ���� ��������� � ������� ���� ��������������, �� �� ������ � ���� ����� � ���� ��������������
        /// ����� ������������ � ���� ��������������
        /// </summary>
        private void PatrolArea()
        {
            if (m_PointPatrol != null)
            {
                bool isInsidePatrolZone = (m_PointPatrol.transform.position - transform.position).sqrMagnitude < m_PointPatrol.Radius * m_PointPatrol.Radius;

                if (isInsidePatrolZone == true)
                {
                    GetNewPatrolPoint();
                }
                else
                {
                    m_MovePosition = m_PointPatrol.transform.position;
                }
            }
        }

        protected virtual void GetNewPatrolPoint()
        {
            Vector2 newPoint = Random.onUnitSphere * m_PointPatrol.Radius + m_PointPatrol.transform.position;

            if (Vector2.Distance(transform.position, m_MovePosition) <= 1.0f)
            {
                newPoint = Random.onUnitSphere * m_PointPatrol.Radius + m_PointPatrol.transform.position;

                m_MovePosition = newPoint;

                m_RandomizeDirectionTimer.ResetTimer(m_RandomSelectMovePointTime);
            }
            else
            if (m_RandomizeDirectionTimer.IsFinished == true)
            {
                m_MovePosition = newPoint;

                m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);
            }
        }

        #endregion

        #region ROUTE_PATROL

        // ������� �� ������, ����������� � ����������
        private void PatrolRoute()
        {
            Transform currentPoint = m_PatrolRoutePoint[currentPatrolIndex].GetComponent<Transform>();

            m_MovePosition = m_PatrolRoutePoint[currentPatrolIndex].transform.position;

            if (Vector2.Distance(transform.position, currentPoint.position) <= 1.0f)
            {
                currentPatrolIndex++;

                if (currentPatrolIndex >= m_PatrolRoutePoint.Length) currentPatrolIndex = 0;
            }
        }

        #endregion

        #region FindNearestPriorityTarget

        /// <summary>
        /// ����� ������������ ����.
        /// m_DetectionRadiusWithoutAngle - ������ ����������� ��� �����������.
        /// m_DetectionRadiusWithAngle, m_DetectionAngle - ������ � ���� ������ ��� �����������.
        /// ������� ������ ������������ �������� � ������� ����������� ����.
        /// �������� ��������� ������������ ���� (ship).
        /// ���� ship ����� null, �������� ������ ������������ ������
        /// </summary>
        /// <returns></returns>
        private Destructible FindNearestDestructibleTarget()
        {
            float maxDist = m_DetectionRadiusWithoutAngle;

            Destructible potentialTarget = null;

            foreach (var target in Destructible.AllDestructible)
            {
                if (target.GetComponent<SpaceShip>() == m_SpaceShip) continue;
                if (target.TeamId == Destructible.TeamId_Neutral) continue;
                if (target.TeamId == m_SpaceShip.TeamId) continue;

                Vector2 toTarget = target.transform.position - m_SpaceShip.transform.position;

                float dist = toTarget.magnitude;

                float angle = Vector3.Angle(m_SpaceShip.transform.up, target.transform.position - m_SpaceShip.transform.position);

                RaycastHit2D hit = Physics2D.Raycast(m_SpaceShip.transform.position,
                                                     target.transform.position - m_SpaceShip.transform.position,
                                                     m_DetectionRadiusWithAngle);
                if (hit)
                {
                    // ���� ���� � ������� ��� ���� ����������� ��� � ������� � ����� �����������
                    if (dist <= maxDist /* ||
                        angle < m_DetectionAngle / 2f */ &&
                        Vector3.Distance(m_SpaceShip.transform.position, target.transform.position) <= m_DetectionRadiusWithAngle &&
                        hit.transform == target.transform)
                    {
                        if (target is SpaceShip)
                        {
                            maxDist = dist;
                            potentialTarget = (Destructible)target;
                        }
                        else
                        if (potentialTarget == null && target as SpaceShip == null)
                        {
                            maxDist = dist;
                            potentialTarget = (Destructible)target;
                        }
                    }
                }
            }

            return potentialTarget;
        }

        #endregion

        // ��������� ������������� ���� ��������
        private const float MAX_ANGLE = 45.0f;

        // ����� ������� ���� �������� ������� � ������� �����
        private static float ComputeAliginTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);

            float angle;

            angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);

            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;

            return -angle;
        }

        // ���������� ���������� ����
        private Vector2 MakeLead(Vector2 targetPos, float projectileSpeed, Vector2 shootPosition, Destructible target)
        {
            Vector2 relativePosition = targetPos - shootPosition;

            float distance = relativePosition.magnitude;
            float timeToIntercept = distance / projectileSpeed;

            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();

            Vector2 targetPoint = targetPos + (rb.velocity * timeToIntercept);

            return targetPoint;
        }

        public void SetPatrolBehaviour(AI_PatrolArea point)
        {
            m_AIBehaviour = AIBehaviour.PatrolArea;
            m_PointPatrol = point;
        }

        #region TIMERS

        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
            m_PrimaryTurretFireTimer = new Timer(m_PrimaryTurretShootDelay);
            m_SecondaryTurretFireTimer = new Timer(m_SecondaryTurretShootDelay);
            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
        }

        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);
            m_PrimaryTurretFireTimer.RemoveTime(Time.deltaTime);
            m_SecondaryTurretFireTimer.RemoveTime(Time.deltaTime);
            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);
        }

        #endregion

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            // ��������� �����, � ������� �������� ������
            if (m_MovePosition == Vector3.zero) return;
            else
            {
                Gizmos.color = new Color(0, 1, 1, 0.1f);
                Gizmos.DrawLine(transform.position, m_MovePosition);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(m_MovePosition, 0.2f);
            }
        }

        private void OnDrawGizmosSelected()
        {
            /*
            // ��������� ������ � ���� ������ ��� ����������� ����
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, m_DetectionRadiusWithAngle);

            Vector2 left = transform.localPosition + Quaternion.Euler(new Vector3(0, 0, m_DetectionAngle / 2f)) * (transform.up * m_DetectionRadiusWithAngle);
            Vector2 right = transform.localPosition + Quaternion.Euler(-new Vector3(0, 0, m_DetectionAngle / 2f)) * (transform.up * m_DetectionRadiusWithAngle);

            Gizmos.DrawLine(transform.position, left);
            Gizmos.DrawLine(transform.position, right);
            */
            // ��������� ������ ��� ����������� ����
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_DetectionRadiusWithoutAngle);

            // ��������� ������ ���������� ������������ � �����
            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(transform.position, m_EvadeMinRadius);

            // ��������� ������ ���������� ������������ � �������������
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, m_EvadeRayLenght);

            // ��������� ������ ��� ����� ����
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_AttackDistance);
        }

#endif

    }
}