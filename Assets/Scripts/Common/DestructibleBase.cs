using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// ������������ ������ �� �����. �� ��� ����� ����� ���������.
    /// </summary>
    public class DestructibleBase : Entity
    {
        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        #region Properties

        /// <summary>
        /// ������ ���������� �����������.
        /// </summary>
        [SerializeField] protected bool m_Indestructible;
        public bool IsIndestructible { get => m_Indestructible; set => m_Indestructible = value; }

        /// <summary>
        /// ��������� ���������� ����������.
        /// </summary>
        [Header("Start Properties")]
        [SerializeField] protected int m_HitPoints;
        public int MaxHitPoints => m_HitPoints;

        /// <summary>
        /// ������� ���������.
        /// </summary>
        protected int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;

        #endregion

        #region Unity Events 

        protected virtual void Start()
        {
            transform.SetParent(null);

            m_CurrentHitPoints = m_HitPoints;
        }

        #endregion

        #region Public API

        /// <summary>
        /// ���������� ����� � �������.
        /// </summary>
        /// <param name="damage"> ���� ��������� ������� </param>
        public void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;

            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0)
            {
                m_CurrentHitPoints = 0;
                OnDeath();
            }
        }

        #endregion

        /// <summary>
        /// ���������������� ������� ����������� �������, ����� ��������� ����� ����
        /// </summary>
        protected virtual void OnDeath()
        {
            m_EventOnDeath?.Invoke();
        }

        #region DESTRUCTIBLE_LIST_TEAMS

        [SerializeField] protected int m_TeamId;

        public int TeamId { get => m_TeamId; set => m_TeamId = value; }

        /// <summary>
        /// �������� ���� ����������� �������� � ������.
        /// �������� �� ������� ������, ���� ������ ���������.
        /// ���������� �� ������.
        /// </summary>
        private static HashSet<DestructibleBase> m_AllDestructible;
        public static IReadOnlyCollection<DestructibleBase> AllDestructible => m_AllDestructible;

        protected virtual void OnEnable()
        {
            if (m_AllDestructible == null)
                m_AllDestructible = new HashSet<DestructibleBase>();

            m_AllDestructible.Add(this);
        }

        protected virtual void OnDestroy()
        {
            if (m_AllDestructible != null)
                m_AllDestructible.Remove(this);
        }

        public const int TeamId_Neutral = 0;
        public const int TeamId_Allies = 1;
        public const int TeamId_Enemies = 2;
        public const int TeamId_Debris = 3;

        #endregion

    }
}