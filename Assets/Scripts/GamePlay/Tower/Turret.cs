using UnityEngine;
using Common;
using TowerDefence;

namespace SpaceShooter
{
    public class Turret : TurretBase
    {
        private Tower m_Tower;
        private float m_RefireTimer;
        
        public bool CanFire => m_RefireTimer <= 0;

        private void Start()
        {
            m_Tower = transform.root.GetComponent<Tower>();
        }

        protected override void FixedUpdate()
        {
            if (m_RefireTimer > 0)
                m_RefireTimer -= Time.deltaTime;
            else
                if (m_Mode == TurretMode.Auto) Fire();
        }

        public override void Fire()
        {
            if (m_TurretProperties == null) return;
            if (m_RefireTimer > 0) return;

            // —оздание снар€да при выстреле
            Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;

            // ѕолучение данных о том кто выпустил снар€д, дл€ избегани€ попаданий в самого себ€
            projectile.SetParentShooter(m_Tower);
            projectile.Damage = m_TurretProperties.ProjectileDamage;

            if (projectile.TryGetComponent(out ProjectileWithExplosion explosion) == true)
            {
                explosion.ExplosionDamage = m_TurretProperties.ProjectileExplosionDamage;
                explosion.ExplosionRadius = m_TurretProperties.ProjectileExplosionRadius;
            }

            m_RefireTimer = m_TurretProperties.RateOfFire;

            /// «вук выстрела.
            if (Mode == TurretMode.Primary && m_TurretProperties.ProjectilePrefab.GetComponent<ArrowProjectile>() == true)
                Sound.Arrow.Play();
            if (Mode == TurretMode.Primary && m_TurretProperties.ProjectilePrefab.GetComponent<FrostProjectile>() == true)
                Sound.Arrow.Play();
            if (Mode == TurretMode.Primary && m_TurretProperties.ProjectilePrefab.GetComponent<SiegeProjectile>() == true)
                Sound.Arrow.Play();
        }

        public override void AssingLoadout(TurretProperties properties)
        {
            if (m_Mode != properties.Mode) return;

            m_RefireTimer = 0;
            m_TurretProperties = properties;
        }
    }
}