using UnityEngine;
using TMPro;
using SpaceShooter;

namespace TowerDefence
{
    public class TextUpdate : MonoBehaviour
    {
        public enum UpdateSource { Lives, Gold, Energy }
        public UpdateSource source = UpdateSource.Gold;

        private TextMeshProUGUI m_Text;

        private void Awake()
        {
            m_Text = GetComponent<TextMeshProUGUI>();

            switch (source)
            {
                case UpdateSource.Gold:
                    TD_Player.Instance.GoldUpdateSubscribe(UpdateText);
                    break;

                case UpdateSource.Lives:
                    TD_Player.Instance.LivesUpdateSubscribe(UpdateText);
                    break;

                case UpdateSource.Energy:
                    TD_Player.Instance.EnergyUpdateSubscribe(UpdateText);
                    break;
            }
        }

        private void UpdateText(int value)
        {
            m_Text.text = value.ToString();
        }
    }
}