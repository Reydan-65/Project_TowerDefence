using UnityEngine;
using SpaceShooter;

namespace Common
{
    public enum TurretMode
    {
        Primary,
        Secondary,
        Auto
    }

    public abstract class TurretPropertiesBase : ScriptableObject
    {
        [SerializeField] private TurretMode m_Mode;
        [SerializeField] private Projectile m_ProjectilePrefab;
        [SerializeField] private float m_RateOfFire;

        public TurretMode Mode => m_Mode;
        public Projectile ProjectilePrefab => m_ProjectilePrefab;
        public float RateOfFire => m_RateOfFire;
    }
}