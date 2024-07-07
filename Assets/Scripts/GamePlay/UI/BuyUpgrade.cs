using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class BuyUpgrade : MonoBehaviour
    {
        [SerializeField] private UpgradeAsset m_UpgradeAsset;
        [SerializeField] private Image m_UpgradeIcon;
        [SerializeField] private TextMeshProUGUI m_LevelNumText, m_CostText;
        [SerializeField] private Button m_BuyButton;

        public void Initialize()
        {
            m_UpgradeIcon.sprite = m_UpgradeAsset.Sprite;

            var savedLevel = Upgrades.GetUpgradeLevel(m_UpgradeAsset);

            this.m_LevelNumText.text = $"Level: {savedLevel + 1}";
            m_CostText.text = m_UpgradeAsset.CostByLevel[savedLevel].ToString();
        }

        public void EX_Buy()
        {
            Upgrades.BuyUpgrade(m_UpgradeAsset);
        }
    }
}