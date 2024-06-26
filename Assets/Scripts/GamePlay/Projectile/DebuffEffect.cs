using UnityEngine;
using System.Collections;
using TowerDefence;

namespace SpaceShooter
{
    public class DebuffEffect : MonoBehaviour
    {
        public enum DebuffType
        {
            Null,
            Frost
        }

        [SerializeField] private DebuffType m_Type;

        public DebuffType Type { get => m_Type; set => m_Type = value; }

        private float m_DebuffTime = 1;
        public float DebuffTime { get => m_DebuffTime; set => m_DebuffTime = value; }

        private void Start()
        {
            if (m_Type == DebuffType.Frost)
            {
                if (transform.root.TryGetComponent(out SpaceShip ship) == true)
                {
                    ship.MaxLinearVelocity /= 2;
                    ship.Thrust /= 2;
                }
            }

            StartCoroutine(OnDisableEnd());
        }

        private IEnumerator OnDisableEnd()
        {
            yield return new WaitForSeconds(m_DebuffTime);

            if (m_Type == DebuffType.Frost)
            {
                if (transform.root.TryGetComponent(out SpaceShip ship) == true)
                {
                    ship.MaxLinearVelocity *= 2;
                    ship.Thrust *= 2;
                }
            }

            Destroy(this);
        }
    }
}