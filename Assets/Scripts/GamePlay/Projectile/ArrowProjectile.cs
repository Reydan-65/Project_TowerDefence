using SpaceShooter;
using UnityEngine;
using System;

namespace TowerDefence
{
    public class ArrowProjectile : Projectile
    {
        [SerializeField] private UpgradeAsset m_ArrowUpgradeAsset;

        protected override void Awake()
        {
            base.Awake();
        
            var level = Upgrades.GetUpgradeLevel(m_ArrowUpgradeAsset);

            m_Damage += level;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
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
    }
}