using System.Collections;
using UnityEngine;

public class BulletNguoiSat2 : MonoBehaviour
{
    public GameObject diemBan1;
    public GameObject diemBan2;
    public GameObject diemBan3;
    public GameObject diemBan4; // Vị trí xuất phát của đạn và quyết định hướng bắn
    public GameObject bulletPrefab;  // Prefab viên đạn
    public float thoiGianBan = 0.3f; // Thời gian giữa các lần bắn
    public float bulletSpeed = 20f;  // Tốc độ bay của đạn
    public float thoiGianDesTroy = 5f; // Thời gian hủy đạn
    public float SoLuongDan = 10f;   // Số lượng đạn bắn ra trong 1 lần dùng skill

    public void Skill3()
    {
        // Bắt đầu chạy Coroutine bắn đạn
        StartCoroutine(FireBulletsRoutine());
    }

    private IEnumerator FireBulletsRoutine()
    {
        // Vòng lặp bắn số lượng đạn đã quy định
        for (int i = 0; i < SoLuongDan; i++)
        {
            if (bulletPrefab == null)
            {
                yield break;
            }

            // Xoay luân phiên 50 và -50 độ ở trục Z
            float zRotation = (i % 2 == 0) ? 50f : -50f;

            // 1. Bắn từ diemBan1 (Hướng trục X / right)
            if (diemBan1 != null)
            {
                Vector3 direction1 = diemBan1.transform.right;
                SpawnBullet(diemBan1.transform.position, direction1, zRotation);
            }

            // 2. Bắn từ diemBan2 (Hướng trục -X / left)
            if (diemBan2 != null)
            {
                Vector3 direction2 = -diemBan2.transform.right;
                SpawnBullet(diemBan2.transform.position, direction2, zRotation);
            }

            // 3. Bắn từ diemBan3 (Hướng trục Z / forward)
            if (diemBan3 != null)
            {
                Vector3 direction3 = diemBan3.transform.forward;
                SpawnBullet(diemBan3.transform.position, direction3, zRotation);
            }

            // 4. Bắn từ diemBan4 (Hướng trục -Z / back)
            if (diemBan4 != null)
            {
                Vector3 direction4 = -diemBan4.transform.forward;
                SpawnBullet(diemBan4.transform.position, direction4, zRotation);
            }

            // Chờ thời gian delay rồi mới bắn loạt tiếp theo
            yield return new WaitForSeconds(thoiGianBan);
        }
    }

    private void SpawnBullet(Vector3 position, Vector3 direction, float zRotation)
    {
        // Sinh ra viên đạn tại vị trí
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.LookRotation(direction) * Quaternion.Euler(0, 0, zRotation));

        // Lập lịch tự động phá huỷ viên đạn này sau vài giây
        Destroy(bullet, thoiGianDesTroy);

        // Xử lý làm cho đạn bay
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = bullet.AddComponent<Rigidbody>();
            rb.useGravity = false; // Tắt trọng lực để đạn bay thẳng
        }

        // Đẩy viên đạn bay đi theo hướng
        rb.linearVelocity = direction * bulletSpeed;
    }
}