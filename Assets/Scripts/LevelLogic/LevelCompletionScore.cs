using TowerDefence;
using UnityEngine;

namespace SpaceShooter
{
    public class LevelCompletionScore : LevelCondition
    {
        //[SerializeField] private int m_Score;
        [SerializeField] private EnemySpawner[] m_EnemySpawners;
        //[SerializeField] private int m_Kills;
        //[SerializeField] private int m_BossesIsDead;

        //private LevelCompletionPosition m_LevelExit;

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

        public override bool IsCompleted
        {
            get
            {
                //if (Player.Instance.ActiveShip == null) return false;

                if (EnemiesLast <= 0 /*Player.Instance.Score >= m_Score && Player.Instance.NumKills >= m_Kills &&
                    Player.Instance.NumBossesIsDead == m_BossesIsDead*/)
                {
                    //m_LevelExit.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);

                    return true;
                }
                
                return false;
            }
        }
    }
}