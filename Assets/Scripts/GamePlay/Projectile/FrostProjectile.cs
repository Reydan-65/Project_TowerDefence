using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    public class FrostProjectile : ProjectileWithExplosion
    {
        private void Start()
        {
            m_Explosion = GetComponent<Explosion>();
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