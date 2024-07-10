using UnityEngine;
using Common;

namespace TowerDefence
{
    [CreateAssetMenu(fileName = "Enemy_n_Propeties", menuName = "TowerDefence/EnemyProperties")]
    public sealed class EnemyAsset : EnemyAssetBase
    {
        public enum UnitType {None, Ground, Air }
        public enum ArmorType { Base, Magic }

        public string Name = "Name";
        public UnitType _UnitType;
        public ArmorType _ArmorType;

        [Header("Visual Model")]
        public Color SpriteColor = Color.white;
        public Vector2 SpriteScale = Vector2.one;
        public RuntimeAnimatorController Animations;
        public float ColliderRadius = 0.24f;
        public float ColliderOffsetX = 0;
        public float ColliderOffsetY = 0;

        [Header("Properties")]
        public int TeamID = 2;
        public int HitPoints = 1;
        public int ArmorPoints = 0;
        public int ScoreValue = 1;

        public float MoveSpeed = 1.0f;

        public int damageToPlayer;
        public int goldValue;
    }
}