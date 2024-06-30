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

            // Отрисовка всех доступных уровней
            while (levelScore != 0 && drawLevel < m_MapLevels.Length &&
                   MapCompletion.Instance.TryIndex(drawLevel, out var episodeName, out levelScore))
            {
                m_MapLevels[drawLevel].SetLevelData(LevelSequencesController.Instance.LevelSequences.LevelsProperties[drawLevel].SceneName, levelScore);
                drawLevel++;
            }

            // Отключить все недоступные уровни
            for (int i = drawLevel; i < m_MapLevels.Length; i++)
            {
                m_MapLevels[i].gameObject.SetActive(false);
            }
        }
    }
}