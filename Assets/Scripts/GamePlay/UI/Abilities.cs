using SpaceShooter;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefence
{
    public class Abilities : SingletonBase<Abilities>
    {
        [Serializable]
        public class Ability
        {
            [SerializeField] protected int m_Cost;
            [SerializeField] protected float m_Cooldown;

            [Space(10)]
            [SerializeField] protected string m_RequaredName;
            [SerializeField] protected int m_RequaredLevelUpgrade;
            public int Cost => m_Cost;
            public float Cooldown => m_Cooldown;
            public string RequaredName => m_RequaredName;
            public int RequaredLevelUpgrade => m_RequaredLevelUpgrade;

            //public virtual void ImageUpdate(Button button, float startTime, float time, TextMeshProUGUI text, int value)
            //{
            //    if (button != null)
            //    {
            //        button.GetComponent<Image>().fillAmount += (Time.deltaTime / startTime) * value;

            //        if (time <= 0) time = 0;
            //        text.text = Mathf.Ceil(time).ToString();
            //    }
            //}
        }

        [Serializable]
        public class ExplosionAbility : Ability
        {
            [Space(10)]
            [SerializeField] private float m_Radius;
            [SerializeField] private int m_Damage;
            [SerializeField] private Color m_TargetingColor;

            private int m_BaseDamage;
            public int Damage { get => m_Damage; set => m_Damage = value; }
            public int BaseDamage { get => m_BaseDamage; set => m_BaseDamage = value; }

            /// <summary>
            /// ѕрименение способности игрока.
            /// - указываем точку применени€;
            /// - ищем противников вокруг этой точки (в заданном радиусе);
            /// - каждому найденному противнику наноситс€ заданный урон;
            /// - кнопка активации отключаетс€ на врем€ перезар€дки;
            /// - визуал дл€ перезар€дки.
            /// </summary>
            public void Use()
            {
                Instance.m_TargetingCircle.gameObject.SetActive(true);
                Instance.m_TargetingCircle.GetComponent<RectTransform>().sizeDelta = new Vector2(m_Radius * 2 * 100, m_Radius * 2 * 100);

                ClickProtection.Instance.Activate((Vector2 clickPosition) =>
                {
                    Instance.m_ExplosionAbilityCooldownImage.color = Color.red;
                    Instance.m_ExplosionAbilityTimerText.enabled = true;
                    Instance.m_ExplosionAbilityCooldownTime = m_Cooldown;

                    Vector3 position = Camera.main.ScreenToWorldPoint(clickPosition);
                    position.z = 0;

                    GameObject explosion = Instantiate(Instance.m_ExplosionAbilityPrefab, position, Quaternion.identity);

                    foreach (var collider2D in Physics2D.OverlapCircleAll(position, m_Radius))
                    {
                        if (collider2D.transform.root.TryGetComponent(out Enemy enemy))
                            enemy.TakeDamage(m_Damage, Projectile.DamageType.Magic);
                    }

                    Destroy(explosion, 0.7f);

                    Instance.m_TargetingCircle.gameObject.SetActive(false);
                    Instance.m_ExplosionAbilityButton.interactable = false;
                    Instance.m_ExplosionAbilityCooldownImage.fillAmount = 0;

                    TD_Player.Instance.ChangeEnergy(-m_Cost);
                    Instance.StartCoroutine(OnCooldownEnd());
                });

                IEnumerator OnCooldownEnd()
                {
                    yield return new WaitForSeconds(m_Cooldown);
                    Instance.m_ExplosionAbilityCooldownImage.color = Color.green;
                    Instance.m_ExplosionAbilityButton.interactable = true;
                    Instance.m_ExplosionAbilityTimerText.enabled = false;
                }
            }

            //public override void ImageUpdate(Button button, float startTime, float time, TextMeshProUGUI text, int value)
            //{
            //    base.ImageUpdate(button, startTime, time, text, value);
            //    time = startTime;
            //}
        }

        [Serializable]
        public class SlowEnemyAbility : Ability
        {
            [Space(10)]
            [SerializeField] private float m_Duration;
            private float m_BaseDuration;
            [SerializeField] private float m_ValueToChangeVelocity;
            public float Duration { get => m_Duration; set => m_Duration = value; }
            public float BaseDuration { get => m_BaseDuration; set => m_BaseDuration = value; }
            public float ValueToChangeVelocity { get => m_ValueToChangeVelocity; set => m_ValueToChangeVelocity = value; }

            /// <summary>
            /// ѕрименение способности игрока.
            /// - все противники на сцене, и те которые будут созданы в период действи€ способности, замедл€ютс€.
            /// - когда врем€ действи€ заканчиваетс€ - их скорость восстанавливаетс€.
            /// - кнопка активации отключаетс€ на врем€ перезар€дки;
            /// - визуал дл€ перезар€дкию
            /// </summary>
            public void Use()
            {
                Instance.m_SlowEnemyAbilityDurationTime = m_Duration;
                Instance.m_SlowEnemyAbilityButton.interactable = false;

                void Slow(Enemy enemy)
                {
                    enemy.GetComponent<SpaceShip>().ChangeMaxLinearVelocityOnValue(m_ValueToChangeVelocity);
                }

                foreach (var enemy in FindObjectsOfType<Enemy>())
                {
                    Slow(enemy);
                }
                TD_Player.Instance.ChangeEnergy(-m_Cost);

                EnemyWaveManager.OnEnemySpawn += Slow;

                Instance.StartCoroutine(Restore());

                IEnumerator Restore()
                {
                    Instance.m_SlowEnemyAbilityCooldownImage.color = Color.white;

                    yield return new WaitForSeconds(m_Duration);

                    foreach (var enemy in FindObjectsOfType<Enemy>())
                    {
                        enemy.GetComponent<SpaceShip>().RestoreMaxLinearVelocityOnValue();
                    }

                    EnemyWaveManager.OnEnemySpawn -= Slow;

                    Instance.StartCoroutine(TimeAbilityCooldown());
                }

                IEnumerator TimeAbilityCooldown()
                {
                    Instance.m_SlowEnemyAbilityCooldownImage.color = Color.red;
                    Instance.m_SlowEnemyAbilityTimerText.enabled = true;
                    Instance.m_SlowEnemyAbilityCooldownTime = m_Cooldown;

                    Instance.m_SlowEnemyAbilityCooldownImage.fillAmount = 0;

                    Instance.m_SlowEnemyAbilityButton.interactable = false;

                    yield return new WaitForSeconds(m_Cooldown);

                    Instance.m_SlowEnemyAbilityCooldownImage.color = Color.green;
                    Instance.m_SlowEnemyAbilityButton.interactable = true;
                    Instance.m_SlowEnemyAbilityTimerText.enabled = false;
                }
            }

            //public override void ImageUpdate(Button button, float startTime, float time, TextMeshProUGUI text, int value)
            //{
            //    base.ImageUpdate(button, startTime, time, text, value);
            //    time = startTime;
            //}
        }

        [Header("FireAbility")]
        [SerializeField] private ExplosionAbility m_ExplosionAbility;
        [SerializeField] private Button m_ExplosionAbilityButton;
        [SerializeField] private Image m_ExplosionAbilityCooldownImage;
        [SerializeField] private Image m_ExplosionAbilityImage;
        [SerializeField] private Image m_ExplosionAbilityLockerImage;
        [SerializeField] private TextMeshProUGUI m_ExplosionAbilityTimerText;
        [SerializeField] private TextMeshProUGUI m_ExplosionAbilityCostText;
        [SerializeField] private Image m_TargetingCircle;
        [SerializeField] private GameObject m_ExplosionAbilityPrefab;

        [Header("TimeAbility")]
        [SerializeField] private SlowEnemyAbility m_SlowEnemyAbility;
        [SerializeField] private Button m_SlowEnemyAbilityButton;
        [SerializeField] private Image m_SlowEnemyAbilityCooldownImage;
        [SerializeField] private Image m_SlowEnemyAbilityImage;
        [SerializeField] private Image m_SlowEnemyAbilityLockerImage;
        [SerializeField] private TextMeshProUGUI m_SlowEnemyAbilityTimerText;
        [SerializeField] private TextMeshProUGUI m_SlowEnemyAbilityCostText;

        private float m_ExplosionAbilityCooldownTime;
        private float m_SlowEnemyAbilityCooldownTime;
        private float m_SlowEnemyAbilityDurationTime;

        public void EX_UseFireAbility() => m_ExplosionAbility.Use();
        public void EX_UseTimeAbility() => m_SlowEnemyAbility.Use();

        public bool IsAvailable(Ability ability) =>
            ability.RequaredName != null && ability.RequaredLevelUpgrade <= Upgrades.GetUpgradeLevel(ability.RequaredName);

        private void Start()
        {
            m_ExplosionAbility.BaseDamage = m_ExplosionAbility.Damage;
            m_SlowEnemyAbility.BaseDuration = m_SlowEnemyAbility.Duration;

            m_ExplosionAbilityTimerText.enabled = false;
            m_SlowEnemyAbilityTimerText.enabled = false;

            m_ExplosionAbilityCostText.text = m_ExplosionAbility.Cost.ToString();
            m_SlowEnemyAbilityCostText.text = m_SlowEnemyAbility.Cost.ToString();

            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);

            if (IsAvailable(m_ExplosionAbility))
            {
                transform.GetChild(0).gameObject.SetActive(true);

                if (Upgrades.GetUpgradeLevel(m_ExplosionAbility.RequaredName) > 1)
                    m_ExplosionAbility.Damage += (int)(m_ExplosionAbility.BaseDamage * 0.5f * (Upgrades.GetUpgradeLevel(m_ExplosionAbility.RequaredName) - 1));
            }

            if (IsAvailable(m_SlowEnemyAbility))
            {
                transform.GetChild(1).gameObject.SetActive(true);

                if (Upgrades.GetUpgradeLevel(m_SlowEnemyAbility.RequaredName) > 1)
                {
                    m_SlowEnemyAbility.ValueToChangeVelocity -= 0.05f * Upgrades.GetUpgradeLevel(m_SlowEnemyAbility.RequaredName);
                    m_SlowEnemyAbility.Duration += (float)(m_SlowEnemyAbility.BaseDuration * 0.25f);
                }
            }
        }

        private void Update()
        {
            UpdateButtonState(m_ExplosionAbilityButton, m_ExplosionAbility.Cooldown, ref m_ExplosionAbilityCooldownTime, m_ExplosionAbilityTimerText, 1, m_ExplosionAbility.Cost, m_ExplosionAbilityLockerImage);

            if (m_SlowEnemyAbilityDurationTime > 0 && m_SlowEnemyAbilityCooldownTime ==0)
                UpdateButtonState(m_SlowEnemyAbilityButton, m_SlowEnemyAbility.Duration, ref m_SlowEnemyAbilityDurationTime, m_SlowEnemyAbilityTimerText, -1, m_SlowEnemyAbility.Cost);
            else
                UpdateButtonState(m_SlowEnemyAbilityButton, m_SlowEnemyAbility.Cooldown, ref m_SlowEnemyAbilityCooldownTime, m_SlowEnemyAbilityTimerText, 1, m_SlowEnemyAbility.Cost, m_SlowEnemyAbilityLockerImage);


            //if (m_ExplosionAbilityButton != null)
            //{
            //    CheckEnergyEnough(m_ExplosionAbilityButton, m_ExplosionAbility.Cost, m_ExplosionAbilityLockerImage);

            //    m_ExplosionAbilityCooldownTime -= Time.deltaTime;

            //    m_ExplosionAbility.ImageUpdate(m_ExplosionAbilityButton, m_ExplosionAbility.Cooldown, m_ExplosionAbilityCooldownTime, m_ExplosionAbilityTimerText, 1);
            //}

            //if (m_SlowEnemyAbilityButton != null)
            //{
            //    CheckEnergyEnough(m_SlowEnemyAbilityButton, m_SlowEnemyAbility.Cost,m_SlowEnemyAbilityLockerImage);

            //    if (m_SlowEnemyAbilityDurationTime > 0)
            //    {
            //        m_SlowEnemyAbilityDurationTime -= Time.deltaTime;
            //        m_SlowEnemyAbility.ImageUpdate(m_SlowEnemyAbilityButton, m_SlowEnemyAbility.Duration, m_SlowEnemyAbilityDurationTime, m_SlowEnemyAbilityTimerText, -1);
            //    }

            //    if (m_SlowEnemyAbilityCooldownTime > 0)
            //    {
            //        m_SlowEnemyAbilityCooldownTime -= Time.deltaTime;
            //        m_SlowEnemyAbility.ImageUpdate(m_SlowEnemyAbilityButton, m_SlowEnemyAbility.Cooldown, m_SlowEnemyAbilityCooldownTime, m_SlowEnemyAbilityTimerText, 1);
            //    }
            //}

            if (Instance.m_TargetingCircle.gameObject.activeSelf)
            {
                Vector3 mousePosition = Input.mousePosition;
                Instance.m_TargetingCircle.transform.position = mousePosition;
            }

        }

        //private void CheckEnergyEnough(Button button, int costEnergy, Image lockerImage)
        //{
        //    if (TD_Player.Instance.CurrentNumEnergy >= costEnergy)
        //    {
        //        lockerImage.enabled = false;
        //        button.interactable = true;
        //    }
        //    else
        //    {
        //        lockerImage.enabled = true;
        //        button.interactable = false;
        //    }
        //}

        public virtual void UpdateButtonState(Button button, float startTime, ref float time, TextMeshProUGUI text, int value, int costEnergy, Image lockerImage = null)
        {
            // ќбновление заполнени€ кнопки
            if (button != null)
            {
                button.GetComponent<Image>().fillAmount += (Time.deltaTime / startTime) * value;

                // ќбновление текста таймера
                time -= Time.deltaTime; // ”меньшаем врем€
                if (time < 0) time = 0;
                text.text = Mathf.Ceil(time).ToString();

                // ѕроверка, достаточно ли энергии и закончилось ли врем€ восстановлени€
                if (lockerImage != null)
                {
                    if (time <= 0 && TD_Player.Instance.CurrentNumEnergy >= costEnergy)
                    {
                        lockerImage.enabled = false;
                        button.interactable = true;
                    }
                    else
                    {
                        lockerImage.enabled = true;
                        button.interactable = false;
                    }
                }
            }
        }
    }
}