using TMPro;
using UnityEngine;

namespace TowerDefence
{
    public class NextWave_GUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_BonusAmount;

        private EnemyWaveManager m_EnemyWaveManager;
        private float m_TimeNextWave;

        private void Start()
        {
            m_EnemyWaveManager = FindObjectOfType<EnemyWaveManager>();

            EnemyWave.OnWavePrepared += (float time) =>
            {
                m_TimeNextWave = time;
            };
        }

        public void EX_CallWave()
        {
            m_EnemyWaveManager.ForceNextWave();
        }

        private void Update()
        {
            var bonus = (int)m_TimeNextWave;

            if (bonus < 0) bonus = 0;

            m_BonusAmount.text = bonus.ToString();
            m_TimeNextWave -= Time.deltaTime;
        }
    }
}