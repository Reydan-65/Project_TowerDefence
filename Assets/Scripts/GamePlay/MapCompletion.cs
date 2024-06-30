using System;
using UnityEngine;
using SpaceShooter;

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

        [SerializeField] private LevelScoreData[] m_CompletionData;

        private void Awake()
        {
            Saver<LevelScoreData[]>.TryLoad(filename, ref m_CompletionData);

            for (int i = 0; i < m_CompletionData.Length; i++)
            {
                m_CompletionData[i].EpisodeName = LevelSequencesController.Instance.LevelSequences.LevelsProperties[i].SceneName;
            }
        }

        public bool TryIndex(int id, out string episodeName, out int levelScore)
        {
            if (id >= 0 && id < m_CompletionData.Length)
            {
                episodeName = m_CompletionData[id].EpisodeName;
                levelScore = m_CompletionData[id].Score;

                return true;
            }

            episodeName = null;
            levelScore = 0;

            return false;
        }

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
    }
}