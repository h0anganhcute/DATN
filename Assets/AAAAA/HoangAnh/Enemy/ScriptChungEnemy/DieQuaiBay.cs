using UnityEngine;
using Unity.FPS.Game; // Gọi cái này để xài được class Health của mày

public class DieQuaiBay : MonoBehaviour
{
    Health mau;
    Rigidbody rb;

    void Start()
    {
        // Lấy các Component trên người con quái bay
        mau = GetComponent<Health>();
        rb = GetComponent<Rigidbody>();

        // Lắng nghe sự kiện: Chừng nào máu gọi OnDie thì tao chạy hàm RoiXuongDat
        if (mau != null)
        {
            mau.OnDie += RoiXuongDat;
        }
    }

    void RoiXuongDat()
    {
        if (rb != null)
        {
            // TẮT Is Kinematic để trả nó về lại cho hệ thống Vật lý quản lý
            rb.isKinematic = false;

            // Tiện tay tao bật luôn Trọng lượng (Gravity) phòng hờ lúc mày setup quên bật
            rb.useGravity = true;
        }
    }

    // Luật giang hồ: Có đăng ký (+=) thì lúc chết (bị Destroy) phải hủy đăng ký (-=)
    void OnDestroy()
    {
        if (mau != null)
        {
            mau.OnDie -= RoiXuongDat;
        }
    }
}