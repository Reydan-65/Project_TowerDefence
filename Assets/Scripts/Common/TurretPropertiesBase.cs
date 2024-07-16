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
        [SerializeField] private int m_ProjectileDamage;
        [SerializeField] private int m_ProjectileExplosionDamage;
        [SerializeField] private float m_ProjectileExplosionRadius;

        public TurretMode Mode => m_Mode;
        public Projectile ProjectilePrefab => m_ProjectilePrefab;
        public float RateOfFire => m_RateOfFire;
        public int ProjectileDamage => m_ProjectileDamage;
        public int ProjectileExplosionDamage => m_ProjectileExplosionDamage;
        public float ProjectileExplosionRadius => m_ProjectileExplosionRadius;
    }
}