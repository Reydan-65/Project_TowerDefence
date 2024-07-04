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

        private static event Action<int> OnGoldUpdate;
        public static event Action<int> OnLivesUpdate;

        public static void GoldUpdateSubscribe(Action<int> action)
        {
            OnGoldUpdate += action;
            action(Instance.m_CurrentGold);
        }
        public static void GoldUpdateUnSubscribe(Action<int> action)
        {
            OnGoldUpdate -= action;
            action(Instance.m_CurrentGold);
        }

        public static void LivesUpdateSubscribe(Action<int> action)
        {
            OnLivesUpdate += action;
            action(Instance.CurrentNumLives);
        }
        public static void LivesUpdateUnSubscribe(Action<int> action)
        {
            OnLivesUpdate -= action;
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
                //if (m_CurrentGold >= m_TowerAsset.GoldCost)
                //{

                ChangeGold(-towerAsset.GoldCost);

                var tower = Instantiate(m_TowerPrefab, buildPoint.position, Quaternion.identity);

                tower.GetComponentInChildren<SpriteRenderer>().sprite = towerAsset.Sprite;
                tower.GetComponentInChildren<Turret>().TowerAsset = towerAsset;

                Destroy(buildPoint.gameObject);
                //}
            }
        }
    }
}