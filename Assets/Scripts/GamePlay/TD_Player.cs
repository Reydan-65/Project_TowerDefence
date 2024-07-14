using UnityEngine;
using SpaceShooter;
using System;

namespace TowerDefence
{
    public class TD_Player : Player
    {
        public static new TD_Player Instance
        {
            get
            {
                return Player.Instance as TD_Player;
            }
        }

        private event Action<int> OnGoldUpdate;
        public event Action<int> OnLivesUpdate;

        public void GoldUpdateSubscribe(Action<int> action)
        {
            OnGoldUpdate += action;
            action(Instance.m_CurrentGold);
        }

        public void LivesUpdateSubscribe(Action<int> action)
        {
            OnLivesUpdate += action;
            action(Instance.CurrentNumLives);
        }

        [SerializeField] private int m_StartGold;

        private int m_CurrentGold;
        public int CurrentGold { get => m_CurrentGold; set => m_CurrentGold = value; }

        private void Awake()
        {
            m_CurrentNumLives = m_NumLives;
            m_CurrentGold = m_StartGold;

        }

        protected override void Start()
        {
            base.Start();

            var levelHealthUpgrade = Upgrades.GetUpgradeLevel(Upgrades.Instance.Assets.PlayerProperties[0].UpgradeName);
            ReduceLives(-levelHealthUpgrade * 5);

            var levelGoldUpgrade = Upgrades.GetUpgradeLevel(Upgrades.Instance.Assets.PlayerProperties[1].UpgradeName);
            ChangeGold(levelGoldUpgrade * 5);
        }

        public void ReduceLives(int value)
        {
            TakeDamage(value);
            OnLivesUpdate(m_CurrentNumLives);
        }

        public void ChangeGold(int value)
        {
            m_CurrentGold += value;
            OnGoldUpdate(CurrentGold);
        }

        [SerializeField] private Tower m_TowerPrefab;

        public void TryBuild(TowerAsset towerAsset, Transform buildPoint)
        {
            if (buildPoint != null)
            {
                ChangeGold(-towerAsset.GoldCost);

                var tower = Instantiate(m_TowerPrefab, buildPoint.position, Quaternion.identity);

                tower.GetComponentInChildren<Turret>().TowerAsset = towerAsset;
                tower.Use(towerAsset);

                Destroy(buildPoint.gameObject);
            }
        }
    }
}