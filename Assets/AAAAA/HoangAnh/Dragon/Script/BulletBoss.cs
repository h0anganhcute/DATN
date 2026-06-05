using UnityEngine;

public class BulletBoss : MonoBehaviour
{
    [Header("Cài đặt Đạn")]
    public float speed = 10f; // Lực đẩy của viên đạn
    public GameObject refapBullet; // Kéo thả viên đạn (Prefab) vào đây từ cửa sổ Project

    private Transform player; // Vị trí của người chơi
    private Transform checkPoint; // Vị trí tạo ra viên đạn (PointAttack)

    void Start()
    {
        // 1. Tự động tìm người chơi thông qua tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Chưa tìm thấy vật thể nào có tag là 'Player' trên bản đồ!");
        }

        // 2. Tự động tìm vị trí tạo đạn thông qua tag "PointAttack"
        GameObject pointObj = GameObject.FindGameObjectWithTag("PointAttack");
        if (pointObj != null)
        {
            checkPoint = pointObj.transform;
        }
        else
        {
            Debug.LogWarning("Chưa tìm thấy vật thể nào có tag là 'PointAttack' trên bản đồ!");
        }
    }

    public void Shoot()
    {
        // Kiểm tra xem đã có đủ đạn prefab, điểm bắn và mục tiêu chưa
        if (refapBullet != null && checkPoint != null && player != null)
        {
            // Bước 1: Tạo ra viên đạn tại đúng vị trí của PointAttack
            GameObject danVuaTao = Instantiate(refapBullet, checkPoint.position, checkPoint.rotation);

            // Bước 2: Tính toán hướng bay (Đích đến trừ đi điểm xuất phát)
            // Lấy vị trí phần giữa thân của Player thay vì dưới chân (cộng thêm 1 chút trục Y)
            Vector3 viTriBan = new Vector3(player.position.x, player.position.y + 1f, player.position.z);
            Vector3 huongBay = (viTriBan - checkPoint.position).normalized;

            // Xoay viên đạn nhìn thẳng về hướng đang bay
            danVuaTao.transform.forward = huongBay;

            // Bước 3: Đẩy viên đạn bay đi
            Rigidbody rbDan = danVuaTao.GetComponent<Rigidbody>();
            if (rbDan != null)
            {
                // Dùng velocity để đẩy đạn bay theo hướng chỉ định với tốc độ speed
                rbDan.linearVelocity = huongBay * speed;
            }
            else
            {
                Debug.LogError("Cảnh báo: refapBullet của bạn chưa gắn Rigidbody! Quả đạn sẽ không bay được.");
            }
        }
    }
}