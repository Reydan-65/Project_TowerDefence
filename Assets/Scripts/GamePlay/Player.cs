using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TowerDefence;

namespace SpaceShooter
{
    public class Player : SingletonBase<Player>
    {
        public static SpaceShip SelectedSpaceShip;

        [SerializeField] protected int m_NumLives;
        protected int m_CurrentNumLives;
        public int CurrentNumLives { get { return m_CurrentNumLives; } }

        //[SerializeField] private SpaceShip m_PlayerShipPrefab;
        //[SerializeField] private float respawnTime;

        //public SpaceShip ActiveShip => m_Ship;

        //private FollowCamera m_FollowCamera;
        //private ShipInputController m_ShipInputController;
        protected Transform m_SpawnPoint;

        //public FollowCamera FollowCamera => m_FollowCamera;
        //public ShipInputController ShipInputController { get => m_ShipInputController; set => m_ShipInputController = value; }

        public void Construct(/*FollowCamera followCamera, ShipInputController shipInputController,*/ Transform spawnPoint)
        {
            /*m_FollowCamera = followCamera;
            m_ShipInputController = shipInputController;*/
            m_SpawnPoint = spawnPoint;
        }

        private SpaceShip m_Ship;
        private LevelCompletionScore m_LevelCompletionScore;

        protected int m_Score;
        protected int m_NumKills;
        protected int m_NumBossesIsDead;
        public int Score => m_Score;
        public int NumKills => m_NumKills;
        public int NumBossesIsDead => m_NumBossesIsDead;

        /*
        public SpaceShip ShipPrefab
        {
            get
            {
                if (SelectedSpaceShip == null) 
                    return m_PlayerShipPrefab;
                else 
                    return SelectedSpaceShip;
            }
        }
        */

        protected virtual void Start()
        {
            m_LevelCompletionScore = FindAnyObjectByType<LevelCompletionScore>();

            //Respawn();
        }

        /*
        private void OnShipDeath()
        {
            m_NumLives--;

            //m_FollowCamera.enabled = false;
            //m_ShipInputController.enabled = false;

            m_Ship.Rigid.bodyType = RigidbodyType2D.Static;

            StartCoroutine(ShipDestroyed());
        }

        private IEnumerator ShipDestroyed()
        {
            yield return new WaitForSeconds(respawnTime);

            if (m_NumLives > 0)
            {
                //m_FollowCamera.enabled = true;
                //m_ShipInputController.enabled = true;

                Respawn();
            }
        }

        private void Respawn()
        {
            var newPlayerShip = Instantiate(ShipPrefab, m_SpawnPoint.position, m_SpawnPoint.rotation);

            m_Ship = newPlayerShip.GetComponent<SpaceShip>();
            m_Ship.EventOnDeath.AddListener(OnShipDeath);
            m_Ship.Nickname = "Player";
            m_Ship.TeamId = 1;

            //m_FollowCamera.SetTarget(m_Ship.transform);
            //m_ShipInputController.SetTargetShip(m_Ship);

            //if (m_ShipInputController.Mode == ShipInputController.ControlMode.Mobile)
            //{
            //    VirtualGamePad gamePad = FindAnyObjectByType<VirtualGamePad>();

            //    gamePad.MobileFirePrimary.GetComponent<Image>().sprite = gamePad._Sprites[0];
            //    gamePad.MobileFireSecondary.GetComponent<Image>().enabled = false;
            //}
        }
        */
        /*
        public void AddScore(int num)
        {
            m_Score += num;
        }

        public void AddKill()
        {
            m_NumKills += 1;
        }

        
        public void AddBossDead()
        {
            m_NumBossesIsDead += 1;
        }
        */

        protected void TakeDamage(int damage)
        {
            m_CurrentNumLives -= damage;

            //if (m_CurrentNumLives <= 0)
            //{
            //    LevelController.Instance.RestartLevel();
            //}
        }

        public void ReduceEnemiesLast()
        {
            m_LevelCompletionScore.EnemiesLast -= 1;
        }
    }
}