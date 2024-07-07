using UnityEngine;
using System;

namespace TowerDefence
{
    public class EnemyWaveManager : MonoBehaviour
    {
        [SerializeField] private Enemy m_EnemyPrefab;
        [SerializeField] private Path[] m_Paths;
        [SerializeField] private EnemyWave m_CurrentWave;
        public EnemyWave CurrentWave => m_CurrentWave;

        public event Action OnAllWavesDead;

        private int m_ActiveEnemyCount = 0;
        private NextWave_GUI m_NextWaveGUI;

        public void RecordEnemyDead()
        {
            //Если уменьшенное количество врагов равно нулю
            if (--m_ActiveEnemyCount == 0)
            {
                if (m_CurrentWave)
                {
                    ForceNextWave();
                }
                else
                {
                    print("All waves dead!");
                    OnAllWavesDead?.Invoke();
                }
            }
        }

        private void Start()
        {
            m_CurrentWave.Prepare(SpawnEnemies);
            m_NextWaveGUI = FindObjectOfType<NextWave_GUI>();
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

                        enemy.OnEnemyDestroy += RecordEnemyDead;
                        enemy.Use(enemyAsset);
                        enemy.GetComponent<TD_PatrolController>().SetPath(m_Paths[pathIndex]);

                        m_ActiveEnemyCount++;
                    }
                }
                else
                    Debug.Log($"Invalid pathIndex in {name}.");
            }

            //Обнуляем прогресс бар
            m_NextWaveGUI.NextWaveBar.fillAmount = 0f;
            m_NextWaveGUI.WaveIndex++;

            //Готовится следующая волна
            m_CurrentWave = m_CurrentWave.PrepareNext(SpawnEnemies);
        }

        public void ForceNextWave()
        {
            if (m_CurrentWave)
            {
                //Награда за принудительный вызов волны
                TD_Player.Instance.ChangeGold((int)m_CurrentWave.GetRemainingTime());

                //Вызов волны
                SpawnEnemies();
            }
        }
    }
}