using System;
using SpaceShooter;
using System.Collections.Generic;

namespace TowerDefence
{
    public class MapCompletion : SingletonBase<MapCompletion>
    {
        public const string filename = "completion.dat";

        [Serializable]
        private class LevelScoreData
        {
            public string EpisodeName;
            public int Score;
        }

        private List<LevelScoreData> m_CompletionData = new List<LevelScoreData>();
        private LevelProperties[] m_LevelProperties;
        private BranchLevelProperties[] m_BranchLevelProperties;
        private int m_TotalScore;
        
        public int TotalScore => m_TotalScore;

        private void Awake()
        {
            Saver<List<LevelScoreData>>.TryLoad(filename, ref m_CompletionData);

            m_LevelProperties = LevelSequencesController.Instance.LevelSequences.LevelsProperties;
            m_BranchLevelProperties = LevelSequencesController.Instance.LevelSequences.BranchLevelsProperties;

            // Если m_CompletionData не содержит нужное количество элементов, добавляем их
            while (m_CompletionData.Count < m_LevelProperties.Length + m_BranchLevelProperties.Length)
            {
                m_CompletionData.Add(new LevelScoreData());
            }

            //Устанавливаем основные эпизоды
            for (int i = 0; i < m_LevelProperties.Length; i++)
            {
                m_CompletionData[i].EpisodeName = m_LevelProperties[i].SceneName;
            }

            //Устанавливаем ответвленные эпизоды
            for (int i = 0; i < m_BranchLevelProperties.Length; i++)
            {
                m_CompletionData[m_LevelProperties.Length + i].EpisodeName = m_BranchLevelProperties[i].SceneName;
            }

            CalculateTotalScore();
        }

        private void CalculateTotalScore()
        {
            m_TotalScore = 0;

            foreach (var levelScore in m_CompletionData)
            {
                m_TotalScore += levelScore.Score;
            }
        }

        public bool TryIndex(int id, out string episodeName, out int levelScore)
        {
            if (id >= 0 && id < m_CompletionData.Count)
            {
                episodeName = m_CompletionData[id].EpisodeName;
                levelScore = m_CompletionData[id].Score;

                return true;
            }

            episodeName = null;
            levelScore = 0;

            return false;
        }

        /// <summary>
        /// Сохранение результата уровня.
        /// </summary>
        public void SaveLevelResult(int levelScore)
        {
            LevelProperties currentLevelProperties = LevelController.Instance.CurrentLevelProperties;
            BranchLevelProperties currentBranchLevelProperties = LevelController.Instance.CurrentBranchLevelProperties;

            if (currentLevelProperties != null)
                SaveResult(currentLevelProperties.SceneName, levelScore);

            if (currentBranchLevelProperties != null)
                SaveResult(currentBranchLevelProperties.SceneName, levelScore);
        }

        private void SaveResult(string episodeName, int levelScore)
        {
            //Находим соответствующую запись в m_CompletionData по EpisodeName
            for (int i = 0; i < m_CompletionData.Count; i++)
            {
                if (m_CompletionData[i].EpisodeName == episodeName)
                {
                    if (levelScore > m_CompletionData[i].Score) //Если новое значение больше текущего
                    {
                        m_CompletionData[i].Score = levelScore;

                        Saver<List<LevelScoreData>>.Save(filename, m_CompletionData); //Сохраняем изменения
                        CalculateTotalScore(); //Пересчитываем общие очки
                    }

                    return;
                }
            }
        }
    }
}