using UnityEngine;
using System;
using SpaceShooter;
using UnityEngine.UI;
using System.Collections;

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
        private int m_EnemyCountInWave;
        public int EnemyCountInWave => m_EnemyCountInWave;

        private void Start()
        {
            m_CurrentWave.Prepare(SpawnWave);
            m_NextWaveGUI = FindObjectOfType<NextWave_GUI>();
        }

        /// <summary>
        /// Определение общего количества врагов для текущей волны и их создание.
        /// </summary>
        private void SpawnWave()
        {
            m_EnemyCountInWave = 0;

            foreach ((EnemyAsset enemyAsset, int count, int pathIndex) in m_CurrentWave.EnumerateSquads())
            {
                m_EnemyCountInWave += count;
            }
            m_NextWaveGUI.SwitchEnabledForceNextWaveButton(false);
            StartCoroutine(SpawnEnemies());
        }

        /// <summary>
        /// Создание врагов:
        /// - их тип, количество и путь заданы в настройках волны
        /// - они создаются один за другим.
        /// </summary>
        private IEnumerator SpawnEnemies()
        {
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
                            patrolController.SetPath(m_Paths[pathIndex]);
                        else
                            Debug.LogWarning($"No TD_PatrolController found on {enemy.name}. Ensure TD_PatrolController is attached.");

                        var images = enemy.GetComponentInChildren<HitPointBar>().GetComponentsInChildren<Image>();

                        foreach (var image in images)
                            image.enabled = false;

                        m_ActiveEnemyCount++;
                        OnEnemySpawn?.Invoke(enemy);

                        yield return new WaitForSeconds(CurrentWave.SpawnDelayForEachEnemyInWave);
                    }
                }
                else
                    Debug.LogWarning($"Invalid pathIndex {pathIndex} in {name}. Ensure m_Paths is correctly assigned.");
            }

            //Готовится следующая волна
            m_CurrentWave = m_CurrentWave.PrepareNext(SpawnWave);

            // Если след. волна есть, включаем кнопку вызова след. волны
            if (m_CurrentWave)
            {
                m_NextWaveGUI.SwitchEnabledForceNextWaveButton(true);

                //Обнуляем прогресс бар
                if (m_NextWaveGUI != null)
                {
                    m_NextWaveGUI.NextWaveBar.fillAmount = 0f;
                    m_NextWaveGUI.WaveIndex++;
                }
            }
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

                    //Награда за завершение прошлой волны
                    var levelGoldUpgrade = Upgrades.GetUpgradeLevel(Upgrades.Instance.Assets.PlayerProperties[1].UpgradeName);
                    TD_Player.Instance.ChangeGold(TD_Player.Instance.StartGold + levelGoldUpgrade * 5);

                    m_NextWaveGUI.SwitchEnabledForceNextWaveButton(false);
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
                TD_Player.Instance.ChangeGold((int)(m_CurrentWave.GetRemainingTime() * 0.5f));

                //Вызов волны
                SpawnWave();
            }
        }

        private void OnDestroy()
        {
            ClearScene();
        }
        
        private void OnApplicationQuit()
        {
            ClearScene();
        }

        private void ClearScene()
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