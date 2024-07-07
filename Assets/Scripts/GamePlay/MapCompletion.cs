using System;
using UnityEngine;
using SpaceShooter;
using Unity.VisualScripting;
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

        [SerializeField] private List<LevelScoreData> m_CompletionData = new List<LevelScoreData>();

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
        /*
        public void SaveLevelResult(int levelScore)
        {
            Instance.SaveResult(LevelController.Instance.CurrentLevelProperties, levelScore);
        }
        
        private void SaveResult(LevelProperties currentLevelProperties, int levelScore)
        {
            foreach (var item in m_CompletionData)
            {
                if (item.EpisodeName == currentLevelProperties.SceneName)
                {
                    if (levelScore > item.Score)
                    {
                        item.Score = levelScore;

                        Saver<LevelScoreData[]>.Save(filename, m_CompletionData);
                    }
                }
            }
        }
        */

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
                    if (levelScore > m_CompletionData[i].Score)
                    {
                        m_CompletionData[i].Score = levelScore;

                        //Сохраняем изменения
                        Saver<List<LevelScoreData>>.Save(filename, m_CompletionData);
                        CalculateTotalScore(); //Пересчитываем общие очки
                    }

                    return;
                }
            }
        }
    }
}