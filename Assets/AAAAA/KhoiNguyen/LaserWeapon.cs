using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    public class LaserWeaponController : MonoBehaviour
    {
        [Header("References")]
        public Transform WeaponMuzzle;
        public GameObject WeaponRoot;
        public LineRenderer laserLine;

        [Header("Shoot")]
        public float DelayBetweenShots = 0.5f;
        public float damage = 10f;
        public float laserDistance = 100f;
        public float laserDuration = 0.05f;

        [Header("Ammo (gi? gi?ng WeaponController)")]
        public bool AutomaticReload = true;
        public float AmmoReloadRate = 1f;
        public float AmmoReloadDelay = 2f;
        public int MaxAmmo = 10;

        float m_CurrentAmmo;
        float m_LastTimeShot = Mathf.NegativeInfinity;
        bool m_IsReloading = false;

        public float CurrentAmmoRatio { get; private set; }
        public bool IsCooling { get; private set; }

        public UnityAction OnShoot;

        void Start()
        {
            m_CurrentAmmo = MaxAmmo;

            if (laserLine != null)
                laserLine.enabled = false;
        }

        void Update()
        {
            UpdateAmmo();

            // test input (chu?t trái)
            if (Input.GetMouseButtonDown(0))
            {
                TryShoot();
            }
        }

        void UpdateAmmo()
        {
            if (AutomaticReload && m_LastTimeShot + AmmoReloadDelay < Time.time && m_CurrentAmmo < MaxAmmo)
            {
                m_CurrentAmmo += AmmoReloadRate * Time.deltaTime;
                m_CurrentAmmo = Mathf.Clamp(m_CurrentAmmo, 0, MaxAmmo);
                IsCooling = true;
            }
            else
            {
                IsCooling = false;
            }

            CurrentAmmoRatio = m_CurrentAmmo / MaxAmmo;
        }

        public void TryShoot()
        {
            if (m_CurrentAmmo < 1f || m_IsReloading)
                return;

            if (Time.time - m_LastTimeShot < DelayBetweenShots)
                return;

            Shoot();
        }

        void Shoot()
        {
            m_LastTimeShot = Time.time;
            m_CurrentAmmo -= 1f;

            StartCoroutine(LaserRoutine());

            OnShoot?.Invoke();
        }

        IEnumerator LaserRoutine()
        {
            if (laserLine == null || WeaponMuzzle == null)
                yield break;

            laserLine.enabled = true;

            Vector3 start = WeaponMuzzle.position;
            Vector3 end = start + WeaponMuzzle.forward * laserDistance;

            RaycastHit hit;

            if (Physics.Raycast(start, WeaponMuzzle.forward, out hit, laserDistance))
            {
                end = hit.point;

                var health = hit.collider.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(damage, gameObject);
                }
            }

            laserLine.SetPosition(0, start);
            laserLine.SetPosition(1, end);

            yield return new WaitForSeconds(laserDuration);

            laserLine.enabled = false;
        }

        public void ShowWeapon(bool show)
        {
            if (WeaponRoot != null)
                WeaponRoot.SetActive(show);
        }
    }
}