using TMPro;
using UnityEngine;

namespace TowerDefence
{
    public class ScoresIndicator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Text;

        private void Start()
        {
            m_Text.text = MapCompletion.Instance.TotalScore.ToString();

            if (MapCompletion.Instance.TotalScore > 0)
                transform.GetChild(0).gameObject.SetActive(true);
            else
                transform.GetChild(0).gameObject.SetActive(false);

        }
    }
}