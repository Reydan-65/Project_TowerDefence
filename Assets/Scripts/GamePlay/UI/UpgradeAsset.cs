using UnityEngine;

namespace TowerDefence
{
    [CreateAssetMenu]
    public class UpgradeAsset : ScriptableObject
    {
        public Sprite Sprite;

        public int[] CostByLevel = { 3 };
    }
}