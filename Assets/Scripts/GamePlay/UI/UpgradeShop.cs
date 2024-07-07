using System;
using TMPro;
using UnityEngine;

namespace TowerDefence
{
    public class UpgradeShop : MonoBehaviour
    {
        [SerializeField] private int m_Money;
        [SerializeField] private TextMeshProUGUI m_MoneyText;
        [SerializeField] private BuyUpgrade[] m_Sales;

        private void Start()
        {
            m_Money = MapCompletion.Instance.TotalScore;
            m_MoneyText.text = m_Money.ToString();

            foreach (var slot in m_Sales)
            {
                slot.Initialize();
            }
        }
    }
}