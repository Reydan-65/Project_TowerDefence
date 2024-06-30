using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    public class LevelDisplayController : MonoBehaviour
    {
        private MapLevel[] m_MapLevels;

        private void Start()
        {
            m_MapLevels = GetComponentsInChildren<MapLevel>();

            var drawLevel = 0;
            int levelScore = 1;

            //LevelProperties[] properties = LevelSequencesController.Instance.LevelSequences.LevelsProperties;

            /*
            for (int i = 0; i < properties.Length; i++)
            {
                print(properties[i].SceneName);
            }
                print(drawLevel);
                print(levelScore);
                print(m_MapLevels.Length);
                print(m_MapLevels[drawLevel]);
                print(MapCompletion.Instance);
            */
            // ���������...

            // ��������� ���� ��������� �������

            while (levelScore != 0 && drawLevel < m_MapLevels.Length &&
                   MapCompletion.Instance.TryIndex(drawLevel, out var episodeName, out levelScore))
            {
                m_MapLevels[drawLevel].SetLevelData(LevelSequencesController.Instance.LevelSequences.LevelsProperties[drawLevel].SceneName, levelScore);
                drawLevel++;
            }

            // ��������� ��� ����������� ������
            for (int i = drawLevel; i < m_MapLevels.Length; i++)
            {
                m_MapLevels[i].gameObject.SetActive(false);
            }
        }
    }
}