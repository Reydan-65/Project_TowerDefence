using UnityEngine;
using System;
using System.Collections.Generic;
using SpaceShooter;
using Unity.VisualScripting;

namespace TowerDefence
{
    public class EnemyWave : MonoBehaviour
    {
        public static Action<float> OnWavePrepared;

        [Serializable]
        private class Squad
        {
            public EnemyAsset Asset;
            public int Count;
        }

        [Serializable]
        private class PathGroup
        {
            public Squad[] Squads;
        }

        [SerializeField] private PathGroup[] m_PathGroups;
        [SerializeField] private float m_PrepareTime;
        [SerializeField] private float m_SpawnDelayForEachEnemyInWave;
        [SerializeField] private EnemyWave m_NextWave;

        private event Action OnWaveReady;

        public float SpawnDelayForEachEnemyInWave => m_SpawnDelayForEachEnemyInWave;
        public float PrepareTime => m_PrepareTime;
        public float GetRemainingTime() { return m_PrepareTime - Time.time; }

        private void Awake()
        {
            enabled = false;
        }

        private void Update()
        {
            if (PausePanel.isPaused) m_PrepareTime += Time.deltaTime;

            if (Time.time >= m_PrepareTime)
            {
                enabled = false;
                OnWaveReady?.Invoke();
            }
        }

        public void Prepare(Action spawnEnemies)
        {
            OnWavePrepared?.Invoke(m_PrepareTime);

            m_PrepareTime = Time.time + m_PrepareTime;
            enabled = true;
            OnWaveReady += spawnEnemies;
        }

        public IEnumerable<(EnemyAsset enemyAsset, int count, int pathIndex)> EnumerateSquads()
        {
            for (int i = 0; i < m_PathGroups.Length; i++)
            {
                foreach (var squad in m_PathGroups[i].Squads)
                {
                    yield return (squad.Asset, squad.Count, i);
                }
            }
        }

        public EnemyWave PrepareNext(Action spawnEnemies)
        {
            OnWaveReady -= spawnEnemies;

            if (m_NextWave)
                m_NextWave.Prepare(spawnEnemies);
            else
                return null;

            return m_NextWave;
        }
    }
}