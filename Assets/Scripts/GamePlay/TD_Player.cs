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

        public event Action<int> OnLivesUpdate;
        private event Action<int> OnGoldUpdate;
        public event Action<int> OnEnergyUpdate;

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

        public void EnergyUpdateSubscribe(Action<int> action)
        {
            OnEnergyUpdate += action;
            action(Instance.CurrentNumEnergy);
        }

        [SerializeField] private int m_StartGold;
        public int StartGold => m_StartGold;

        private int m_CurrentGold;
        public int CurrentGold { get => m_CurrentGold; set => m_CurrentGold = value; }

        private void Awake()
        {
            m_CurrentNumLives = m_StartNumLives;
            m_CurrentGold = m_StartGold;
            m_CurrentNumEnergy = m_StartEnergy;
            m_CurrentGoldAddRate = m_BaseGoldAddRate;
            m_CurrentEnergyRegenRate = m_BaseEnergyRegenRate;
        }

        protected override void Start()
        {
            base.Start();

            var levelHealthUpgrade = Upgrades.GetUpgradeLevel(Upgrades.Instance.Assets.PlayerProperties[0].UpgradeName);
            ReduceLives(-levelHealthUpgrade * 5);

            var levelGoldUpgrade = Upgrades.GetUpgradeLevel(Upgrades.Instance.Assets.PlayerProperties[1].UpgradeName);
            ChangeGold(levelGoldUpgrade * 5);

            //ѕроверка доступности хот€ бы одно из способностей.
            bool allUpgradesLevelAtZero = true;

            foreach (var upgrade in Upgrades.Instance.Assets.AbilityProperties)
            {
                if (Upgrades.GetUpgradeLevel(upgrade.UpgradeName) != 0)
                {
                    allUpgradesLevelAtZero = false;
                    break;
                }
            }

            TextUpdate[] textUpdates = FindObjectsOfType<TextUpdate>();

            if (textUpdates != null)
            {
                foreach (var text in textUpdates)
                {
                    if (text.transform.parent.name == "PlayerEnergy_InfoPanel")
                        text.transform.parent.gameObject.SetActive(!allUpgradesLevelAtZero);
                }
            }
        }

        private void Update()
        {
            AddGoldRate(Time.deltaTime);
            RegenRateEnergy(Time.deltaTime);
        }

        private void AddGoldRate(float deltaTime)
        {
            m_AddGoldTimer += deltaTime;

            if (m_AddGoldTimer >= 1.0f)
            {
                m_CurrentGold = (int)MathF.Min(m_CurrentGold + m_CurrentGoldAddRate, 999);
                m_AddGoldTimer = 0.0f;
                OnGoldUpdate(m_CurrentGold);
            }
        }

        private void RegenRateEnergy(float deltaTime)
        {
            m_EnergyRegenTimer += deltaTime;

            if (m_EnergyRegenTimer >= 1.0f)
            {
                m_CurrentNumEnergy = (int)MathF.Min(m_CurrentNumEnergy + m_CurrentEnergyRegenRate, m_MaxEnergy);
                m_EnergyRegenTimer = 0.0f;
                OnEnergyUpdate(m_CurrentNumEnergy);
            }
        }

        public void ReduceLives(int value)
        {
            TakeDamage(value);
            OnLivesUpdate(m_CurrentNumLives);
        }

        public void ChangeEnergy(int value)
        {
            m_CurrentNumEnergy += value;
            OnEnergyUpdate(m_CurrentNumEnergy);
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