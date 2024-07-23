using UnityEngine;
using TMPro;
using TowerDefence;
using UnityEngine.UI;
using System;

namespace SpaceShooter
{
    public class ResultPanel : MonoBehaviour
    {
        private const string PassedText = "You Won!";
        private const string LostText = "You Lose!";

        [SerializeField] private TextMeshProUGUI m_Result;
        [SerializeField] private Image m_PassedImage;
        [SerializeField] private Image m_NotPassedImage;
        [SerializeField] private TextMeshProUGUI m_LevelTimeText;
        [SerializeField] private TextMeshProUGUI m_LivesLeftText;

        private bool m_LevelPassed = false;

        private int m_PlayerHealthAtStartLevel;
        private bool AudioClipHasPlayed;

        private void Start()
        {
            AudioClipHasPlayed = false;
            m_PlayerHealthAtStartLevel = TD_Player.Instance.CurrentNumLives;

            gameObject.SetActive(false);
            m_PassedImage.gameObject.SetActive(false);
            m_NotPassedImage.gameObject.SetActive(false);

            LevelController.Instance.LevelLost += OnLevelLost;
            LevelController.Instance.LevelPassed += OnLevelPassed;
        }

        private void OnDestroy()
        {
            if (LevelController.Instance != null)
            {
                LevelController.Instance.LevelLost -= OnLevelLost;
                LevelController.Instance.LevelPassed -= OnLevelPassed;
            }
        }

        private void OnLevelPassed()
        {
            PlayResultTrack(Sound.Win);
            
            gameObject.SetActive(true);
            m_LevelPassed = true;
            FillLevelStatistics();
            m_Result.text = PassedText;
        }

        private void OnLevelLost()
        {
            PlayResultTrack(Sound.Lose);

            gameObject.SetActive(true);
            FillLevelStatistics();
            m_Result.text = LostText;
        }

        private void FillLevelStatistics()
        {
            if (m_LevelPassed == true)
            {
                m_PassedImage.gameObject.SetActive(true);
                CheckTimeResult(m_LevelTimeText, (int)LevelController.Instance.LevelTime, (int)LevelController.Instance.StartReferenceTime);
                CheckLivesResult(m_LivesLeftText, TD_Player.Instance.CurrentNumLives, m_PlayerHealthAtStartLevel);
            }
            else
            {
                m_NotPassedImage.gameObject.SetActive(true);
                m_LevelTimeText.transform.parent.GetChild(2).gameObject.SetActive(true);
                m_LivesLeftText.transform.parent.GetChild(2).gameObject.SetActive(true);

                m_LevelTimeText.gameObject.SetActive(false);
                m_LivesLeftText.gameObject.SetActive(false);
            }
        }

        private void CheckResult(TextMeshProUGUI text, int resultText, int needText, Func<int, int, bool> comparison)
        {
            if (comparison(resultText, needText))
                text.color = Color.red;
            else
            {
                text.transform.parent.GetChild(1).gameObject.SetActive(true);
                text.transform.parent.GetChild(0).gameObject.SetActive(false);
            }

            text.text = $"{resultText}/{needText}";
        }

        private void CheckTimeResult(TextMeshProUGUI text, int resultText, int needText)
        {
            CheckResult(text, resultText, needText, (resultText, needText) => resultText > needText);
        }

        private void CheckLivesResult(TextMeshProUGUI text, int resultText, int needText)
        {
            CheckResult(text, resultText, needText, (resultText, needText) => resultText < needText);
        }

        /*
        public void EX_OnButtonAction()
        {
            Sound.Click.Play();

            gameObject.SetActive(false);

            if (m_LevelPassed == true)
                LevelController.Instance.LoadNextLevel();
            else
                LevelController.Instance.RestartLevel();
        }
        */

        public void EX_ReturnLevelMap()
        {
            Sound.Click.PlaySound();

            LevelController.Instance.ReturnLevelMap();
        }

        // Выключить фоновый звук, и включить звук события
        private void PlayResultTrack(Sound sound)
        {
            if (AudioClipHasPlayed == false)
            {
                SoundPlayer.Instance.Stop();
                sound.PlaySound();

                AudioClipHasPlayed = true;
            }
        }
    }
}