using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    public class LevelDisplayController : MonoBehaviour
    {
        private MapLevel[] m_MapLevels;
        private MapBranchLevel[] m_MapBranchLevels;

        private void Start()
        {
            m_MapLevels = transform.GetChild(0).GetComponentsInChildren<MapLevel>();
            m_MapBranchLevels = transform.GetChild(1).GetComponentsInChildren<MapBranchLevel>();

            var drawLevel = 0;
            int levelScore = 1;

            string sceneName = null;

            // Отрисовка всех доступных основных уровней.
            while (levelScore != 0 && drawLevel < m_MapLevels.Length &&
                   MapCompletion.Instance.TryIndex(drawLevel, out var episodeName, out levelScore))
            {
                sceneName = LevelSequencesController.Instance.LevelSequences.LevelsProperties[drawLevel].SceneName;

                m_MapLevels[drawLevel].SetLevelData(sceneName, levelScore);

                drawLevel++;
            }

            // Отключить все недоступные уровни.
            for (int i = drawLevel; i < m_MapLevels.Length; i++)
            {
                m_MapLevels[i].gameObject.SetActive(false);
            }

            //Ответвление.
            for (int i = 0; i < m_MapBranchLevels.Length; i++)
            {
                //Пробуем включить панель.
                m_MapBranchLevels[i].TryActivate();

                if (m_MapBranchLevels[i].gameObject.activeSelf)
                {
                    //Узнаём количество очков полученных за прохождение уровня.
                    MapCompletion.Instance.TryIndex(m_MapLevels.Length + i, out var episodeName, out levelScore);

                    //Если очков больше 0, включаем панель результата уровня и визуализируем очки.
                    m_MapBranchLevels[i].GetComponent<MapLevel>().ResultPanel.gameObject.SetActive(levelScore > 0);

                    for (int j = 0; j < levelScore; j++)
                    {
                        m_MapBranchLevels[i].GetComponent<MapLevel>().ResultImage[j].color = Color.white;
                    }
                }
            }
        }
    }
}