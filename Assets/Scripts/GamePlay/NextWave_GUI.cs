using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class NextWave_GUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_BonusText;
        [SerializeField] private TextMeshProUGUI m_BonusAmount;
        [SerializeField] private Image m_BonusImage;
        [SerializeField] private Image m_NextWaveBar;
        [SerializeField] private Image m_ButtonImage;
        [SerializeField] private Sprite m_CrossSprite;

        private EnemyWaveManager m_EnemyWaveManager;
        private EnemyWave[] m_EnemyWaves;
        private float[] m_TimeToSpawn;
        private float m_TimeNextWave;
        private int m_WaveIndex;
        public int WaveIndex { get => m_WaveIndex; set => m_WaveIndex = value; }

        public Image NextWaveBar { get => m_NextWaveBar; set => m_NextWaveBar = value; }

        #region Unity Events

        private void Start()
        {
            m_WaveIndex = 0;

            m_EnemyWaveManager = FindObjectOfType<EnemyWaveManager>();
            m_EnemyWaves = m_EnemyWaveManager.GetComponentsInChildren<EnemyWave>();

            if (m_EnemyWaves != null)
            {
                m_TimeToSpawn = new float[m_EnemyWaves.Length];

                for (int i = 0; i < m_EnemyWaves.Length; i++)
                {
                    m_TimeToSpawn[i] = m_EnemyWaves[i].PrepareTime;
                }
            }

            SwitchEnabledForceNextWaveButton(false);

            EnemyWave.OnWavePrepared += (float time) =>
            {
                m_TimeNextWave = time;

                if (m_WaveIndex != 0)
                {
                    SwitchEnabledForceNextWaveButton(true);
                }
            };
        }

        private void Update()
        {
            var bonus = (int)(m_TimeNextWave * 0.5f);

            if (bonus < 0) bonus = 0;
            m_BonusAmount.text = bonus.ToString();

            if (m_WaveIndex >= 0 && m_WaveIndex < m_EnemyWaves.Length && m_ButtonImage.GetComponent<Button>().interactable == true)
            {
                m_TimeNextWave -= Time.deltaTime;
                m_NextWaveBar.enabled = true;
                m_NextWaveBar.fillAmount += Time.deltaTime / m_TimeToSpawn[m_WaveIndex];
            }
            else
                m_NextWaveBar.enabled = false;

            if (m_WaveIndex == m_EnemyWaves.Length)
            {
                SwitchEnabledForceNextWaveButton(false);
                m_ButtonImage.sprite = m_CrossSprite;
            }
        }

        #endregion

        /// <summary>
        /// Принудительный вызов новой волны, если она есть.
        /// - закрыть BuyControl, если открыт;
        /// - отключить кнопку вызова волны, до окончания текущего призыва.
        /// </summary>
        public void EX_CallWave()
        {
            float delay = m_EnemyWaveManager.CurrentWave.SpawnDelayForEachEnemyInWave * m_EnemyWaveManager.EnemyCountInWave;

            m_EnemyWaveManager.ForceNextWave();

            BuyControl bc = FindObjectOfType<BuyControl>();

            if (bc != null) bc.gameObject.SetActive(false);

            StartCoroutine(WaitCooldown(delay));
        }

        private IEnumerator WaitCooldown(float delay)
        {
            SwitchEnabledForceNextWaveButton(false);

            yield return new WaitForSeconds(delay);

            SwitchEnabledForceNextWaveButton(true);
        }

        public void SwitchEnabledForceNextWaveButton(bool value)
        {
            if (m_ButtonImage != null)
            {
                Button button = m_ButtonImage.GetComponent<Button>();

                if (button != null)
                {
                    button.interactable = value;
                }

                m_BonusText.enabled = value;
                m_BonusImage.enabled = value;
                m_BonusAmount.enabled = value;
            }
        }
    }
}