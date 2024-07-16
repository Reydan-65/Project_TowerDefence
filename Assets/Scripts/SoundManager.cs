using UnityEngine;
using SpaceShooter;

namespace Common
{
    public class SoundManager : SingletonBase<SoundManager>
    {
        public AudioProperties AudioProperties;

        private AudioSource m_AudioSource;

        private void Awake()
        {
            m_AudioSource = Camera.main.GetComponent<AudioSource>();
        }

        public void PlayOneShot(AudioClip[] audioClips, int soundID, float volume)
        {
            if (soundID >= 0 && soundID < audioClips.Length)
            {
                m_AudioSource.PlayOneShot(audioClips[soundID]);
                m_AudioSource.volume = volume;
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