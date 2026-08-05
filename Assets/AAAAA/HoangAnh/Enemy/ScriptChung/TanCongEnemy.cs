using UnityEngine;
using UnityEngine.AI;

public class TanCongEnemy : MonoBehaviour
{
    NavMeshAgent agent;
    Animator ani;

    [Header("Cài đặt Tấn công")]
    public float thoiGianHoiChieu = 1.5f; // Thời gian giãn cách giữa các đòn đánh
    private float thoiGianDanhTiepTheo = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Kiểm tra xem quái vật có đang di chuyển không bằng cách check vận tốc (velocity)
        if (agent.velocity.magnitude < 0.1f)
        {
            // Đang đứng yên -> Tấn công
            // Sử dụng bộ đếm thời gian để không gọi SetTrigger liên tục mỗi frame (tránh lỗi animation)
            if (Time.time >= thoiGianDanhTiepTheo)
            {
                ani.SetTrigger("Attack");
                thoiGianDanhTiepTheo = Time.time + thoiGianHoiChieu; // Cài đặt thời gian cho đòn đánh tiếp theo
            }
        }
    }
}