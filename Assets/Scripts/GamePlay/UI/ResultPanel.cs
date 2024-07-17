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
        private const string NextText = "Next";
        private const string LevelDoneText = "Continue";
        private const string RestartText = "Restart";
        private const string BackToLevelMapText = "Back";
        private const string MainMenuText = "Main Menu";
        private const string KillsPrefix = "Kills: ";
        private const string ScoresPrefix = "Score: ";
        private const string TimePrefix = "Time: ";

        /*
        [SerializeField] private TextMeshProUGUI m_Kills;
        [SerializeField] private TextMeshProUGUI m_Score;
        [SerializeField] private TextMeshProUGUI m_Time;
        */

        [SerializeField] private TextMeshProUGUI m_Result;

        [SerializeField] private Image m_PassedImage;
        [SerializeField] private Image m_NotPassedImage;
        [SerializeField] private TextMeshProUGUI m_LevelTimeText;
        [SerializeField] private TextMeshProUGUI m_LivesLeftText;
        //[SerializeField] private TextMeshProUGUI m_Button;

        private bool m_LevelPassed = false;
        private LevelCondition m_LevelCondition;
        //private bool AudioClipHasPlayed = false;

        private int m_PlayerHealthAtStartLevel;

        private void Start()
        {
            //AudioClipHasPlayed = false;
            m_PlayerHealthAtStartLevel = TD_Player.Instance.CurrentNumLives;
            m_LevelCondition = FindObjectOfType<LevelCondition>();
            gameObject.SetActive(false);
            m_PassedImage.gameObject.SetActive(false);
            m_NotPassedImage.gameObject.SetActive(false)
                ;
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
            gameObject.SetActive(true);

            m_LevelPassed = true;

            //PlaySoundByIndexOneTime(2);

            FillLevelStatistics();

            m_Result.text = PassedText;

            //if (LevelSequencesController.Instance.CurrentLevelIsLast() == true)
            //{
            //    m_Button.text = MainMenuText;
            //}
            //else
            //{
            //    m_Button.text = NextText;
            //}
        }

        private void OnLevelLost()
        {
            gameObject.SetActive(true);

            //PlaySoundByIndexOneTime(3);

            FillLevelStatistics();

            m_Result.text = LostText;
            //m_Button.text = RestartText;
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

            //m_Kills.text = KillsPrefix + Player.Instance.NumKills.ToString();
            //m_Score.text = ScoresPrefix + Player.Instance.Score.ToString();
            //m_Time.text = TimePrefix + LevelController.Instance.LevelTime.ToString("F0");
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

        public void EX_OnButtonAction()
        {
            gameObject.SetActive(false);

            TDButton.PlayClickSound();
            //AudioClipHasPlayed = false;

            if (m_LevelPassed == true)
            {
                LevelController.Instance.LoadNextLevel();
            }
            else
            {
                LevelController.Instance.RestartLevel();
            }
        }

        public void EX_ReturnLevelMap()
        {
            TDButton.PlayClickSound();
            LevelController.Instance.ReturnLevelMap();
        }

        /*
        // Выключить фоновый звук, и включить звук события
        private void PlaySoundByIndexOneTime(int index)
        {
            if (AudioClipHasPlayed == false)
            {
                SoundManager.Instance.Stop(LevelController.Instance.AudioSource);

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.AudioProperties.SoundtrackClips, index,
                             LevelController.Instance.AudioSource, SoundManager.Instance.AudioProperties.MusicVolume);

                AudioClipHasPlayed = true;
            }
        }
        */
    }
}