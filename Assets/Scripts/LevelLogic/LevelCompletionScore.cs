using TowerDefence;
using UnityEngine;

namespace SpaceShooter
{
    public class LevelCompletionScore : LevelCondition
    {
        public class Stats
        {
            public int numKills;
            public float time;
            public int score;
        }

        public static Stats TotalStats { get; private set; }

        //[SerializeField] private EnemySpawner[] m_EnemySpawners;
        [SerializeField] private float m_TimeLimit;

        //[SerializeField] private int m_Score;
        //[SerializeField] private int m_Kills;
        //[SerializeField] private int m_BossesIsDead;

        //private LevelCompletionPosition m_LevelExit;
        /*
        private int m_EnemiesLast;
        public int EnemiesLast { get => m_EnemiesLast; set => m_EnemiesLast = value; }

        private void Start()
        {
            int sum = 0;
            int[] sumSpawned = new int[m_EnemySpawners.Length];

            for (int i = 0; i < m_EnemySpawners.Length; i++)
            {
                sumSpawned[i] = m_EnemySpawners[i].NumSpawns * m_EnemySpawners[i].MaxCountSpawn;
                sum += sumSpawned[i];
            }

            m_EnemiesLast = sum;
        }
        
        
        private void UpdateCurrentLevelStats()
        {
            // Бонус за время прохождения
            int timeBonus = (int)(LevelController.Instance.ReferenceTime - LevelController.Instance.LevelTime);

            if (timeBonus > 0)
            {
                TotalStats.score += timeBonus;
            }
        }
        */

        private void Start()
        {
            FindObjectOfType<EnemyWaveManager>().OnAllWavesDead += () =>
            {
                IsCompleted = true;
            };
        }

        /*
        public override bool IsCompleted
        {
            get
            {
                if (EnemiesLast <= 0 || LevelController.Instance.LevelTime >= m_TimeLimit)
                {
                    return true;
                }
                
                return false;
            }
        }*/

    }
}