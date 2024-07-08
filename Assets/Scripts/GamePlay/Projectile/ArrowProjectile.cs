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
    }
}