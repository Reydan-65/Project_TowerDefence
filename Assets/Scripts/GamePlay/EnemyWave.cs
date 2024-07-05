using UnityEngine;
using System;
using System.Collections.Generic;

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
        [SerializeField] private float m_PrepareTime = 10f;

        public float GetRemainingTime() { return m_PrepareTime - Time.time; }

        private void Awake()
        {
            enabled = false;
        }

        private event Action OnWaveReady;

        public void Prepare(Action spawnEnemies)
        {
            OnWavePrepared?.Invoke(m_PrepareTime);

            m_PrepareTime += Time.time;
            enabled = true;
            OnWaveReady += spawnEnemies;
        }

        private void Update()
        {
            if (Time.time >= m_PrepareTime)
            {
                enabled = false;
                OnWaveReady?.Invoke();
            }
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

        [SerializeField] private EnemyWave m_NextWave;

        public EnemyWave PrepareNext(Action spawnEnemies)
        {
            OnWaveReady -= spawnEnemies;

            if (m_NextWave)
                m_NextWave.Prepare(spawnEnemies);

            return m_NextWave;
        }
    }
}