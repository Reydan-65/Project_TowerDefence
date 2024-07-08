using System;
using UnityEngine;

namespace TowerDefence
{
    public class Upgrades : SingletonBase<Upgrades>
    {
        public const string filename = "upgrades.dat";

        [Serializable]
        private class UpgradeSave
        {
            public UpgradeAsset UpgradeAsset;
            public int UpgradeLevel = 0;
        }

        [SerializeField] private UpgradeSave[] m_Saves;

        private void Awake()
        {
            Saver<UpgradeSave[]>.TryLoad(filename, ref m_Saves);    
        }

        public static void BuyUpgrade(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.m_Saves)
            {
                if (upgrade.UpgradeAsset == asset)
                {
                    upgrade.UpgradeLevel++;
                    Saver<UpgradeSave[]>.Save(filename, Instance.m_Saves);
                }
            }
        }

        public static int GetTotalCost()
        {
            int result = 0;

            foreach (var upgrade in Instance.m_Saves)
            {
                for (int i = 0; i < upgrade.UpgradeLevel; i++)
                {
                    result += upgrade.UpgradeAsset.CostByLevel[i];
                }
            }

            return result;
        }

        public static int GetUpgradeLevel(UpgradeAsset asset)
        {
            foreach (var upgrade in Instance.m_Saves)
            {
                if (upgrade.UpgradeAsset == asset)
                {
                    return upgrade.UpgradeLevel;
                }
            }

            return 0;
        }
    }
}