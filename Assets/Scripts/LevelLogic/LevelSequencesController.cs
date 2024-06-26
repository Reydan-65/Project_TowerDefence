using UnityEngine;
using UnityEngine.SceneManagement;
using TowerDefence;

namespace SpaceShooter
{
    public class LevelSequencesController : SingletonBase<LevelSequencesController>
    {
        public LevelSequences LevelSequences;

        public bool CurrentLevelIsLast()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            string lastLevelSceneName = LevelSequences.LevelsProperties[LevelSequences.LevelsProperties.Length - 1].SceneName;

            return lastLevelSceneName == sceneName;
        }

        public LevelProperties GetCurrentLoadedLevel()
        {
            string sceneName = SceneManager.GetActiveScene().name;

            for (int i = 0; i < LevelSequences.LevelsProperties.Length; i++)
            {
                if (LevelSequences.LevelsProperties[i].SceneName == sceneName)
                    return LevelSequences.LevelsProperties[i];
            }

            Debug.Log("Level properties not found!");
            return null;
        }

        public LevelProperties GetNextLevelProperties(LevelProperties levelProperties)
        {
            for (int i = 0; i < LevelSequences.LevelsProperties.Length; i++)
            {
                if (LevelSequences.LevelsProperties[i].SceneName == levelProperties.SceneName)
                {
                    if (i < LevelSequences.LevelsProperties.Length - 1)
                        return LevelSequences.LevelsProperties[i + 1];
                }
            }

            Debug.Log("Level properties is last!");
            return null;
        }
    }
}