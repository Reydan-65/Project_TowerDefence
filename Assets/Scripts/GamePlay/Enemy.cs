using UnityEngine;
using SpaceShooter;
using UnityEditor;
using System;

namespace TowerDefence
{
    [RequireComponent(typeof(TD_PatrolController))]
    [RequireComponent(typeof(Destructible))]
    public class Enemy : MonoBehaviour
    {
        public enum UnitType
        {
            None,
            Ground,
            Air
        }

        public enum ArmorType { Base = 0, Magic = 1 }

        // Func последний параметр (например, int) возвращаемое значение.
        private static Func<int, Projectile.DamageType, int, int>[] ArmorDamageFunctions =
        {
            // Функция для Базового типа брони
            (int damage, Projectile.DamageType damageType, int armor) =>
            {
                int damageValue = damage - armor;
                if (damageValue < 0) damageValue = 0;

                // Если тип снаряда Магический, а НЕ Базовый : нанести полный урон.
                // Если НЕ Магический : нанести (урон - броня) урона.
                switch (damageType)
                {
                    case Projectile.DamageType.Magic: return damage;
                    default: return Mathf.Max(1, damageValue);
                }
            },

            // Функция для Магического типа брони
            (int damage, Projectile.DamageType damageType, int armor) =>
            {
                // Если тип снаряда Базовый, а НЕ Магический : броня снижается вдвое.
                // В противном случае : нанести (урон - броня) урона.
                if (Projectile.DamageType.Base == damageType) armor = armor / 2;

                int damageValue = damage - armor;
                if (damageValue < 0) damageValue = 0;

                return Mathf.Max(1, damageValue);
            }
        };

        private UnitType m_UnitType;
        private int m_Damage;
        private int m_Gold;
        private int m_Armor;
        private ArmorType m_ArmorType;

        private Destructible m_Destructible;

        public UnitType UType => m_UnitType;
        public ArmorType AType => m_ArmorType;

        public event Action OnEnemyDestroy;

        public void OnDestroy()
        {
            OnEnemyDestroy?.Invoke();
        }

        private void Awake()
        {
            m_Destructible = GetComponent<Destructible>();
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }

        public void Use(EnemyAsset asset)
        {
            var sr = transform.Find("View").GetComponent<SpriteRenderer>();
            sr.color = asset.SpriteColor;

            sr.GetComponent<Animator>().runtimeAnimatorController = asset.Animations;
            sr.transform.localScale = new Vector3(asset.SpriteScale.x, asset.SpriteScale.y, 1f);

            GetComponent<SpaceShip>().Use(asset);
            GetComponentInChildren<CircleCollider2D>().radius = asset.ColliderRadius;
            GetComponentInChildren<CircleCollider2D>().offset = new Vector2(asset.ColliderOffsetX, asset.ColliderOffsetY);

            m_UnitType = (UnitType)asset._UnitType;
            m_ArmorType = (ArmorType)asset._ArmorType;
            m_Armor = asset.ArmorPoints;
            m_Damage = asset.damageToPlayer;
            m_Gold = asset.goldValue;
        }

        public void EX_ApplyDamageToPlayer()
        {
            TD_Player.Instance.ReduceLives(m_Damage);
        }

        public void EX_AddGold()
        {
            TD_Player.Instance.ChangeGold(m_Gold);
        }

        public void TakeDamage(int damage, Projectile.DamageType damageType)
        {
            //Получает урон равный второму значению, если оно меньше 1, то урон равен 1.
            m_Destructible.ApplyDamage(ArmorDamageFunctions[(int)m_ArmorType](damage, damageType, m_Armor));
        }

#if UNITY_EDITOR

        [CustomEditor(typeof(Enemy))]
        public class EnemyInspector : Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                EnemyAsset asset = EditorGUILayout.ObjectField(null, typeof(EnemyAsset), false) as EnemyAsset;

                if (asset != null)
                {
                    (target as Enemy).Use(asset);
                }
            }
        }

#endif

    }
}