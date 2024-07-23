using System.Collections;
using UnityEngine;

namespace TowerDefence
{
    public class OpenShopButton : MonoBehaviour
    {
        [SerializeField] private UpgradeShop m_Shop;

        public void EX_OpenShop()
        {
            Sound.Click.PlaySound();
            StartCoroutine(DelayTime());
        }

        private IEnumerator DelayTime()
        {
            yield return new WaitForSeconds(0.3f);
            m_Shop.gameObject.SetActive(true);
        }
    }
}