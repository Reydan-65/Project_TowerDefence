using UnityEngine;
using TowerDefence;
namespace SpaceShooter
{
    public class Explosion : MonoBehaviour
    {
        public enum ExplosionType
        {
            Missle,
            Frost,
            Siege,
            Mine
        }

        [SerializeField] private ExplosionType m_Type;
        [SerializeField] private float m_DebuffDuration;
        [SerializeField] private float m_DebuffSlowPower;

        private void Awake()
        {
            float durationStep = 0.2f;
            float SlowPowerStep = 0.2f;
            var upgradeLevel = Upgrades.GetUpgradeLevel(Upgrades.Instance.Assets.TowerProperties[1].UpgradeName);

            if (durationStep > 0)
            {
                m_DebuffDuration += durationStep * (upgradeLevel - 1);
                m_DebuffSlowPower += SlowPowerStep * (upgradeLevel - 1);
            }
        }

        public void Explode(float explosionRadius, float explosionDamage, Projectile projectile = null, DebuffEffect disablerPrefab = null)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

            if (hitColliders == null) return;

            foreach (Collider2D hit in hitColliders)
            {
                Enemy enemy = hit.transform.root.GetComponent<Enemy>();

                var dist = Vector2.Distance(transform.position, hit.transform.position);
                int damage = CalculateDamage(dist, explosionRadius, explosionDamage);

                if (enemy.GetComponent<Destructible>() == true)
                {
                    Projectile.DamageType type;

                    if (m_Type == ExplosionType.Frost)
                        type = Projectile.DamageType.Magic;
                    else
                        type = Projectile.DamageType.Base;

                    enemy.TakeDamage(damage, type);
                }

                if (m_Type == ExplosionType.Frost)
                {
                    if (disablerPrefab != null)
                    {
                        if (enemy.TryGetComponent(out SpaceShip ship) == true)
                        {
                            if (enemy.TryGetComponent(out DebuffEffect debuff) == true)
                            {
                                ship.RestoreMaxLinearVelocityOnValue();
                                Destroy(debuff);
                            }

                            DebuffEffect de = enemy.gameObject.AddComponent<DebuffEffect>();

                            de.Type = DebuffEffect.DebuffType.Frost;
                            de.DebuffTime = m_DebuffDuration;
                            de.DebuffSlowPower = m_DebuffSlowPower;
                        }
                    }
                }
            }

            Destroy(gameObject);
        }

        private int CalculateDamage(float distance, float explosionRadius, float explosionDamage)
        {
            float _damage = explosionDamage * (1 - Mathf.Clamp01(distance / explosionRadius));

            if (_damage < 1) _damage = 0;

            return (int)_damage;
        }
    }
}