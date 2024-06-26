using UnityEngine;

namespace TowerDefence
{
    public class EnemySpawner : Spawner
    {
        [Space(10)]
        [SerializeField] private Enemy m_EnemyPrefab;

        [SerializeField] private Path m_Path;
        [SerializeField] private EnemyAsset[] m_EnemyAssets;

        protected override GameObject GenerateSpawnedEntity()
        {
            var enemy = Instantiate(m_EnemyPrefab);

            enemy.Use(m_EnemyAssets[Random.Range(0, m_EnemyAssets.Length)]);
            enemy.GetComponent<TD_PatrolController>().SetPath(m_Path);

            /*
                if (enemy.transform.root.TryGetComponent(out SpaceShip ship) == true)
                {
                    SetTeamID(m_TeamID);

                    if (m_TeamID == 0) { ship.Nickname = "Neutral"; ship.TeamId = 0; }
                    if (m_TeamID == 1) { ship.Nickname = "Freandly"; ship.TeamId = 1; }
                    if (m_TeamID == 2) { ship.Nickname = "Enemy Ship"; ship.TeamId = 2; }

                 }
            */

            return enemy.gameObject;
        }
    }
}