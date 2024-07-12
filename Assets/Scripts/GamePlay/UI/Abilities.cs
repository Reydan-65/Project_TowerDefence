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
            [SerializeField] protected float m_Cost;
            [SerializeField] protected float m_Cooldown;
            public float Cost => m_Cost;
            public float Cooldown => m_Cooldown;

            public virtual void ImageUpdate(Button button, float startTime, float time, TextMeshProUGUI text, int value)
            {
                if (button != null)
                {
                    button.GetComponent<Image>().fillAmount += (Time.deltaTime / startTime) * value;

                    if (time <= 0) time = 0;
                    text.text = Mathf.Ceil(time).ToString();
                }
            }
        }

        [Serializable]
        public class FireAbility : Ability
        {
            [Space(10)]
            [SerializeField] private int m_Radius;
            [SerializeField] private int m_Damage;
            [SerializeField] private Color m_TargetingColor;

            /// <summary>
            /// Применение способности игрока.
            /// - указываем точку применения;
            /// - ищем противников вокруг этой точки (в заданном радиусе);
            /// - каждому найденному противнику наносится заданный урон;
            /// - кнопка активации отключается на время перезарядки;
            /// - визуал для перезарядкию
            /// </summary>
            public void Use()
            {
                Instance.m_TargetingCircle.gameObject.SetActive(true);

                ClickProtection.Instance.Activate((Vector2 clickPosition) =>
                {
                    Instance.m_FireAbilityTimerText.enabled = true;
                    Instance.m_FireAbilityCooldownTime = m_Cooldown;

                    Vector3 position = Camera.main.ScreenToWorldPoint(clickPosition);

                    foreach (var collider2D in Physics2D.OverlapCircleAll(position, m_Radius))
                    {
                        if (collider2D.transform.root.TryGetComponent(out Enemy enemy) == true)
                        {
                            enemy.TakeDamage(m_Damage, Projectile.DamageType.Magic);
                        }
                    }

                    Instance.m_TargetingCircle.gameObject.SetActive(false);
                    Instance.m_FireAbilityButton.interactable = false;
                    Instance.m_FireAbilityButtonImage.fillAmount = 0;

                    Instance.StartCoroutine(OnCooldownEnd());
                });

                IEnumerator OnCooldownEnd()
                {
                    yield return new WaitForSeconds(m_Cooldown);
                    Instance.m_FireAbilityButton.interactable = true;
                    Instance.m_FireAbilityTimerText.enabled = false;
                }
            }

            public override void ImageUpdate(Button button, float startTime, float time, TextMeshProUGUI text, int value)
            {
                base.ImageUpdate(button, startTime, time, text, value);
                time = startTime;
            }
        }

        [Serializable]
        public class TimeAbility : Ability
        {
            [Space(10)]
            [SerializeField] private float m_Duration;
            public float Duration => m_Duration;

            /// <summary>
            /// Применение способности игрока.
            /// - все противники на сцене, и те которые будут созданы в период действия способности, замедляются.
            /// - когда время действия заканчивается - их скорость восстанавливается.
            /// - кнопка активации отключается на время перезарядки;
            /// - визуал для перезарядкию
            /// </summary>
            public void Use()
            {
                Instance.m_TimeAbilityDurationTime = m_Duration;

                void Slow(Enemy enemy)
                {
                    enemy.GetComponent<SpaceShip>().ChangeMaxLinearVelocityOnValue(0.5f);
                }

                foreach (var enemy in FindObjectsOfType<Enemy>())
                {
                    Slow(enemy);
                }

                EnemyWaveManager.OnEnemySpawn += Slow;

                Instance.StartCoroutine(Restore());

                IEnumerator Restore()
                {
                    Instance.m_TimeAbilityButtonImage.color = Color.cyan;

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
                    Instance.m_TimeAbilityTimerText.enabled = true;
                    Instance.m_TimeAbilityCooldownTime = m_Cooldown;

                    Instance.m_TimeAbilityButtonImage.color = Color.white;
                    Instance.m_TimeAbilityButtonImage.fillAmount = 0;

                    Instance.m_TimeAbilityButton.interactable = false;

                    yield return new WaitForSeconds(m_Cooldown);

                    Instance.m_TimeAbilityButton.interactable = true;
                    Instance.m_TimeAbilityTimerText.enabled = false;
                }
            }

            public override void ImageUpdate(Button button, float startTime, float time, TextMeshProUGUI text, int value)
            {
                base.ImageUpdate(button, startTime, time, text, value);
                time = startTime;
            }
        }

        [Header("FireAbility")]
        [SerializeField] private FireAbility m_FireAbility;
        [SerializeField] private Button m_FireAbilityButton;
        [SerializeField] private Image m_FireAbilityButtonImage;
        [SerializeField] private TextMeshProUGUI m_FireAbilityTimerText;
        [SerializeField] private Image m_TargetingCircle;

        [Header("TimeAbility")]
        [SerializeField] private TimeAbility m_TimeAbility;
        [SerializeField] private Button m_TimeAbilityButton;
        [SerializeField] private Image m_TimeAbilityButtonImage;
        [SerializeField] private TextMeshProUGUI m_TimeAbilityTimerText;

        private float m_FireAbilityCooldownTime;
        private float m_TimeAbilityCooldownTime;
        private float m_TimeAbilityDurationTime;

        public void EX_UseFireAbility() => m_FireAbility.Use();
        public void EX_UseTimeAbility() => m_TimeAbility.Use();

        private void Start()
        {
            m_FireAbilityTimerText.enabled = false;
            m_TimeAbilityTimerText.enabled = false;
        }

        private void Update()
        {
            if (m_FireAbilityButton != null)
            {
                m_FireAbilityCooldownTime -= Time.deltaTime;
                if (m_FireAbilityCooldownTime <= 0) m_FireAbilityCooldownTime = 0;
                m_FireAbility.ImageUpdate(m_FireAbilityButton, m_FireAbility.Cooldown, m_FireAbilityCooldownTime, m_FireAbilityTimerText, 1);
            }

            if (m_TimeAbilityButton != null)
            {
                if (m_TimeAbilityDurationTime > 0)
                {
                    m_TimeAbilityDurationTime -= Time.deltaTime;
                    m_TimeAbility.ImageUpdate(m_TimeAbilityButton, m_TimeAbility.Duration, m_TimeAbilityDurationTime, m_TimeAbilityTimerText, -1);
                }

                if (m_TimeAbilityCooldownTime > 0)
                {
                    m_TimeAbilityCooldownTime -= Time.deltaTime;
                    m_TimeAbility.ImageUpdate(m_TimeAbilityButton, m_TimeAbility.Cooldown, m_TimeAbilityCooldownTime, m_TimeAbilityTimerText, 1);
                }
            }
        }
    }
}