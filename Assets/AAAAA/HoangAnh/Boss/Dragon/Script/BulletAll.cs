using UnityEngine;

public class BulletAll : MonoBehaviour
{
    [Header("Mục tiêu")]
    public Transform nguoiChoi;
    public Transform nguoiChoi2;

    [Header("Cài đặt Đạn")]
    public float speed = 20f; // Tốc độ bay của đạn

    // Thay vì lưu mục tiêu để đuổi theo, ta chỉ lưu "Hướng bay cố định" lúc vừa sinh ra
    private Vector3 huongBayCoDinh;

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

        // 2. Phân tích góc ngắm để tìm ra kẻ địch đang bị nhắm tới lúc bóp cò
        Transform mucTieuCuaDan = TuDongNhanDienMucTieu();

        // 3. CHỐT HƯỚNG BAY NGAY LÚC VỪA SINH RA ĐẠN
        if (mucTieuCuaDan != null)
        {
            // Lấy vị trí giữa thân người chơi
            Vector3 viTriBan = new Vector3(mucTieuCuaDan.position.x, mucTieuCuaDan.position.y + 1f, mucTieuCuaDan.position.z);

            // Tính toán hướng từ nòng súng tới người chơi và LƯU LẠI VÀO BIẾN CỐ ĐỊNH
            huongBayCoDinh = (viTriBan - transform.position).normalized;

            // Xoay đầu viên đạn nhìn thẳng về hướng đó 1 lần duy nhất
            transform.forward = huongBayCoDinh;
        }
        else
        {
            // Nếu không tìm thấy ai, cứ bay thẳng theo hướng mặt định của nòng súng
            huongBayCoDinh = transform.forward;
        }
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

        // Góc nào nhỏ hơn (đứng gần hướng nòng súng hơn) thì xác định là người đó
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
        // 4. Đạn cứ thế bay thẳng theo cái hướng đã chốt ở Start, 
        // Dù người chơi có chạy đi chỗ khác thì đạn vẫn không rẽ hướng.
        transform.position += huongBayCoDinh * speed * Time.deltaTime;
    }
}