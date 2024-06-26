using UnityEngine;
using TMPro;

namespace TowerDefence
{
    public class TextUpdate : MonoBehaviour
    {
        public enum UpdateSource { Gold, Lives }
        public UpdateSource source = UpdateSource.Gold;

        private TextMeshProUGUI m_Text;

        private void Awake()
        {
            m_Text = GetComponent<TextMeshProUGUI>();

            switch (source)
            { 
                case UpdateSource.Gold:
                    TD_Player.GoldUpdateSubscribe(UpdateText);
                    break;

                case UpdateSource.Lives:
                    TD_Player.LivesUpdateSubscribe(UpdateText);
                    break;
            }
        }

        private void UpdateText(int value)
        {
            m_Text.text = value.ToString();
        }

        private void OnDestroy()
        {
            TD_Player.GoldUpdateUnSubscribe(UpdateText);
            TD_Player.LivesUpdateUnSubscribe(UpdateText);
        }
    }
}