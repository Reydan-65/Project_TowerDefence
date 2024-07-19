using SpaceShooter;
using System;
using UnityEngine;

namespace TowerDefence
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : SingletonBase<SoundPlayer>
    {
        [Range(0f, 1f)]
        [SerializeField] private float m_MusicVolume;
        [Range(0f, 1f)]
        [SerializeField] private float m_SoundsVolume;

        [Space(10)]
        [SerializeField] private AudioProperties m_Sounds;
        [SerializeField] private AudioClip m_BGM;

        private AudioSource m_AudioSource;
        public AudioSource AudioSource { get => m_AudioSource; set => m_AudioSource = value; }

        public float MusicVolume { get => m_MusicVolume; set => m_MusicVolume = value; }
        public float SoundsVolume { get => m_SoundsVolume; set => m_SoundsVolume = value; }

        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
            m_AudioSource.volume = m_SoundsVolume;

            Instance.m_AudioSource.clip = m_BGM;
            Instance.m_AudioSource.Play();
        }

        public void Play(Sound sound, float volume)
        {
            m_AudioSource.PlayOneShot(m_Sounds[sound]);
            m_AudioSource.volume = volume;
        }

        public void Stop()
        {
            m_AudioSource.Stop();
        }
    }
}