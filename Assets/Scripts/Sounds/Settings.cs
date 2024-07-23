using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private Slider m_Slider;
        public Slider Slider { get => m_Slider; set => m_Slider = value; }
    }
}