using UnityEngine;

namespace TowerDefence
{
    public class BuyControl : MonoBehaviour
    {
        private RectTransform m_RectTransform;

        private void Start()
        {
            m_RectTransform = GetComponent<RectTransform>();

            BuildPoint.OnClickEvent += MoveToBuildPoint;

            gameObject.SetActive(false);
        }

        private void MoveToBuildPoint(Transform buildPoint)
        {
            if (buildPoint)
            {
                if (m_RectTransform != null)
                {
                    var position = Camera.main.WorldToScreenPoint(buildPoint.position);
                    m_RectTransform.anchoredPosition = new Vector2(position.x, position.y);

                    gameObject.SetActive(true);
                }
            }
            else
                gameObject.SetActive(false);

            foreach (var towerBuyControl in GetComponentsInChildren<TowerBuyControl>())
            {
                towerBuyControl.SetBuildPoint(buildPoint);
            }
        }

        private void OnDestroy()
        {
            BuildPoint.OnClickEvent -= MoveToBuildPoint;
        }
    }
}