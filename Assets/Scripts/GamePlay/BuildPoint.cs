using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace TowerDefence
{
    public class BuildPoint : MonoBehaviour, IPointerDownHandler
    {
        public static event Action<Transform> OnClickEvent;

        protected virtual void Start()
        {
            transform.SetParent(null);
        }

        public static void HideControls()
        {
            OnClickEvent(null);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnClickEvent(transform.root);
        }
    }
}