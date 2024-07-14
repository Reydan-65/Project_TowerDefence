using System.Collections.Generic;
using UnityEngine;

namespace TowerDefence
{
    [CreateAssetMenu]
    public class UpgradeAsset : ScriptableObject
    {
        [System.Serializable]
        public class PropertiesUpgrade
        {
            [SerializeField] protected string m_UpgradeName;
            [SerializeField] protected Sprite m_Sprite;
            [SerializeField] protected int[] m_CostByLevel = { 5 };

            public string UpgradeName => m_UpgradeName;
            public Sprite Sprite => m_Sprite;
            public int[] CostByLevel => m_CostByLevel;
        }

        [System.Serializable]
        public class PlayerUpgradeProperties : PropertiesUpgrade { }  

        [System.Serializable]
        public class TowerUpgradeProperties : PropertiesUpgrade { } 

        [System.Serializable]
        public class AbilityUpgradeProperties : PropertiesUpgrade { } 

        public PlayerUpgradeProperties[] PlayerProperties;
        public TowerUpgradeProperties[] TowerProperties;
        public AbilityUpgradeProperties[] AbilityProperties;
    }
}