using UnityEngine;
using SpaceShooter;
using UnityEditor;
using System;

namespace TowerDefence
{
    [RequireComponent(typeof(TD_PatrolController))]
    public class Enemy : MonoBehaviour
    {
        public enum UnitType
        {
            None,
            Ground,
            Air
        }

        private UnitType m_Type;
        private int m_Damage;
        private int m_Gold;

        public UnitType Type => m_Type;

        public event Action OnEnemyDestroy;

        public void OnDestroy() { OnEnemyDestroy?.Invoke(); }

        private void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("Enemy");    
        }

        public void Use(EnemyAsset asset)
        {
            var sr = transform.Find("View").GetComponent<SpriteRenderer>();
            sr.color = asset.SpriteColor;
            sr.transform.localScale = new Vector3(asset.SpriteScale.x, asset.SpriteScale.y, 1f);
            sr.GetComponent<Animator>().runtimeAnimatorController = asset.Animations;

            GetComponent<SpaceShip>().Use(asset);
            GetComponentInChildren<CircleCollider2D>().radius = asset.ColliderRadius;
            GetComponentInChildren<CircleCollider2D>().offset = new Vector2(asset.ColliderOffsetX, asset.ColliderOffsetY);

            m_Type = (UnitType)asset.Type;
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