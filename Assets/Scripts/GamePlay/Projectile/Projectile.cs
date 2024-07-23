using UnityEngine;
using Common;
using TowerDefence;

namespace SpaceShooter
{
    public class Projectile : ProjectileBase
    {
        public enum DamageType { Base, Magic }

        [SerializeField] protected DamageType m_DamageType;
        [SerializeField] protected GameObject m_ImpactEffect;
        [SerializeField] protected Sound m_LaunchSound;
        [SerializeField] protected Sound m_HitSound;

        protected override void Awake()
        {
            base.Awake();
            m_LaunchSound.PlaySound();
        }
        protected override void OnHit(Destructible destructible)
        {
            base.OnHit(destructible);
            m_HitSound.PlaySound();
        }

        protected override void OnHit(RaycastHit2D hit)
        {
            Destructible destructible = hit.collider.transform.root.GetComponent<Destructible>();

            if (destructible != null && destructible != m_Parent)
            {
                destructible.ApplyDamage(m_Damage);

                OnHit(destructible);
                OnProjectileLifeEnd(hit.collider, hit.point);
            }
        }

        protected override void OnProjectileLifeEnd(Collider2D collider, Vector2 position)
        {
            position = transform.position;

            if (m_ImpactEffect != null)
                Instantiate(m_ImpactEffect, position, Quaternion.identity);

            Destroy(gameObject);
        }


        // Попадание в препятствие
        protected override RaycastHit2D OnHitObstacles(RaycastHit2D hit)
        {
            Collider2D collider = hit.collider.GetComponent<Collider2D>();

            return base.OnHitObstacles(hit);
        }

        protected bool CanDealDamageToTarget(Enemy target, Turret turret)
        {
            if (target == null) return false;

            if (turret.TowerAsset.Type == TowerAsset.TargetType.All &&
                target.UType == Enemy.UnitType.Ground ||
                target.UType == Enemy.UnitType.Air) return true;

            if (turret.TowerAsset.Type == TowerAsset.TargetType.Ground &&
                target.UType == Enemy.UnitType.Ground) return true;

            if (turret.TowerAsset.Type == TowerAsset.TargetType.Air &&
                target.UType == Enemy.UnitType.Air) return true;

            return false;
        }
    }
}