using UnityEngine;
using System;
using SpaceShooter;
using UnityEngine.UI;

namespace TowerDefence
{
    public class EnemyWaveManager : MonoBehaviour
    {
        public static event Action<Enemy> OnEnemySpawn;

        [SerializeField] private Enemy m_EnemyPrefab;
        [SerializeField] private Path[] m_Paths;
        [SerializeField] private EnemyWave m_CurrentWave;

        public EnemyWave CurrentWave => m_CurrentWave;

        public event Action OnAllWavesDead;

        private int m_ActiveEnemyCount = 0;
        private NextWave_GUI m_NextWaveGUI;

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
                        Vector3 spawnPosition = m_Paths[pathIndex].StartArea.GetRandomInsideZone();

                        var enemy = Instantiate(m_EnemyPrefab, spawnPosition, Quaternion.identity);

                        enemy.OnEnemyDestroy += RecordEnemyDead;
                        enemy.Use(enemyAsset);

                        TD_PatrolController patrolController = enemy.GetComponent<TD_PatrolController>();

                        if (patrolController != null)
                            enemy.GetComponent<TD_PatrolController>().SetPath(m_Paths[pathIndex]);
                        else
                            Debug.LogWarning($"No TD_PatrolController found on {enemy.name}. Ensure TD_PatrolController is attached.");
                        
                        var images = enemy.GetComponentInChildren<HitPointBar>().GetComponentsInChildren<Image>();

                        foreach (var image in images)
                            image.enabled = false;

                        m_ActiveEnemyCount++;
                        OnEnemySpawn?.Invoke(enemy);
                    }
                }
                else
                    Debug.LogWarning($"Invalid pathIndex {pathIndex} in {name}. Ensure m_Paths is correctly assigned.");
            }

            //Обнуляем прогресс бар
            if (m_NextWaveGUI != null)
            {
                m_NextWaveGUI.NextWaveBar.fillAmount = 0f;
                m_NextWaveGUI.WaveIndex++;
            }

            //Готовится следующая волна
            m_CurrentWave = m_CurrentWave.PrepareNext(SpawnEnemies);
        }

        /// <summary>
        /// Считаем оставшихся врагов на сцене.
        /// Если уменьшенное количество врагов равно нулю и,
        ///      если есть готовая волна - вызваем новую волну,
        ///      иначе вызываем событие, что все волны врагов мертвы.
        /// </summary>
        public void RecordEnemyDead()
        {
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

        /// <summary>
        /// Принудительный вызов новой волны, если она есть.
        /// И получение игроком золота (в зависимости от времени,
        /// оставшегося до выхода новой волны).
        /// </summary>  
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

        //При выходе из программы иногда создаются враги...
        //Очистка памяти.
        private void OnApplicationQuit()
        {
            m_CurrentWave = null;
            m_NextWaveGUI.NextWaveBar = null;

            Enemy[] enemies = FindObjectsOfType<Enemy>();

            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.OnEnemyDestroy -= RecordEnemyDead;
                    Destroy(enemy);
                }
            }
        }
    }
}