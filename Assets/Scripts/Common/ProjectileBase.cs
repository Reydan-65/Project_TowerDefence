using UnityEngine;
using SpaceShooter;
using TowerDefence;
using System;

namespace Common
{
    public abstract class ProjectileBase : Entity
    {
        [SerializeField] protected float m_Velocity;
        [SerializeField] protected float m_Lifetime;
        [SerializeField] protected int m_Damage;

        protected virtual void OnHit(Destructible destructible) { }
        protected virtual void OnHit(Collider2D collider2D) { }
        protected virtual void OnHit(RaycastHit2D hit) { }
        protected virtual void OnProjectileLifeEnd(Collider2D collider, Vector2 position) { }

        public float Velocity => m_Velocity;
        protected float m_Timer;

        protected Entity m_Parent;

        protected virtual void Awake() { }

        protected virtual void FixedUpdate()
        {
            float stepLenght = Time.deltaTime * m_Velocity;

            Vector2 step = transform.up * stepLenght;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLenght);

            if (hit)
            {
                OnHit(hit.collider);
                OnHit(hit);
            }

            //hit = OnHitObstacles(hit);

            // Время жизни снаряда.
            m_Timer += Time.deltaTime;

            if (m_Timer > m_Lifetime)
                OnProjectileLifeEnd(hit.collider, hit.point);

            transform.position += new Vector3(step.x, step.y, 0);
        }
        /*
        
        */
        
        
        

        // Попадание в препятствие
        protected virtual RaycastHit2D OnHitObstacles(RaycastHit2D hit)
        {
            return hit;
        }

        // Указание на того, кто выпустил снаряд.
        public void SetParentShooter(Entity parent)
        {
            m_Parent = parent;
        }

        public void SetTarget(Destructible target)
        {

        }
    }
}