using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class UpgradeShop : MonoBehaviour
    {
        [SerializeField] private int m_Money;
        [SerializeField] private BuyUpgrade[] m_Sales;
        [SerializeField] private TextMeshProUGUI m_MoneyText;

        public int Money => m_Money;

        private void Start()
        {
            foreach (var slot in m_Sales)
            {
                slot.Initialize();
                slot.transform.Find("Buy_Button").GetComponent<Button>().onClick.AddListener(UpdateMoney);
            }

            UpdateMoney();

            gameObject.SetActive(false);
        }

        public void UpdateMoney()
        {
            m_Money = MapCompletion.Instance.TotalScore;
            m_Money -= Upgrades.GetTotalCost();
            m_MoneyText.text = m_Money.ToString();

            foreach (var slot in m_Sales)
            {
                slot.CheckCost(m_Money);
            }
        }

        public void EX_CloseShop()
        {
            gameObject.SetActive(false);
        }
    }
}