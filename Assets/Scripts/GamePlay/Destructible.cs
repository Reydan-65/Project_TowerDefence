using UnityEngine;
using Common;
using TowerDefence;

namespace SpaceShooter
{
    public class Destructible : DestructibleBase
    {
        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;

        protected override void OnDeath()
        {
            Sound.EnemyDie.Play();

            Destroy(gameObject);
            base.OnDeath();
        }

        protected void Use(EnemyAsset asset)
        {
            Nickname = asset.Name;
            m_TeamId = asset.TeamID;

            m_HitPoints = asset.HitPoints;
            m_ScoreValue = asset.ScoreValue;
        }
    }
}