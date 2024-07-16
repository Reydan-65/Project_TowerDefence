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
        [SerializeField] private float m_DebuffTime;

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

                    if (m_Type == ExplosionType.Frost) type = Projectile.DamageType.Magic;
                    else type = Projectile.DamageType.Base;

                    //enemy.GetComponent<Destructible>().ApplyDamage(damage);
                    enemy.TakeDamage(damage, type);

                    //if (projectile != null)
                    //    projectile.OnTargetDestroyed(enemy.GetComponent<Destructible>());
                }

                /*
                if (m_Type == ExplosionType.Missle || m_Type == ExplosionType.Mine)
                {
                    if (hit.transform.root.TryGetComponent(out Rigidbody2D rb) == true)
                    {
                        Vector2 direction = (hit.transform.position - transform.position).normalized;

                        if (hit.transform.root.GetComponent<SpaceShip>() == true)
                            rb.AddForce(direction * CalculateDamage(dist, explosionRadius, explosionDamage) * 2f, ForceMode2D.Force);

                        rb.AddForce(direction * CalculateDamage(dist, explosionRadius, explosionDamage) * 4f, ForceMode2D.Force);
                    }
                }
                */
                if (m_Type == ExplosionType.Frost)
                {
                    if (disablerPrefab != null)
                    {
                        if (enemy.TryGetComponent(out SpaceShip ship) == true)
                        {
                            if (enemy.TryGetComponent(out DebuffEffect debuff) == true)
                            {
                                ship.RestoreMaxLinearVelocityOnValue();
                                //ship.Thrust *= 2;

                                Destroy(debuff);
                            }

                            DebuffEffect de = enemy.gameObject.AddComponent<DebuffEffect>();

                            de.Type = DebuffEffect.DebuffType.Frost;
                            de.DebuffTime = m_DebuffTime;
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