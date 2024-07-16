using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDefence
{
    public class NullBuildPoint : BuildPoint
    {
        protected override void Start() { }

        private void Awake()
        {
            transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            HideControls();
        }
    }
}