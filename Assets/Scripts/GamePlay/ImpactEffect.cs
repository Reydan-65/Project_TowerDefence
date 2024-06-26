using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class ImpactEffect : ImpactEffectBase
    {
        [Header("Destroyed Object Prefab")]
        [SerializeField] private GameObject m_ObjectPrefab;

        /*
        private void Awake()
        {
            if (m_ObjectPrefab.TryGetComponent(out SmallPlasmaProjectile object0) == true)
                PlaySoundByIndex(0);

            if (m_ObjectPrefab.TryGetComponent(out PlasmaCannonProjectile object1) == true)
                PlaySoundByIndex(1);

            if (m_ObjectPrefab.TryGetComponent(out Missle object2_1) == true ||
                m_ObjectPrefab.TryGetComponent(out Mine object2_2) == true)
                PlaySoundByIndex(2);

            if (m_ObjectPrefab.TryGetComponent(out EMPBomb object3) == true)
                PlaySoundByIndex(3);
        }

        private void PlaySoundByIndex(int index)
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.AudioProperties.ProjectileEndClips, index,
                         transform.GetComponent<AudioSource>(), SoundManager.Instance.AudioProperties.SoundsVolume);
        }
    */
    }
}