using UnityEngine;

namespace TowerDefence
{
    public class OpenShopButton : MonoBehaviour
    {
        [SerializeField] private UpgradeShop m_Shop;

        public void EX_OpenShop()
        {
            m_Shop.gameObject.SetActive(true);
        }
    }
}