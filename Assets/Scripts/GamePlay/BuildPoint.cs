using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace TowerDefence
{
    public class BuildPoint : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private TowerAsset[] m_BuildableTowers;

        public static event Action<BuildPoint> OnClickEvent;

        public TowerAsset[] BuildableTowers { get => m_BuildableTowers; set => m_BuildableTowers = value; }

        protected virtual void Start()
        {
            if (transform.root.TryGetComponent(out Tower tower) == false)
                transform.SetParent(null);
        }

        public void SetBuildableTowers(TowerAsset[] towers)
        {
            if (towers == null || towers.Length == 0) Destroy(this);
            else m_BuildableTowers = towers;
        }

        public static void HideControls()
        {
            OnClickEvent(null);
        }

        /// <summary>
        /// ѕереопредел€емый метод.
        /// ≈сли кликаем по точке строительства, открываем BuyControl в этой точке.
        /// ≈сли кликаем вне точки строительства и вне BuyControl, закрываем BuyControl.
        /// </summary>
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnClickEvent(this);
        }
    }
}