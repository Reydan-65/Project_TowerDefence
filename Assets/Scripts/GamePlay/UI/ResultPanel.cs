using UnityEngine;
using TMPro;
using Common;

namespace SpaceShooter
{
    public class ResultPanel : MonoBehaviour
    {
        private const string PassedText = "Passed";
        private const string LostText = "Lose";
        private const string NextText = "Next";
        private const string LevelDoneText = "Done";
        private const string RestartText = "Restart";
        private const string MainMenuText = "Main Menu";
        private const string KillsPrefix = "Kills: ";
        private const string ScoresPrefix = "Score: ";
        private const string TimePrefix = "Time: ";

        [SerializeField] private TextMeshProUGUI m_Kills;
        [SerializeField] private TextMeshProUGUI m_Score;
        [SerializeField] private TextMeshProUGUI m_Time;
        [SerializeField] private TextMeshProUGUI m_Result;
        [SerializeField] private TextMeshProUGUI m_Button;

        private bool m_LevelPassed = false;
        private bool AudioClipHasPlayed = false;

        private void Start()
        {
            AudioClipHasPlayed = false;

            gameObject.SetActive(false);

            LevelController.Instance.LevelLost += OnLevelLost;
            LevelController.Instance.LevelPassed += OnLevelPassed;
        }

        private void OnDestroy()
        {
            LevelController.Instance.LevelLost -= OnLevelLost;
            LevelController.Instance.LevelPassed -= OnLevelPassed;
        }

        private void OnLevelPassed()
        {
            gameObject.SetActive(true);

            m_LevelPassed = true;

            //PlaySoundByIndexOneTime(2);

            //FillLevelStatistics();

            m_Result.text = PassedText;

            if (LevelSequencesController.Instance.CurrentLevelIsLast() == true)
            {
                m_Button.text = MainMenuText;
            }
            else
            {
                /*m_Button.text = NextText;*/
                m_Button.text = LevelDoneText;
            }
        }

        private void OnLevelLost()
        {
            gameObject.SetActive(true);

            //PlaySoundByIndexOneTime(3);

            //FillLevelStatistics();

            m_Result.text = LostText;
            m_Button.text = RestartText;
        }

        private void FillLevelStatistics()
        {
            m_Kills.text = KillsPrefix + Player.Instance.NumKills.ToString();
            m_Score.text = ScoresPrefix + Player.Instance.Score.ToString();
            m_Time.text = TimePrefix + LevelController.Instance.LevelTime.ToString("F0");
        }

        public void EX_OnButtonAction()
        {
            gameObject.SetActive(false);

            AudioClipHasPlayed = false;

            if (m_LevelPassed == true)
            {
                LevelController.Instance.LoadNextLevel();
            }
            else
            {
                LevelController.Instance.RestartLevel();
            }
        }

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
    }
}