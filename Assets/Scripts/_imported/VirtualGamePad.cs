using UnityEngine;
using UnityEngine.UI;
using Common;

namespace SpaceShooter
{
    public class VirtualGamePad : MonoBehaviour
    {
        public PointerClickHold MobileFirePrimary;
        public PointerClickHold MobileFireSecondary;
        public VirtualJoystick VirtualJoystick;

        [SerializeField] private Sprite[] m_Sprites;

        public Sprite[] _Sprites { get => m_Sprites; set => m_Sprites = value; }

        private void Start()
        {
            MobileFireSecondary.GetComponent<Image>().enabled = false;
        }
    }
}