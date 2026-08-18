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

        [Header("Spread Settings")]
        [Tooltip("Góc tỏa đạn khi bắn thường (không nhắm)")]
        [SerializeField] private float defaultSpreadAngle = 5f;
        [Tooltip("Hệ số tỏa đạn khi nhắm (Ví dụ 0.1 tức là độ tỏa đạn chỉ còn 10% so với bình thường)")]
        [SerializeField] private float aimSpreadMultiplier = 0.1f;

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
            
            // Check if player is aiming
            var weaponsManager = GetComponentInParent<Unity.FPS.Gameplay.PlayerWeaponsManager>();
            bool isAiming = weaponsManager != null && weaponsManager.IsAiming;

            // Calculate current spread angle
            float currentSpread = isAiming ? (defaultSpreadAngle * aimSpreadMultiplier) : defaultSpreadAngle;
            
            Vector3 shootDirection = muzzleTransform.forward;
            if (currentSpread > 0f)
            {
                float spreadRatio = currentSpread / 180f;
                shootDirection = Vector3.Slerp(shootDirection, UnityEngine.Random.insideUnitSphere, spreadRatio);
            }

            Vector3 end = start + shootDirection * laserDistance;

            RaycastHit hit;

            if (Physics.Raycast(start, shootDirection, out hit, laserDistance))
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