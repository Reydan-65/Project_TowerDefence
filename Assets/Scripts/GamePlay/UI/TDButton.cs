using Common;
using SpaceShooter;
using UnityEngine;

namespace TowerDefence
{
    public class TDButton : MonoBehaviour
    {
        public static void PlayClickSound()
        {
            SoundManager.Instance.PlayOneShot(
                SoundManager.Instance.AudioProperties.ClickClips, 0, SoundManager.Instance.AudioProperties.SoundsVolume);
        }
    }
}