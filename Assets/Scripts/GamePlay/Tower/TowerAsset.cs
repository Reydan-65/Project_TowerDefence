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
        public int GoldCost = 15;
        public Sprite Sprite;
        public Sprite GUISprite;
        public TurretProperties TurretProperties;

        [Space(10)]
        [Header("UpgradesProperties")]
        [SerializeField] private UpgradeAsset RequaredUpgrade;
        [SerializeField] private int RequaredLevelUpgrade;

        public bool IsAvailable() => 
            !RequaredUpgrade || RequaredLevelUpgrade <= Upgrades.GetUpgradeLevel(RequaredUpgrade);

        public TowerAsset[] m_UpgradesTo;
    }
}