using UnityEngine;
using Common;
using TowerDefence;

namespace SpaceShooter
{
    public class Projectile : ProjectileBase
    {
        [SerializeField] protected GameObject m_ImpactEffect;

        protected override void OnHit(Destructible destructible)
        {
            base.OnHit(destructible);

            OnTargetDestroyed(destructible);
        }

        /// <summary>
        /// Начисление очков за уничтожение объектов.
        /// Начисление количества убитых противников.
        /// Исключить корабль игрока.
        /// </summary>
        /// <param name="destructible"></param>
        public void OnTargetDestroyed(Destructible destructible)
        {
            if (destructible == null) return;

            if (destructible.HitPoints <= 0)
            {
                //if (m_Parent == Player.Instance.ActiveShip)
                //{
                //    if (destructible != Player.Instance.ActiveShip)
                //    {
                //        Player.Instance.AddScore(destructible.ScoreValue);

                //        if (destructible is SpaceShip)
                //        {
                //            if (destructible.HitPoints <= 0)
                //                Player.Instance.AddKill();
                //        }
                //    }
                //}

                //TD_Player.Instance.ReduceEnemiesLast();
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

            //if (collider.GetComponent<Wall>() == true || collider.transform.root.GetComponent<GravityWell>() == true)
            //    OnProjectileLifeEnd(hit.collider, hit.point);

            return base.OnHitObstacles(hit);
        }

        protected bool CanDealDamageToTarget(Enemy target, Turret turret)
        {
            if (target == null) return false;

            if (turret.TowerAsset.Type == TowerAsset.TargetType.All &&
                target.Type == Enemy.UnitType.Ground ||
                target.Type == Enemy.UnitType.Air) return true;

            if (turret.TowerAsset.Type == TowerAsset.TargetType.Ground &&
                target.Type == Enemy.UnitType.Ground) return true;

            if (turret.TowerAsset.Type == TowerAsset.TargetType.Air &&
                target.Type == Enemy.UnitType.Air) return true;

            return false;
        }
    }
}