using System;
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

        private int m_Cost;

        /// <summary>
        /// Инициализация панели апгрейда в магазине:
        /// назначение спрайта апгрейда.
        /// уровня апгрейда.
        /// После выполнения покупки:
        /// Если куплен последний уровень апгрейда, блокируем.
        /// Иначе обновляем параметры.
        /// </summary>
        public void Initialize()
        {
            m_UpgradeIcon.sprite = m_UpgradeAsset.Sprite;

            var savedLevel = Upgrades.GetUpgradeLevel(m_UpgradeAsset);

            if (savedLevel >= m_UpgradeAsset.CostByLevel.Length)
            {
                m_BuyButton.interactable = false;
                m_BuyButton.transform.Find("ScoreIcon_Image").gameObject.SetActive(false);
                m_CostText.text = "Max";
                m_CostText.color = Color.yellow;
                m_CostText.alignment = TextAlignmentOptions.Center;
                m_Cost = -1;
            }
            else
            {
                this.m_LevelNumText.text = $"Level: {savedLevel + 1}";
                m_Cost = m_UpgradeAsset.CostByLevel[savedLevel];
                m_CostText.text = m_Cost.ToString();
            }
        }

        public void EX_Buy()
        {
            Upgrades.BuyUpgrade(m_UpgradeAsset);
            Initialize();
        }

        /// <summary>
        /// Проверяем стоимость улучшения,
        /// если денег хватает - кнопка активна;
        /// иначе - кнопка неактивна.
        /// </summary>
        public void CheckCost(int m_Money)
        {
            if (m_Money >= m_Cost && m_Cost >= 0)
            {
                m_CostText.color = Color.green;
                m_BuyButton.interactable = true;
            }
            else
            {
                m_CostText.color = Color.red;
                m_BuyButton.interactable = false;
            }
        }
    }
}