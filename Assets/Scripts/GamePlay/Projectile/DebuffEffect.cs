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

        private DebuffType m_Type;
        private float m_DebuffTime = 1;
        private float m_DebuffSlowPower;

        public DebuffType Type { get => m_Type; set => m_Type = value; }

        public float DebuffTime { get => m_DebuffTime; set => m_DebuffTime = value; }
        public float DebuffSlowPower { get => m_DebuffSlowPower; set => m_DebuffSlowPower = value; }

        private void Start()
        {
            if (m_Type == DebuffType.Frost)
            {
                if (transform.root.TryGetComponent(out SpaceShip ship) == true)
                {
                    ship.ChangeMaxLinearVelocityOnValue(0.5f);
                    //ship.Thrust /= 2;
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
                    ship.RestoreMaxLinearVelocityOnValue();
                    //ship.Thrust *= 2;
                }
            }

            Destroy(this);
        }
    }
}