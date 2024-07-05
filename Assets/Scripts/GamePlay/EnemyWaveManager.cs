using UnityEngine;
using Common;
using SpaceShooter;

namespace TowerDefence
{
    public class EnemyWaveManager : MonoBehaviour
    {
        [SerializeField] private Enemy m_EnemyPrefab;
        [SerializeField] private Path[] m_Paths;
        [SerializeField] private EnemyWave m_CurrentWave;

        private void Start()
        {
            m_CurrentWave.Prepare(SpawnEnemies);
        }

        private void SpawnEnemies()
        {
            //Создать волну врагов
            foreach ((EnemyAsset enemyAsset, int count, int pathIndex) in m_CurrentWave.EnumerateSquads())
            {
                if (pathIndex < m_Paths.Length)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var enemy = Instantiate(m_EnemyPrefab, m_Paths[pathIndex].StartArea.GetRandomInsideZone(), Quaternion.identity);

                        enemy.Use(enemyAsset);
                        enemy.GetComponent<TD_PatrolController>().SetPath(m_Paths[pathIndex]);
                    }
                }
                else
                    Debug.Log($"Invalid pathIndex in {name}.");
            }

            //Готовится следующая волна
            m_CurrentWave = m_CurrentWave.PrepareNext(SpawnEnemies);
        }
    }
}