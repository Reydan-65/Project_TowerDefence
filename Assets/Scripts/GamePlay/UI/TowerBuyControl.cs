using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class TowerBuyControl : MonoBehaviour
    {
        [SerializeField] private TowerAsset m_TowerAsset;
        [SerializeField] private TextMeshProUGUI m_Text;
        [SerializeField] private Button m_Button;
        [SerializeField] private Transform m_BuildPoint;

        public void SetBuildPoint(Transform value)
        {
            m_BuildPoint = value;
        }

        private void Awake()
        {
            TD_Player.GoldUpdateSubscribe(GoldStatusCheck);
        }

        private void Start()
        {
            m_Text.text = m_TowerAsset.GoldCost.ToString();
            m_Button.GetComponent<Image>().sprite = m_TowerAsset.GUISprite;
        }

        private void GoldStatusCheck(int value)
        {
            if (value >= m_TowerAsset.GoldCost != m_Button.interactable)
            {
                m_Button.interactable = !m_Button.interactable;
                m_Text.color = m_Button.interactable ? Color.green : Color.red;
            }
        }

        public void Buy()
        {
            TD_Player.Instance.TryBuild(m_TowerAsset, m_BuildPoint);

            BuildPoint.HideControls();
        }

        private void OnDestroy()
        {
            TD_Player.GoldUpdateUnSubscribe(GoldStatusCheck);
        }
    }
}