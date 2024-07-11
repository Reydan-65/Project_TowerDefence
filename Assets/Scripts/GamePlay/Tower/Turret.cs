using UnityEngine;
using Common;
using TowerDefence;

namespace SpaceShooter
{
    public class Turret : TurretBase
    {
        private Tower m_Tower;
        private float m_RefireTimer;

        public bool CanFire => m_RefireTimer <= 0;

        private void Start()
        {
            m_Tower = transform.root.GetComponent<Tower>();
        }

        protected override void FixedUpdate()
        {
            if (m_RefireTimer > 0)
                m_RefireTimer -= Time.deltaTime;
            else
                if (m_Mode == TurretMode.Auto) Fire();
        }

        public override void Fire()
        {
            if (m_TurretProperties == null) return;
            if (m_RefireTimer > 0) return;

            /*
            if (m_Ship != null)
            {
                // Трата ресурсов на выстрел
                if (m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false) return;
                if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false) return;
            }
            */

            // Создание снаряда при выстреле
            Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;

            // Получение данных о том кто выпустил снаряд, для измегания попаданий в самого себя
            projectile.SetParentShooter(m_Tower);

            /*
            if (m_Ship.TeamId == 0) projectile.Nickname = "Neutral Projectile";
            if (m_Ship.TeamId == 1) projectile.Nickname = "Frendly Projectile";
            if (m_Ship.TeamId == 2) projectile.Nickname = "Enemies Projectile";
            if (m_Ship.TeamId == 3) projectile.Nickname = "Debris Projectile";
            */

            m_RefireTimer = m_TurretProperties.RateOfFire;

            /// <summary>
            /// Звук выстрела.
            /// Звук слегка изменяется.
            /// </summary>
            /*
            //if (Mode == TurretMode.Primary && m_TurretProperties.ProjectilePrefab.GetComponent<SmallPlasmaProjectile>() == true)
            //    PlaySoundByIndex(0);
            //if (Mode == TurretMode.Primary && m_TurretProperties.ProjectilePrefab.GetComponent<PlasmaCannonProjectile>() == true)
            //    PlaySoundByIndex(1);
            //if (Mode == TurretMode.Secondary && m_TurretProperties.ProjectilePrefab.GetComponent<Missle>() == true)
            //    PlaySoundByIndex(2);
            //if (Mode == TurretMode.Secondary && m_TurretProperties.ProjectilePrefab.GetComponent<EMPBomb>() == true)
            //    PlaySoundByIndex(3);
            */
        }

        public override void AssingLoadout(TurretProperties properties)
        {
            if (m_Mode != properties.Mode) return;

            m_RefireTimer = 0;
            m_TurretProperties = properties;
        }

        /*
        private void PlaySoundByIndex(int index)
        {
            float pitch = Random.Range(0.95f, 1.05f);

            transform.root.GetComponent<AudioSource>().pitch = pitch;

            SoundManager.Instance.PlayOneShot(SoundManager.Instance.AudioProperties.ProjectileLaunchClips, index,
                         transform.root.GetComponent<AudioSource>(), SoundManager.Instance.AudioProperties.SoundsVolume);
        }
        */
    }
}