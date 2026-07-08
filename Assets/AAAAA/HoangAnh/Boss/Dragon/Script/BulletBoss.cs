using UnityEngine;

public class BulletBoss : MonoBehaviour
{
    [Header("Cài đặt Đạn")]
    public float speed = 20f; // Lực đẩy viên đạn (bạn có thể tăng giảm trong Inspector)
    public GameObject refapBullet; // Kéo thả viên đạn (Prefab) vào đây từ cửa sổ Project

    private Transform checkPoint; // Vị trí tạo ra viên đạn (PointAttack)

    void Start()
    {
        // Tự động tìm vị trí tạo đạn thông qua tag "PointAttack"
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
        // Kiểm tra xem đã có đủ đạn prefab và điểm bắn chưa
        if (refapBullet != null && checkPoint != null)
        {
            // Bước 1: Tạo ra viên đạn tại vị trí và góc xoay của nòng súng (PointAttack)
            GameObject danVuaTao = Instantiate(refapBullet, checkPoint.position, checkPoint.rotation);

            // Bước 2: Lấy Rigidbody của viên đạn để tác dụng lực
            Rigidbody rbDan = danVuaTao.GetComponent<Rigidbody>();

            if (rbDan != null)
            {
                // Bước 3: Đẩy viên đạn về phía trước (hướng forward của nòng súng) bằng AddForce.
                // Mình dùng ForceMode.Impulse vì đây là một lực đẩy bộc phát ngay lập tức giống như bắn súng.
                rbDan.AddForce(checkPoint.forward * speed, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("Cảnh báo: refapBullet của bạn chưa gắn Rigidbody! Quả đạn sẽ không bay được.");
            }
        }
    }
}