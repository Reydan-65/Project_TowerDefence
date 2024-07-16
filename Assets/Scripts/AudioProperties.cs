using UnityEngine;

namespace SpaceShooter
{
    [CreateAssetMenu]
    public sealed class AudioProperties : ScriptableObject
    {
        [Header("Music")]
        [SerializeField] private AudioClip[] m_SoundtrackClips;

        [Header("Menu SFX")]
        [SerializeField] private AudioClip[] m_ClickClips;

        [Header("Projectile Launch SFX")]
        [SerializeField] private AudioClip[] m_ProjectileLaunchClips;

        [Header("Projectile LifeEnd SFX")]
        [SerializeField] private AudioClip[] m_ProjectileEndClips;

        [Header("Other Sounds")]
        [SerializeField] private AudioClip[] m_OtherClips;

        [Space(10)]
        [Range(0f, 1f)]
        [SerializeField] private float m_MusicVolume;
        [Range(0f, 1f)]
        [SerializeField] private float m_SoundsVolume;

        public AudioClip[] SoundtrackClips => m_SoundtrackClips;
        public AudioClip[] ClickClips => m_ClickClips;
        public AudioClip[] ProjectileLaunchClips => m_ProjectileLaunchClips;
        public AudioClip[] ProjectileEndClips => m_ProjectileEndClips;
        public AudioClip[] OtherClips => m_OtherClips;

        public float MusicVolume { get => m_MusicVolume; set => m_MusicVolume = value; }
        public float SoundsVolume { get => m_SoundsVolume; set => m_SoundsVolume = value; }
    }
}