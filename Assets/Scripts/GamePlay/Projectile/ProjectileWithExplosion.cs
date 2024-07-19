using TowerDefence;
using UnityEngine;

namespace SpaceShooter
{
    public class ProjectileWithExplosion : Projectile
    {
        [SerializeField] protected DebuffEffect m_DebuffPrefab;

        protected float m_ExplosionRadius;
        protected int m_ExplosionDamage;

        protected Explosion m_Explosion;

        public float ExplosionRadius { get => m_ExplosionRadius; set => m_ExplosionRadius = value; }
        public int ExplosionDamage { get => m_ExplosionDamage; set => m_ExplosionDamage = value; }
    }
}