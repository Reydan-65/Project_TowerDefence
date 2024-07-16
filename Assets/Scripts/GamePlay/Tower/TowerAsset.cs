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

        [Header("Properties")]
        public TargetType Type;
        public int GoldCost;
        public Sprite Sprite;
        public Sprite GUISprite;
        public TurretProperties TurretProperties;

        [Space(10)]
        [Header("UpgradesProperties")]
        [SerializeField] private string RequaredUpgradeName;
        [SerializeField] private int RequaredLevelUpgrade;

        public bool IsAvailable() =>
            RequaredUpgradeName != null && RequaredLevelUpgrade <= Upgrades.GetUpgradeLevel(RequaredUpgradeName);

        public TowerAsset[] m_UpgradesTo;
    }
}