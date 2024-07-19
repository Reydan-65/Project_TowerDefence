using TowerDefence;
using UnityEngine;

namespace SpaceShooter
{
    public class PlayerSpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        //[SerializeField] private FollowCamera m_followCameraPrefab;
        [SerializeField] private Player m_PlayerPrefab;
        //[SerializeField] private ShipInputController m_ShipInputControllerPrefab;
        //[SerializeField] private VirtualGamePad m_VirtualGamePadPrefab;

        public Player Spawn()
        {
            //FollowCamera followCamera = Instantiate(m_followCameraPrefab, m_SpawnPoint);
            //VirtualGamePad virtualGamePad = Instantiate(m_VirtualGamePadPrefab);

            //ShipInputController shipInputController = Instantiate(m_ShipInputControllerPrefab);
            //shipInputController.Construct(virtualGamePad);

            Player player = Instantiate(m_PlayerPrefab);
            player.Construct(/*followCamera, shipInputController,*/ transform);

            return player;
        }
    }
}