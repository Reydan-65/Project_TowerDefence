using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using SpaceShooter;

namespace TowerDefence
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : SingletonBase<SoundPlayer>
    {
        public const string filename = "sound.dat";

        [Range(0f, 1f)]
        [SerializeField] private float m_SoundsVolume;

        [Space(10)]
        [SerializeField] private AudioProperties m_Musics;
        [SerializeField] private AudioProperties m_Sounds;

        private Settings m_Settings;
        private AudioSource m_AudioSource;

        public AudioSource AudioSource { get => m_AudioSource; set => m_AudioSource = value; }
        public float SoundsVolume { get => m_SoundsVolume; set => m_SoundsVolume = value; }

        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
            m_Settings = FindObjectOfType<Settings>();

            EX_LoadSettings();

            SceneManager.sceneLoaded += OnSceneLoaded;

            if (m_Settings != null)
                m_Settings.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (m_Settings != null)
            {
                m_SoundsVolume = m_Settings.Slider.value;
                m_AudioSource.volume = m_SoundsVolume;
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            string sceneName = scene.name;

            switch (sceneName)
            {
                case "mainmenu":
                    Music.Menu.PlayMusic();
                    break;
                case "levelMap":
                    Music.LevelMap.PlayMusic();
                    break;
                case "level_01":
                    Music.Level_01.PlayMusic();
                    break;
                case "level_02":
                    Music.Level_02.PlayMusic();
                    break;
                case "level_03":
                    Music.Level_03.PlayMusic();
                    break;
                case "level_04":
                    Music.Level_04.PlayMusic();
                    break;
                case "level_01_2":
                    Music.Level_01_2.PlayMusic();
                    break;
                case "level_02_2":
                    Music.Level_02_2.PlayMusic();
                    break;
                default:
                    Debug.LogWarning($"Unhandled scene name: {sceneName}");
                    break;
            }
        }

        public void PlayMusic(Music music, float volume)
        {
            m_AudioSource.PlayOneShot(m_Musics[music]);
            m_AudioSource.volume = volume;
        }

        public void PlaySound(Sound sound, float volume)
        {
            m_AudioSource.PlayOneShot(m_Sounds[sound]);
            m_AudioSource.volume = volume;
        }

        public void Stop()
        {
            m_AudioSource.Stop();
        }

        public void EX_LoadSettings()
        {
            Saver<float>.TryLoad(filename, ref m_SoundsVolume);

            if (m_Settings != null)
                m_Settings.Slider.value = m_SoundsVolume;

            m_AudioSource.volume = m_SoundsVolume;
        }

        public void EX_AcceptChanges()
        {
            Sound.Click.PlaySound();

            Saver<float>.Save(filename, Instance.m_SoundsVolume);
            m_AudioSource.volume = m_SoundsVolume;
            m_Settings.gameObject.SetActive(false);
        }

        public void EX_CancelChanges()
        {
            Sound.Click.PlaySound();

            EX_LoadSettings();
            m_AudioSource.volume = m_SoundsVolume;
            m_Settings.gameObject.SetActive(false);
        }
    }
}