using UnityEngine;
using System.Collections.Generic;

namespace TowerDefence
{
    public class BuyControl : MonoBehaviour
    {
        [SerializeField] private TowerBuyControl m_TowerBuyControlPrefab;

        private List<TowerBuyControl> m_ActiveTowerBuyControl;
        private RectTransform m_RectTransform;
        private BuildPoint m_SelectedBuildPoint;

        #region Unity Events

        private void Start()
        {
            m_RectTransform = GetComponent<RectTransform>();

            BuildPoint.OnClickEvent += MoveToBuildPoint;
            m_ActiveTowerBuyControl = new List<TowerBuyControl>();

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            BuildPoint.OnClickEvent -= MoveToBuildPoint;
        }

        #endregion

        /// <summary>
        /// Перемещаем объект к точке строительства.
        /// </summary>
        private void MoveToBuildPoint(BuildPoint buildPoint)
        {
            if (buildPoint)
            {
                if (m_SelectedBuildPoint != null && m_SelectedBuildPoint != buildPoint)
                    ClearBuyControl();

                m_SelectedBuildPoint = buildPoint;

                if (m_RectTransform != null)
                {
                    var position = Camera.main.WorldToScreenPoint(buildPoint.transform.root.position);
                    m_RectTransform.anchoredPosition = new Vector2(position.x, position.y);

                    m_ActiveTowerBuyControl.Clear();

                    foreach (var asset in buildPoint.BuildableTowers)
                    {
                        if (asset.IsAvailable())
                        {
                            var newControl = Instantiate(m_TowerBuyControlPrefab, transform);

                            m_ActiveTowerBuyControl.Add(newControl);
                            newControl.SetTowerAsset(asset);
                        }
                    }
                }

                if (m_ActiveTowerBuyControl.Count > 0)
                {
                    gameObject.SetActive(true);

                    var angle = 360 / m_ActiveTowerBuyControl.Count;

                    for (int i = 0; i < m_ActiveTowerBuyControl.Count; i++)
                    {
                        var offset = Quaternion.AngleAxis(angle * i, Vector3.forward) * (Vector3.left * 80);
                        m_ActiveTowerBuyControl[i].transform.position += offset;
                    }

                    foreach (var towerBuyControl in GetComponentsInChildren<TowerBuyControl>())
                    {
                        towerBuyControl.SetBuildPoint(buildPoint.transform.root);
                    }
                }
            }
            else
            {
                ClearBuyControl();
                gameObject.SetActive(false);
            }
        }

        private void ClearBuyControl()
        {
            if (m_ActiveTowerBuyControl != null)
            {
                foreach (var control in m_ActiveTowerBuyControl)
                    Destroy(control.gameObject);

                m_ActiveTowerBuyControl.Clear();
            }
        }
    }
}