using UnityEngine;
using SpaceShooter;
using TowerDefence;

namespace Common
{
    public abstract class TurretBase : MonoBehaviour
    {
        [SerializeField] protected TurretMode m_Mode;
        [SerializeField] protected TurretProperties m_TurretProperties;
        [SerializeField] protected TowerAsset m_TowerAsset;

        public TurretMode Mode => m_Mode;
        public TurretProperties TurretProperties => m_TurretProperties;
        public TowerAsset TowerAsset { get => m_TowerAsset; set => m_TowerAsset = value; }

        protected virtual void FixedUpdate() { }

        // Public API
        public virtual void Fire() { }
        public virtual void AssingLoadout(TurretProperties properties) { }
    }
}