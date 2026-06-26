using UnityEngine;

public class BulletAll : MonoBehaviour
{
    [Header("Mục tiêu")]
    public Transform nguoiChoi;
    public Transform nguoiChoi2;

    [Header("Cài đặt Đạn")]
    public float speed = 20f; // Tốc độ bay của đạn

    private Transform mucTieuCuaDan; // Lưu xem viên đạn quyết định bay đuổi theo ai

    void Start()
    {
        // 1. Tự động tìm người chơi nếu chưa được kéo thả
        if (nguoiChoi == null)
        {
            GameObject timNguoiChoi = GameObject.FindGameObjectWithTag("Player");
            if (timNguoiChoi != null) nguoiChoi = timNguoiChoi.transform;
        }
        if (nguoiChoi2 == null)
        {
            GameObject timNguoiChoi2 = GameObject.FindGameObjectWithTag("Player2");
            if (timNguoiChoi2 != null) nguoiChoi2 = timNguoiChoi2.transform;
        }

        // 2. Phân tích góc ngắm để biết Boss đang bắn ai
        mucTieuCuaDan = TuDongNhanDienMucTieu();
    }

    // Hàm thông minh giúp viên đạn xác định mục tiêu lúc vừa sinh ra
    Transform TuDongNhanDienMucTieu()
    {
        if (nguoiChoi == null && nguoiChoi2 == null) return null;
        if (nguoiChoi == null) return nguoiChoi2;
        if (nguoiChoi2 == null) return nguoiChoi;

        // Tính góc lệch
        Vector3 huongToiP1 = (nguoiChoi.position - transform.position).normalized;
        float gocLechP1 = Vector3.Angle(transform.forward, huongToiP1);

        Vector3 huongToiP2 = (nguoiChoi2.position - transform.position).normalized;
        float gocLechP2 = Vector3.Angle(transform.forward, huongToiP2);

        // Góc nào nhỏ hơn (đứng gần hướng nòng súng hơn) thì đuổi theo người đó
        if (gocLechP1 < gocLechP2)
        {
            return nguoiChoi;
        }
        else
        {
            return nguoiChoi2;
        }
    }

    void Update()
    {
        // 3. Đạn liên tục cập nhật vị trí để bay đuổi theo mục tiêu
        if (mucTieuCuaDan != null)
        {
            // Lấy vị trí giữa thân người chơi để đạn bay vào người (chứ không bay xuống chân)
            Vector3 viTriBan = new Vector3(mucTieuCuaDan.position.x, mucTieuCuaDan.position.y + 1f, mucTieuCuaDan.position.z);

            // Xoay đầu viên đạn nhìn về phía mục tiêu
            transform.LookAt(viTriBan);

            // Bay từ vị trí hiện tại của đạn tới vị trí người chơi với tốc độ speed
            transform.position = Vector3.MoveTowards(transform.position, viTriBan, speed * Time.deltaTime);
        }
    }
}