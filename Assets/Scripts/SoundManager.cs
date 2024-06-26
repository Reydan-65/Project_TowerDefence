using UnityEngine;
using SpaceShooter;

namespace Common
{
    public class SoundManager : SingletonBase<SoundManager>
    {
        public AudioProperties AudioProperties;

        public void PlayOneShot(AudioClip[] audioClips, int soundID, AudioSource audioSource, float volume)
        {
            if (soundID >= 0 && soundID < audioClips.Length)
            {
                audioSource.PlayOneShot(audioClips[soundID]);
                audioSource.volume = volume;
            }
            else
            {
                Debug.LogError("Invalid sound index");
            }
        }

        public void Pause(AudioSource audioSource)
        {
            audioSource.Pause();
        }

        public void UnPause(AudioSource audioSource)
        {
            audioSource.UnPause();
        }

        public void Stop(AudioSource audioSource)
        {
            audioSource.Stop();
        }
    }
}