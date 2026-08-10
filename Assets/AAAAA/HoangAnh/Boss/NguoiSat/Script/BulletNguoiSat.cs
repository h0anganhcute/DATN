using System.Collections;
using UnityEngine;

public class BulletNguoiSat : MonoBehaviour
{
    public GameObject diemBan;       // Vị trí xuất phát của đạn và quyết định hướng bắn
    public GameObject bulletPrefab;  // Prefab viên đạn
    public float thoiGianBan = 0.3f; // Thời gian giữa các lần bắn
    public float bulletSpeed = 20f;  // Tốc độ bay của đạn
    public float thoiGianDesTroy = 5f; // Thời gian hủy đạn
    public float SoLuongDan = 10f;   // Số lượng đạn bắn ra trong 1 lần dùng skill

    public void Skill1()
    {
        // Bắt đầu chạy Coroutine bắn đạn
        StartCoroutine(FireBulletsRoutine());
    }

    private IEnumerator FireBulletsRoutine()
    {
        // Vòng lặp bắn số lượng đạn đã quy định
        for (int i = 0; i < SoLuongDan; i++)
        {
            if (bulletPrefab == null || diemBan == null)
            {
                yield break;
            }

            // 1. Tính toán hướng bắn (bắn thẳng về hướng trục X của diemBan)
            // Trong Unity: trục X (đỏ) là .right, trục Y (xanh lá) là .up, trục Z (xanh dương) là .forward
            Vector3 direction = diemBan.transform.right;

            // 2. Sinh ra viên đạn tại vị trí của diemBan
            // Dùng Quaternion.LookRotation để viên đạn quay mặt về hướng đang bay tới (trục X)
            GameObject bullet = Instantiate(bulletPrefab, diemBan.transform.position, Quaternion.LookRotation(direction));

            // 3. Lập lịch tự động phá huỷ viên đạn này sau vài giây (thoiGianDesTroy)
            Destroy(bullet, thoiGianDesTroy);

            // 4. Xử lý làm cho đạn bay
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb == null)
            {
                rb = bullet.AddComponent<Rigidbody>();
                rb.useGravity = false; // Tắt trọng lực để đạn bay thẳng
            }

            // 5. Đẩy viên đạn bay đi theo hướng trục X
            rb.linearVelocity = direction * bulletSpeed;

            // Chờ thời gian delay rồi mới bắn viên tiếp theo
            yield return new WaitForSeconds(thoiGianBan);
        }
    }
}