using UnityEngine;
using Common;
using SpaceShooter;

namespace TowerDefence
{
    public abstract class Spawner : MonoBehaviour
    {
        protected abstract GameObject GenerateSpawnedEntity();

        /// <summary>
        /// Режим спавна
        /// </summary>
        public enum SpawnMode
        {
            Start,
            Loop
        }

        [SerializeField] protected SpawnMode m_SpawnMode;

        [Range(0, 3)]
        [SerializeField] protected int m_TeamID;
        [SerializeField] protected CircleArea m_Area;

        [Header("Spawn Settings")]
        [SerializeField] protected int m_NumSpawns;
        [SerializeField] protected float m_RespawnTime;
        [Header("If Loop Mode")]
        [SerializeField] protected int m_MaxCountSpawn;

        //protected FollowCamera m_Camera;

        public int NumSpawns => m_NumSpawns;
        public int MaxCountSpawn => m_MaxCountSpawn;

        protected int m_CountSpawn;
        protected float m_Timer;

        public int CountSpawned { get => m_CountSpawn; set => m_CountSpawn = value; }

        protected virtual void Start()
        {
            //m_Camera = FindAnyObjectByType<FollowCamera>();

            m_CountSpawn = 0;

            if (m_SpawnMode == SpawnMode.Start)
            {
                Spawn();
            }

            m_Timer = m_RespawnTime;
        }

        protected virtual void FixedUpdate()
        {
            if (m_Timer > 0)
                m_Timer -= Time.deltaTime;

            if (m_SpawnMode == SpawnMode.Loop && m_Timer < 0)
            {
                if (m_CountSpawn < m_MaxCountSpawn)
                {
                    Spawn();

                    m_CountSpawn++;

                    m_Timer = m_RespawnTime;
                }
            }
        }

        protected virtual void Spawn() 
        {
            for (int i = 0; i < m_NumSpawns; ++i)
            {
                var entity = GenerateSpawnedEntity();
                entity.transform.position = m_Area.GetRandomInsideZone();
            }
        }

        protected int SetTeamID(int teamID)
        {
            return teamID;
        }

        //protected bool IsInCameraView(Vector2 position)
        //{
        //    Vector3 screenPoint = m_Camera.GetComponent<Camera>().WorldToScreenPoint(position);
        //    return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        //}
    }
}