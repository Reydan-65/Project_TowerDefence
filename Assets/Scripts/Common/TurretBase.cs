using UnityEngine;
using SpaceShooter;
using TowerDefence;

namespace Common
{
    public abstract class TurretBase : MonoBehaviour
    {
        [SerializeField] protected TurretMode m_Mode;

        protected TurretProperties m_TurretProperties;
        protected TowerAsset m_TowerAsset;

        public TurretMode Mode => m_Mode;

        /// <summary>
        /// TowerAsset и TurretProperties назначаются при создании башни
        /// </summary>
        public TowerAsset TowerAsset { get => m_TowerAsset; set => m_TowerAsset = value; }
        public TurretProperties TurretProperties { get => m_TurretProperties; set => m_TurretProperties = value; }

        protected virtual void FixedUpdate() { }

        public virtual void Fire() { }
        public virtual void AssingLoadout(TurretProperties properties) { }
    }
}