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
        [SerializeField] private AudioClip[] m_Musics;
        [SerializeField] private AudioClip[] m_Sounds;

        public AudioClip this[Music m] => m_Musics[(int)m];
        public AudioClip this[Sound s] => m_Sounds[(int)s];

#if UNITY_EDITOR

        [CustomEditor(typeof(AudioProperties))]
        public class SoundsInspector : Editor
        {
            private static readonly int musicCount = Enum.GetValues(typeof(Music)).Length;
            private static readonly int soundCount = Enum.GetValues(typeof(Sound)).Length;

            private new AudioProperties target => base.target as AudioProperties;

            public override void OnInspectorGUI()
            {
                serializedObject.Update();

                EditorGUILayout.LabelField("Музыка", EditorStyles.boldLabel); // Заголовок для музыки

                if (target.m_Musics.Length < musicCount)
                {
                    Array.Resize(ref target.m_Musics, musicCount);
                }

                for (int i = 0; i < target.m_Musics.Length; i++)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    EditorGUI.indentLevel++;

                    target.m_Musics[i] = EditorGUILayout.ObjectField($"{(Music)i}", target.m_Musics[i], typeof(AudioClip), false) as AudioClip;

                    EditorGUI.indentLevel--;

                    EditorGUILayout.EndVertical();

                    serializedObject.ApplyModifiedProperties();
                }

                EditorGUILayout.Space();
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Звуки", EditorStyles.boldLabel); // Заголовок для звука
                
                if (target.m_Sounds.Length < soundCount)
                {
                    Array.Resize(ref target.m_Sounds, soundCount);
                }

                for (int i = 0; i < target.m_Sounds.Length; i++)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    EditorGUI.indentLevel++;

                    target.m_Sounds[i] = EditorGUILayout.ObjectField($"{(Sound)i}", target.m_Sounds[i], typeof(AudioClip), false) as AudioClip;

                    EditorGUI.indentLevel--;

                    EditorGUILayout.EndVertical();

                    serializedObject.ApplyModifiedProperties();
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