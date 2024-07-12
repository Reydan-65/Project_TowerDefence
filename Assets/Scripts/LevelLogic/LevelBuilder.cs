using UnityEngine;
using TowerDefence;
using Common;

namespace SpaceShooter
{
    public class LevelBuilder : MonoBehaviour
    {
        /*
        [Header("Prefabs")]
        [SerializeField] private GameObject m_PlayerHUDPrefab;
        [SerializeField] private GameObject m_LevelGUIPrefab;
        [SerializeField] private GameObject m_BackgroundPrefab;
        */

        [Header("Dependencies")]
        [SerializeField] private PlayerSpawner m_PlayerSpawner;
        //[SerializeField] private LevelBoundary m_LevelBoundary;
        [SerializeField] private LevelController m_LevelController;
        //[SerializeField] private SoundManager m_SoundManager;
        [SerializeField] private Abilities m_Abilities;
        [SerializeField] private ClickProtection m_ClickProtection;

        private void Awake()
        {
            //m_LevelBoundary.Init();
            m_LevelController.Init();

            TD_Player player = (TD_Player)m_PlayerSpawner.Spawn();

            player.Init();
            m_Abilities.Init();
            m_ClickProtection.Init();
            //m_SoundManager.Init();

            //Instantiate(m_PlayerHUDPrefab);       
            //Instantiate(m_LevelGUIPrefab);

            //GameObject background = Instantiate(m_BackgroundPrefab);
            //background.AddComponent<SyncTransform>().SetSyncTarget(player.FollowCamera.transform);
        }
    }
}