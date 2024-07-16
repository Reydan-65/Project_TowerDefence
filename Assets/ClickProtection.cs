using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerDefence
{
    /// <summary>
    /// При Активации:
    /// - блокируются прочие интерактивные объекты на сцене;
    /// - при первом клике в области блокировки запускается событие
    ///   и блокировка снимается.
    /// </summary>
    public class ClickProtection : SingletonBase<ClickProtection>, IPointerClickHandler
    {
        private Image m_ProtectionImage;

        private void Start()
        {
            m_ProtectionImage = GetComponent<Image>();
        }

        private Action<Vector2> m_OnClickAction;

        public void Activate(Action<Vector2> mouseAction)
        {
            m_ProtectionImage.enabled = true;
            m_OnClickAction = mouseAction;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            m_ProtectionImage.enabled = false;

            m_OnClickAction(eventData.pressPosition);
            m_OnClickAction = null;
        }
    }
}