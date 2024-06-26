using UnityEngine;
using Common;

namespace TowerDefence
{
    public class EntitiesSpawner : Spawner
    {
        [Space(10)]
        [SerializeField] private Entity[] m_EntityPrefabs;

        protected override GameObject GenerateSpawnedEntity()
        {
            return Instantiate(m_EntityPrefabs[Random.Range(0, m_EntityPrefabs.Length)].gameObject);
        }
    }
}