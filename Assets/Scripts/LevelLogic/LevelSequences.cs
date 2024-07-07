using UnityEngine;
using TowerDefence;

namespace SpaceShooter
{
    [CreateAssetMenu]
    public class LevelSequences : ScriptableObject
    {
        [Space(10)]
        public LevelProperties[] LevelsProperties;

        [Space(10)]
        public BranchLevelProperties[] BranchLevelsProperties;
    }
}