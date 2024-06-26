using UnityEngine.EventSystems;

namespace TowerDefence
{
    public class NullBuildPoint : BuildPoint
    {
        protected override void Start() { }

        public override void OnPointerDown(PointerEventData eventData)
        {
            HideControls();
        }
    }
}