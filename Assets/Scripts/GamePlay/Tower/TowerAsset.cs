using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    [CreateAssetMenu]
    public class TowerAsset : ScriptableObject
    {
        public enum TargetType
        {
            All,
            Ground,
            Air
        }

        public TargetType Type;
        public int GoldCost = 15;
        public Sprite Sprite;
        public Sprite GUISprite;
        public Projectile ProjectilePrefab;
        public float RateOfFire;
    }
}