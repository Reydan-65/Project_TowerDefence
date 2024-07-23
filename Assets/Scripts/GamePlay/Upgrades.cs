using System;
using System.Collections.Generic;

namespace TowerDefence
{
    public class Upgrades : SingletonBase<Upgrades>
    {
        public const string filename = "upgrades.dat";

        public UpgradeAsset Assets;

        [Serializable]
        private class UpgradeSave
        {
            public string UpgradeName;
            public int UpgradeLevel = 0;
        }

        private List<UpgradeSave> m_Saves = new List<UpgradeSave>();

        private void Awake()
        {
            while (m_Saves.Count < Instance.Assets.PlayerProperties.Length +
                                   Instance.Assets.TowerProperties.Length +
                                   Instance.Assets.AbilityProperties.Length)
            {
                m_Saves.Add(new UpgradeSave());
            }

            for (int i = 0; i < Instance.Assets.PlayerProperties.Length; i++)
            {
                m_Saves[i].UpgradeName = Instance.Assets.PlayerProperties[i].UpgradeName;
            }

            for (int i = 0; i < Instance.Assets.TowerProperties.Length; i++)
            {
                m_Saves[Instance.Assets.PlayerProperties.Length + i].UpgradeName = Instance.Assets.TowerProperties[i].UpgradeName;
            }

            for (int i = 0; i < Instance.Assets.AbilityProperties.Length; i++)
            {
                m_Saves[Instance.Assets.PlayerProperties.Length + Instance.Assets.TowerProperties.Length + i].UpgradeName = Instance.Assets.AbilityProperties[i].UpgradeName;
            }

            Saver<List<UpgradeSave>>.TryLoad(filename, ref m_Saves);
        }

        /// <summary>
        /// Покупаем улучшение:
        /// повышаем его уровень,
        /// сохранаяем.
        /// </summary>
        public static void BuyUpgrade(string propertiesName)
        {
            foreach (var upgrade in Instance.m_Saves)
            {
                if (upgrade.UpgradeName == propertiesName)
                {
                    upgrade.UpgradeLevel++;

                    Saver<List<UpgradeSave>>.Save(filename, Instance.m_Saves);
                }
            }
        }

        /// <summary>
        /// Получаем уровень улучшения из сохранённых данных,
        /// Если этих данных нет - уровень равен 0.
        /// </summary>
        public static int GetUpgradeLevel(string propertiesName)
        {
            foreach (var upgrade in Instance.m_Saves)
            {
                if (upgrade.UpgradeName == propertiesName)
                {
                    return upgrade.UpgradeLevel;
                }
            }
            return 0;
        }

        /// <summary>
        /// В зависимости от уровня улучшения,
        /// получаем его стоимость из массива по имени улучшения в массиве ассетов.
        /// </summary>
        public static int GetTotalCost()
        {
            int result = 0;

            foreach (var upgrade in Instance.m_Saves)
            {
                for (int j = 0; j < Instance.Assets.PlayerProperties.Length; j++)
                {
                    if (upgrade.UpgradeName == Instance.Assets.PlayerProperties[j].UpgradeName)
                    {
                        for (int i = 0; i < upgrade.UpgradeLevel; i++)
                        {
                            if (i < Instance.Assets.PlayerProperties[j].CostByLevel.Length)
                                result += Instance.Assets.PlayerProperties[j].CostByLevel[i];
                        }
                    }
                }

                for (int k = 0; k < Instance.Assets.TowerProperties.Length; k++)
                {
                    if (upgrade.UpgradeName == Instance.Assets.TowerProperties[k].UpgradeName)
                    {
                        for (int i = 0; i < upgrade.UpgradeLevel; i++)
                        {
                            if (i < Instance.Assets.TowerProperties[k].CostByLevel.Length)
                                result += Instance.Assets.TowerProperties[k].CostByLevel[i];
                        }
                    }
                }

                for (int l = 0; l < Instance.Assets.AbilityProperties.Length; l++)
                {
                    if (upgrade.UpgradeName == Instance.Assets.AbilityProperties[l].UpgradeName)
                    {
                        for (int i = 0; i < upgrade.UpgradeLevel; i++)
                        {
                            if (i < Instance.Assets.AbilityProperties[l].CostByLevel.Length)
                                result += Instance.Assets.AbilityProperties[l].CostByLevel[i];
                        }
                    }
                }
            }

            return result;
        }
    }
}