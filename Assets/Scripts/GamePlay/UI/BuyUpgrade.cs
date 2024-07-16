using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TowerDefence.UpgradeAsset;

namespace TowerDefence
{
    public class BuyUpgrade : MonoBehaviour
    {
        private const string PlayerUpgrade = "PlayerUpgrades";
        private const string TowersUpgrade = "TowersUpgrades";
        private const string AbilitiesUpgrade = "AbilitiesUpgrades";

        [SerializeField] private Image m_UpgradeIcon;
        [SerializeField] private TextMeshProUGUI m_LevelNumText, m_CostText;
        [SerializeField] private Button m_BuyButton;

        private int m_Cost;
        private int m_Index;

        private PropertiesUpgrade m_PropertiesUpgrade;

        private void Awake()
        {
            // Определяем индекс и тип улучшения
            m_Index = transform.GetSiblingIndex();

            if (transform.parent.name == PlayerUpgrade)
                m_PropertiesUpgrade = Upgrades.Instance.Assets.PlayerProperties[m_Index];
            else if (transform.parent.name == TowersUpgrade)
                m_PropertiesUpgrade = Upgrades.Instance.Assets.TowerProperties[m_Index];
            else if (transform.parent.name == AbilitiesUpgrade)
                m_PropertiesUpgrade = Upgrades.Instance.Assets.AbilityProperties[m_Index];
        }

        public void Initialize()
        {
            // Инициализация спрайта и стоимости
            if (m_PropertiesUpgrade != null)
            {
                m_UpgradeIcon.sprite = m_PropertiesUpgrade.Sprite;
                InitCost(m_PropertiesUpgrade.CostByLevel, m_PropertiesUpgrade.UpgradeName);
            }
        }

        private void InitCost(int[] costByLevel, string propertiesName)
        {
            var savedLevel = Upgrades.GetUpgradeLevel(propertiesName);

            if (savedLevel >= costByLevel.Length)
            {
                m_BuyButton.interactable = false;
                m_BuyButton.transform.Find("ScoreIcon_Image").gameObject.SetActive(false);
                m_CostText.color = Color.green;
                m_CostText.text = "Max";
                m_CostText.color = Color.yellow;
                m_CostText.alignment = TextAlignmentOptions.Center;
                m_Cost = -1;
            }
            else
            {
                m_LevelNumText.text = $"Level: {savedLevel + 1}";
                m_Cost = costByLevel[savedLevel];
                m_CostText.text = m_Cost.ToString();
            }
        }

        public void EX_Buy()
        {
            TDButton.PlayClickSound();

            // Выполнение покупки
            if (m_PropertiesUpgrade != null)
            {
                Upgrades.BuyUpgrade(m_PropertiesUpgrade.UpgradeName);
                Initialize(); // Обновление после покупки
            }

            // Обновление ColorBlock у кнопки
            m_BuyButton.interactable = false;
            m_BuyButton.interactable = true;
        }

        public void CheckCost(int m_Money)
        {
            // Проверка стоимости улучшения
            if (m_Money >= m_Cost && m_Cost >= 0)
                m_BuyButton.interactable = true;
            else
                m_BuyButton.interactable = false;
        }
    }
}
