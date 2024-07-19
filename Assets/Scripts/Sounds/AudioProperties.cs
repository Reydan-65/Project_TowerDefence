using TowerDefence;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceShooter
{
    [CreateAssetMenu]
    public sealed class AudioProperties : ScriptableObject
    {
        [SerializeField] private AudioClip[] m_Sounds;
        public AudioClip this[Sound s] => m_Sounds[(int) s];
        /*
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
        
        public AudioClip[] SoundtrackClips => m_SoundtrackClips;
        public AudioClip this[Sound s] => m_SoundtrackClips[(int)s];

        public AudioClip[] ClickClips => m_ClickClips;
        public AudioClip[] ProjectileLaunchClips => m_ProjectileLaunchClips;
        public AudioClip[] ProjectileEndClips => m_ProjectileEndClips;
        public AudioClip[] OtherClips => m_OtherClips;
        */

        //[Space(10)]
        //[Range(0f, 1f)]
        //[SerializeField] private float m_MusicVolume;
        //[Range(0f, 1f)]
        //[SerializeField] private float m_SoundsVolume;
        //public float MusicVolume { get => m_MusicVolume; set => m_MusicVolume = value; }
        //public float SoundsVolume { get => m_SoundsVolume; set => m_SoundsVolume = value; }

#if UNITY_EDITOR

        [CustomEditor(typeof(AudioProperties))]
        public class SoundsInspector : Editor
        {
            private static readonly int soundCount = Enum.GetValues(typeof(Sound)).Length;
            private new AudioProperties target => base.target as AudioProperties;

            /// <summary>
            /// $"{(Sound)i}" - текстовая метка;
            /// target.m_Sounds[i] - отображаемый объект;
            /// typeof(AudioClip) - тип отображаемого объекта;
            /// false - объект не является объектом на сцене.
            /// </summary>
            public override void OnInspectorGUI()
            {
                if ( target.m_Sounds.Length < soundCount)
                {
                    Array.Resize(ref target.m_Sounds, soundCount);
                }

                for (int i = 0; i < target.m_Sounds.Length; i++)
                {
                    target.m_Sounds[i] = EditorGUILayout.ObjectField($"{(Sound)i}", target.m_Sounds[i], typeof(AudioClip), false) as AudioClip;
                }
            }
        }
#endif

    }
}

/*
 * using TowerDefence;
using UnityEngine;

namespace SpaceShooter
{
    public enum Sound
    {
        Soundtrack,
        Click,
        ProjectileLaunch,
        ProjectileEnd,
        Other
    }

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

        public AudioClip GetSoundtrackClip(int index = 0)
        {
            if (m_SoundtrackClips != null && m_SoundtrackClips.Length > index)
            {
                return m_SoundtrackClips[index];
            }
            return null;
        }

        public AudioClip GetClickClip(int index = 0)
        {
            if (m_ClickClips != null && m_ClickClips.Length > index)
            {
                return m_ClickClips[index];
            }
            return null;
        }

        public AudioClip GetProjectileLaunchClip(int index = 0)
        {
            if (m_ProjectileLaunchClips != null && m_ProjectileLaunchClips.Length > index)
            {
                return m_ProjectileLaunchClips[index];
            }
            return null;
        }

        public AudioClip GetProjectileEndClip(int index = 0)
        {
            if (m_ProjectileEndClips != null && m_ProjectileEndClips.Length > index)
            {
                return m_ProjectileEndClips[index];
            }
            return null;
        }

        public AudioClip GetOtherClip(int index = 0)
        {
            if (m_OtherClips != null && m_OtherClips.Length > index)
            {
                return m_OtherClips[index];
            }
            return null;
        }

        public float MusicVolume { get => m_MusicVolume; set => m_MusicVolume = value; }
        public float SoundsVolume { get => m_SoundsVolume; set => m_SoundsVolume = value; }
    }
}
*/