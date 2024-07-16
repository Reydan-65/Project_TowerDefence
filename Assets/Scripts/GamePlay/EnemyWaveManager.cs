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
        /// ����������� ������ ���������� ������ ��� ������� ����� � �� ��������.
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
        /// �������� ������:
        /// - �� ���, ���������� � ���� ������ � ���������� �����
        /// - ��� ��������� ���� �� ������.
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

            //��������� ��������� �����
            m_CurrentWave = m_CurrentWave.PrepareNext(SpawnWave);

            // ���� ����. ����� ����, �������� ������ ������ ����. �����
            if (m_CurrentWave)
            {
                m_NextWaveGUI.SwitchEnabledForceNextWaveButton(true);

                //�������� �������� ���
                if (m_NextWaveGUI != null)
                {
                    m_NextWaveGUI.NextWaveBar.fillAmount = 0f;
                    m_NextWaveGUI.WaveIndex++;
                }
            }
        }

        /// <summary>
        /// ������� ���������� ������ �� �����.
        /// ���� ����������� ���������� ������ ����� ���� �,
        ///      ���� ���� ������� ����� - ������� ����� �����,
        ///      ����� �������� �������, ��� ��� ����� ������ ������.
        /// </summary>
        public void RecordEnemyDead()
        {
            if (--m_ActiveEnemyCount == 0)
            {
                if (m_CurrentWave)
                {
                    ForceNextWave();

                    //������� �� ���������� ������� �����
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
        /// �������������� ����� ����� �����, ���� ��� ����.
        /// � ��������� ������� ������ (� ����������� �� �������,
        /// ����������� �� ������ ����� �����).
        /// </summary>  
        public void ForceNextWave()
        {
            if (m_CurrentWave)
            {
                //������� �� �������������� ����� �����
                TD_Player.Instance.ChangeGold((int)(m_CurrentWave.GetRemainingTime() * 0.5f));

                //����� �����
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