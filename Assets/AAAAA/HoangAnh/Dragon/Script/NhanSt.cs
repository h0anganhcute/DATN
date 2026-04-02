using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.Game
{
    public class NhanSt : MonoBehaviour
    {
        [Tooltip("Multiplier to apply to the received damage")]
        public float DamageMultiplier = 1f;

        [Range(0, 1)]
        [Tooltip("Multiplier to apply to self damage")]
        public float SensibilityToSelfdamage = 0.5f;

        [Tooltip("Kéo thả đối tượng UI Text hiển thị sát thương vào đây")]
        public Text DamageTextDisplay;

        // 1. Khai báo thêm biến Pivot để điều khiển hướng xoay của Text
        [Tooltip("Kéo thả Transform của UI Text (hoặc object chứa nó) vào đây")]
        public Transform DamageTextPivot;

        public Health Health { get; private set; }

        void Awake()
        {
            // find the health component either at the same level, or higher in the hierarchy
            Health = GetComponent<Health>();
            if (!Health)
            {
                Health = GetComponentInParent<Health>();
            }
        }

        // 2. Thêm hàm Update để cập nhật hướng xoay liên tục
        void Update()
        {
            // Kiểm tra xem đã gán Pivot và có Camera chính (người chơi) trong Scene chưa
            if (DamageTextPivot != null && Camera.main != null)
            {
                // Xoay Pivot hướng về vị trí của Camera
                DamageTextPivot.LookAt(Camera.main.transform.position);
            }
        }

        public void InflictDamage(float damage, bool isExplosionDamage, GameObject damageSource)
        {
            if (Health)
            {
                var totalDamage = damage;

                // skip the crit multiplier if it's from an explosion
                if (!isExplosionDamage)
                {
                    totalDamage *= DamageMultiplier;
                }

                // potentially reduce damages if inflicted by self
                if (Health.gameObject == damageSource)
                {
                    totalDamage *= SensibilityToSelfdamage;
                }

                // Cập nhật Text hiển thị
                if (DamageTextDisplay != null)
                {
                    DamageTextDisplay.text = "-" + totalDamage.ToString("0");
                }

                // apply the damages
                Health.TakeDamage(totalDamage, damageSource);
            }
        }
    }
}