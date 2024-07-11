using TMPro;
using UnityEngine;

namespace TowerDefence
{
    [RequireComponent(typeof(MapLevel))]
    public class MapBranchLevel : MonoBehaviour
    {
        [SerializeField] private MapLevel m_RootLevel;
        [SerializeField] private int m_NeedScors;
        [SerializeField] private TextMeshProUGUI m_NeedScoresText;

        //public bool RootIsActive
        //{
        //    get
        //    {
        //        return m_RootLevel.IsComplete;
        //    }
        //}

        ///<summary>
        ///���� ����� ����� ��������� ����� ������ ������������ ��� ��������� ������,
        ///��������� ������ ������� ������ � ����� ������� ��������� ����� ��� ���������.
        ///�����, �������� ����������� ������.
        ///</summary>
        public void TryActivate()
        {
            gameObject.SetActive(m_RootLevel.IsComplete);

            if (m_NeedScors > MapCompletion.Instance.TotalScore)
            {
                m_NeedScoresText.text = m_NeedScors.ToString();
            }
            else
            {
                m_NeedScoresText.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}