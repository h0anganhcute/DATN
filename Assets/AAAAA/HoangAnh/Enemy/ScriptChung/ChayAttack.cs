using UnityEngine;
using UnityEngine.AI; // Gọi thư viện để xài NavMeshAgent

public class ChayAttack : MonoBehaviour
{
    Animator ani;
    NavMeshAgent agent;
    [SerializeField] MonoBehaviour Run;

    [Header("Cài đặt Tấn Công")]
    public Transform nguoiChoi;

    [Tooltip("Thời gian chạy xong cái Animation Attack (tính bằng giây)")]
    public float thoiGianChayHoatAnh = 1.5f;

    private float demNguoc = 0f;
    private bool dangTanCong = false;

    void Start()
    {
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // Tự tìm Player nếu quên kéo thả
        if (nguoiChoi == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) nguoiChoi = p.transform;
        }
    }

    void Update()
    {
        if (nguoiChoi == null) return;

        // Đo khoảng cách giữa Quái và Player
        float khoangCach = Vector3.Distance(transform.position, nguoiChoi.position);

        if (dangTanCong)
        {
            // Nếu đang trong lúc vung tay đấm, bắt đầu đếm lùi thời gian
            demNguoc -= Time.deltaTime;

            if (demNguoc <= 0)
            {
                // Đếm ngược xong -> Hoạt ảnh đánh đã kết thúc (chui vào cục Exit)
                dangTanCong = false;

                // BẬT LẠI mọi thứ để chuẩn bị rượt tiếp
                if (Run != null) Run.enabled = true;

                // --- ĐÃ FIX: CHỈ NHẢ PHANH KHI ĐANG ĐỨNG TRÊN NAVMESH ---
                if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
                {
                    agent.isStopped = false;
                }
            }
        }
        else
        {
            // Nếu không đánh, thì check xem Player đã vào vùng Stopping Distance chưa?
            if (khoangCach <= agent.stoppingDistance)
            {
                BatDauTanCong();
            }
        }
    }

    void BatDauTanCong()
    {
        dangTanCong = true;
        demNguoc = thoiGianChayHoatAnh; // Reset đồng hồ bằng đúng thời gian clip múa võ

        // TẮT kịch bản chạy
        if (Run != null) Run.enabled = false;

        // --- ĐÃ FIX: CHỈ ĐẠP PHANH KHI ĐANG ĐỨNG TRÊN NAVMESH ---
        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        // Gọi Trigger chạy Animation Attack
        ani.SetTrigger("Attack");
    }
}