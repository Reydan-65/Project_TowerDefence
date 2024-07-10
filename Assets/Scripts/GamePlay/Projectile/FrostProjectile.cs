using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    public class FrostProjectile : Projectile
    {
        [SerializeField] private float m_ExplosionRadius;
        [SerializeField] private float m_ExplosionDamage;
        [SerializeField] private DebuffEffect m_DebuffPrefab;
        [SerializeField] private UpgradeAsset m_FrostProjectileRadiusUpgradeAsset;

        private Explosion m_Explosion;

        private void Start()
        {
            m_Explosion = GetComponent<Explosion>();

            var level = Upgrades.GetUpgradeLevel(m_FrostProjectileRadiusUpgradeAsset);

            m_ExplosionRadius += (float)(level * 0.15);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void OnHit(Destructible destructible)
        {
            base.OnHit(destructible);

            m_Explosion.Explode(m_ExplosionRadius, m_ExplosionDamage, this, m_DebuffPrefab);
        }

        protected override void OnHit(RaycastHit2D hit)
        {
            var enemy = hit.collider.transform.root.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(m_Damage, m_DamageType);

                OnHit(enemy.GetComponent<Destructible>());
                OnProjectileLifeEnd(hit.collider, hit.point);
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, m_ExplosionRadius);
        }

#endif
    }
}