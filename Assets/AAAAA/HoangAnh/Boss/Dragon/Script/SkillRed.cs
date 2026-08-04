using System.Collections;
using Unity.FPS.Game;
using UnityEngine;

public class SkillRed : MonoBehaviour
{
    public GameObject diemBan;      // Vị trí xuất phát của đạn
    public GameObject bulletPrefab; // Prefab viên đạn
    public Transform player;        // Mục tiêu bắn tới

    public float bulletSpeed = 20f; // Tốc độ bay của đạn

    public void Skill1()
    {
        // Bắt đầu chạy Coroutine bắn đạn
        StartCoroutine(FireBulletsRoutine());
    }

    private IEnumerator FireBulletsRoutine()
    {
        // Vòng lặp bắn 10 viên
        for (int i = 0; i < 10; i++)
        {
            if (player == null || bulletPrefab == null || diemBan == null)
            {
                yield break;
            }

            // 1. Sinh ra viên đạn
            GameObject bullet = Instantiate(bulletPrefab, diemBan.transform.position, Quaternion.identity);

            // 2. Lập lịch tự động phá huỷ viên đạn này sau 5 giây
            Destroy(bullet, 5f);

            // 3. Tính toán hướng bắn. 
            // Lưu ý: Tôi cộng thêm Vector3.up * 1f để tâm ngắm nhích lên giữa thân người chơi
            // (vì thông thường player.position nằm ở dưới gót chân).
            Vector3 targetPosition = player.position + Vector3.up * 1f;
            Vector3 direction = (targetPosition - diemBan.transform.position).normalized;

            // Xoay hướng đạn
            bullet.transform.rotation = Quaternion.LookRotation(direction);

            // 4. Xử lý làm cho đạn bay (Vì đạn không có script tự bay)
            // Lấy component Rigidbody của đạn
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            // Nếu đạn của bạn chưa gắn sẵn Rigidbody, code sẽ tự động thêm vào để nó bay được
            if (rb == null)
            {
                rb = bullet.AddComponent<Rigidbody>();
                rb.useGravity = false; // Tắt trọng lực để đạn bay thẳng theo đường ngắm thay vì bị rớt xuống đất
            }

            // Đẩy viên đạn bay đi bằng cách set vận tốc
            // Lưu ý: Từ Unity 6 trở lên người ta dùng rb.linearVelocity, nhưng rb.velocity vẫn dùng được cho các bản cũ.
            rb.linearVelocity = direction * bulletSpeed;

            // 5. Chờ 0.3s rồi bắn viên tiếp theo
            yield return new WaitForSeconds(0.3f);
        }
    }
}