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

        /// <summary>
        /// Обновление количества ресурсов:
        /// - создание списка для объединения всех обновлений;
        /// - добавление элементов из каждого массива в общик список;
        /// - получение общей стоимости обновлений;
        /// - обновление количества ресурсов;
        /// - обновление стоимости элементов.
        /// </summary>
        public void UpdateMoney()
        {
            m_Money = MapCompletion.Instance.TotalScore;

            int cost = Upgrades.GetTotalCost();

            m_Money -= cost;
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

        private void OnDestroy()
        {
            foreach (var slot in m_Sales)
            {
                slot.transform.Find("Buy_Button").GetComponent<Button>().onClick.RemoveListener(UpdateMoney);
            }
        }
    }
}