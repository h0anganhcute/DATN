using System;
using System.Collections;
using Unity.FPS.Game;
using UnityEngine;

namespace TheDeveloperTrain.SciFiGuns
{
    public class Gun : MonoBehaviour
    {
        public ParticleSystem[] gunParticleSystems;

        [Header("Laser Settings")]
        [SerializeField] private Transform muzzleTransform;
        [SerializeField] private LineRenderer laserLine;
        [SerializeField] private float laserDistance = 100f;
        [SerializeField] private float laserDuration = 0.05f;
        [SerializeField] private float damage = 10f;

        public GunStats stats;

        [HideInInspector] public int currentBulletCount;
        private int currentMagLeft;

        [HideInInspector] public bool isReloading = false;
        public bool IsInShotCooldown { get; private set; } = false;

        public Action onBulletShot;
        public Action onGunReloadStart;
        public Action onGunShootingStart;

        void Start()
        {
            currentBulletCount = stats.magazineSize;
            currentMagLeft = stats.totalAmmo;

            if (laserLine != null)
                laserLine.enabled = false;
        }

        public void Shoot()
        {
            if (currentBulletCount > 0 && !isReloading && !IsInShotCooldown)
            {
                IsInShotCooldown = true;

                onGunShootingStart?.Invoke();

                foreach (var ps in gunParticleSystems)
                    ps.Play();

                currentBulletCount--;

                StartCoroutine(LaserShoot());

                if (currentBulletCount == 0)
                    Reload();

                StartCoroutine(ResetGunShotCooldown());
            }
        }

        IEnumerator LaserShoot()
        {
            if (laserLine == null || muzzleTransform == null)
                yield break;

            laserLine.enabled = true;

            Vector3 start = muzzleTransform.position;
            Vector3 end = start + muzzleTransform.forward * laserDistance;

            RaycastHit hit;

            if (Physics.Raycast(start, muzzleTransform.forward, out hit, laserDistance))
            {
                end = hit.point;

                var health = hit.collider.GetComponent<Health>();
                if (health != null)
                {
                    // FIX L?I 2 PARAM
                    health.TakeDamage(damage, gameObject);
                }
            }

            laserLine.SetPosition(0, start);
            laserLine.SetPosition(1, end);

            yield return new WaitForSeconds(laserDuration);

            laserLine.enabled = false;

            onBulletShot?.Invoke();
        }

        public void Reload()
        {
            StartCoroutine(ReloadGun());
        }

        private IEnumerator ReloadGun()
        {
            if (!isReloading)
            {
                onGunReloadStart?.Invoke();
                isReloading = true;

                yield return new WaitForSeconds(stats.reloadDuration);

                if (currentMagLeft != 0)
                {
                    if (currentMagLeft >= stats.magazineSize)
                    {
                        currentMagLeft -= stats.magazineSize;
                        currentBulletCount = stats.magazineSize;
                    }
                    else
                    {
                        currentBulletCount = currentMagLeft;
                        currentMagLeft = 0;
                    }
                }

                isReloading = false;
            }
        }

        private IEnumerator ResetGunShotCooldown()
        {
            yield return new WaitForSeconds(1 / stats.fireRate);
            IsInShotCooldown = false;
        }
    }
}