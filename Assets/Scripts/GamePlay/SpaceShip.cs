using UnityEngine;
using Common;
using TowerDefence;
using System;

namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        //[SerializeField] private Player m_Player;

        /// <summary>
        /// m_Mass - Масса для автоматической установки у твердого тела.
        /// m_Thrust - Толкающая вперёд сила.
        /// m_Mobility - Вращающая сила.
        /// m_MaxLinearVelocity - Максимальная линейная скорость.
        /// m_MaxAngularVelocity - Максимальная вращательная скорость. В градусах/сек.
        /// m_ShipPreview - Сохраненная ссылка на превью корабля
        /// </summary>
        [Space(10)]
        [Header("Space Ship")]
        [SerializeField] private float m_Mass;
        [SerializeField] private float m_Thrust;
        [SerializeField] private float m_Mobility;
        [SerializeField] private float m_MaxLinearVelocity;
        private float m_MaxLinearVelocityBackup;
        [SerializeField] private float m_MaxAngularVelocity;
        [SerializeField] private Sprite m_ShipPreview;

        [Space(10)]
        [SerializeField] private Turret[] m_Turrets;

        [Space(10)]
        [SerializeField] private int m_MaxEnergy;
        [SerializeField] private int m_MaxAmmo;
        [SerializeField] private int m_EnergyRegenPerSecond;

        [Space(10)]
        [SerializeField] private GameObject impactPrefab;

        private bool isDead = false;
        public bool IsDead { get => isDead; set => isDead = value; }

        //private float m_PrimaryEnergy;
        //private int m_SecondaryAmmo;
        //public float PrimaryEnergy => m_PrimaryEnergy;
        //public int SecondaryAmmo { get => m_SecondaryAmmo; set => m_SecondaryAmmo = value; }
        public int MaxEnergy => m_MaxEnergy;
        public int MaxAmmo => m_MaxAmmo;

        private Rigidbody2D m_Rigid;
        private SpriteRenderer m_SpriteProperties;

        public Rigidbody2D Rigid { get => m_Rigid; set => m_Rigid = value; }
        public SpriteRenderer SpriteProperties => m_SpriteProperties;
        public float Thrust { get => m_Thrust; set => m_Thrust = value; }
        public float Mobility { get => m_Mobility; set => m_Mobility = value; }
        public Sprite ShipPreview => m_ShipPreview;

        #region Public API

        /// <summary>
        /// Управление линейной тягой. -1.0 до +1.0
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// Управление вращательной тягой. -1.0 до +1.0
        /// </summary>
        public float TorqueControl { get; set; }

        #endregion

        #region Unity Events

        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertia = 1;

            if (GetComponentInChildren<TrailRenderer>() != null)
                GetComponentInChildren<TrailRenderer>().enabled = true;

            //InitOffensive();
        }

        private void FixedUpdate()
        {
            UpdateRigidBody();

            //UpdateEnergyRegen();
        }

        #endregion

        /// <summary>
        /// Метод добавления сил кораблю для движения.
        /// </summary>
        private void UpdateRigidBody()
        {
            m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);
            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);
            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            if (m_MaxLinearVelocity < 0.2f) m_MaxLinearVelocity = 0.2f;
            if (m_MaxAngularVelocity < 0.2f) m_MaxAngularVelocity = 0.2f;
        }

        /// <summary>
        /// Запоминание максимальной линейной скорости и
        /// её изменение на value.
        /// </summary>
        public void ChangeMaxLinearVelocityOnValue(float value)
        {
            if (m_MaxLinearVelocityBackup < m_MaxLinearVelocity)
                m_MaxLinearVelocityBackup = m_MaxLinearVelocity;

            m_MaxLinearVelocity *= value;
        }

        /// <summary>
        /// Восстановление максимальной линейной скорости из бэкапа.
        /// </summary>
        public void RestoreMaxLinearVelocityOnValue() { m_MaxLinearVelocity = m_MaxLinearVelocityBackup; }

        /// <summary>
        /// TODO: заменить временный метод-заглушку
        /// Используется ИИ.
        /// </summary>
        public void Fire(TurretMode mode)
        {
            //for (int i = 0; i < m_Turrets.Length; i++)
            //{
            //    if (m_Turrets[i].Mode == mode)
            //    {
            //        m_Turrets[i].Fire();
            //    }
            //}
            return;
        }

        /*
        #region Offensive

        /// <summary>
        /// Ограничение стрельбы.
        /// </summary>
        public void AddEnergy(int energy)
        {
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + energy, 0, m_MaxEnergy);
        }

        public void AddAmmo(int ammo)
        {
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        }

        public void RepairShip(int hitPoints)
        {
            m_CurrentHitPoints = Mathf.Clamp(m_CurrentHitPoints + hitPoints, 0, m_HitPoints);
        }

        private void InitOffensive()
        {
            if (this == Player.Instance.ActiveShip)
            {
                m_PrimaryEnergy = m_MaxEnergy / 2;
                m_SecondaryAmmo = 0;
            }
            else
            {
                m_PrimaryEnergy = m_MaxEnergy;
                m_SecondaryAmmo = m_MaxAmmo;
            }
        }

        private void UpdateEnergyRegen()
        {
            m_PrimaryEnergy += (float)m_EnergyRegenPerSecond * Time.fixedDeltaTime;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
        }

        #endregion

        /// <summary>
        /// Изменение свойств туррели.
        /// </summary>
        public void AssingWeapon(TurretProperties properties)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                m_Turrets[i].AssingLoadout(properties);
            }
        }
        */

        /// <summary>
        /// TODO: заменить временный метод-заглушку.
        /// Используется турелями
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>

        public bool DrawEnergy(int count)
        {
            return true;
        }

        /// <summary>
        /// TODO: заменить временный метод-заглушку.
        /// Используется турелями
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool DrawAmmo(int count)
        {
            return true;
        }

        /// <summary>
        /// Взрыв корабля.
        /// </summary>
        protected override void OnDeath()
        {
            base.OnDeath();
            //if (CompareTag("Boss") == true)
            //    Player.Instance.AddBossDead();
            Vector3 position = new Vector3(transform.root.position.x, transform.root.position.y, 0);
            Instantiate(impactPrefab, position,Quaternion.identity);
            isDead = true;
        }

        public new void Use(EnemyAsset asset)
        {
            m_MaxLinearVelocity = asset.MoveSpeed;

            base.Use(asset);
        }
    }
}