using UnityEngine;
using UnityEngine.AI;
using Unity.FPS.Game; // Bắt buộc gọi để xài Script Health

public class BanXaAttack : MonoBehaviour
{
    Animator ani;
    NavMeshAgent agent;
    Health mau; // Khai báo biến máu
    [SerializeField] MonoBehaviour Run;

    [Header("Cài đặt Mục Tiêu")]
    public Transform nguoiChoi;

    [Header("Cài đặt Bắn Xa")]
    [Tooltip("Khoảng cách bắt đầu xả đạn (Nên bằng với Stopping Distance của NavMesh)")]
    public float khoangCachBan = 20f;

    [Tooltip("Kéo Prefab viên đạn (ví dụ: DanLua) vào đây")]
    public GameObject danPrefab;

    [Tooltip("Tạo 1 Empty GameObject ở ngay nòng súng/miệng quái rồi kéo vào đây")]
    public Transform viTriBan;

    [Tooltip("Thời gian chạy TỔNG CỘNG của clip Animation (tính bằng giây)")]
    public float thoiGianChayHoatAnh = 1.5f;

    [Tooltip("Thời gian CHỜ tính từ lúc bắt đầu chém tay, đến lúc viên đạn bay ra")]
    public float delayBanDan = 0.5f;

    private float demNguoc = 0f;
    private bool dangTanCong = false;
    private bool daBanDan = false;

    void Start()
    {
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        mau = GetComponent<Health>(); // Lấy script Health

        // Tự tìm Player nếu quên kéo thả
        if (nguoiChoi == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) nguoiChoi = p.transform;
        }

        // --- ĐỒNG BỘ HÓA TỰ ĐỘNG ---
        // Ép cái Stopping Distance của NavMesh bằng đúng với Khoảng Cách Bắn mày chỉnh ngoài Inspector
        if (agent != null)
        {
            agent.stoppingDistance = khoangCachBan;
        }
    }

    void Update()
    {
        // --- CHỐT CHẶN 1: MÁU BẰNG 0 THÌ DỪNG MỌI HOẠT ĐỘNG ---
        if (mau != null && mau.CurrentHealth <= 0f)
        {
            dangTanCong = false;
            return; // Thoát ngay và luôn
        }

        if (nguoiChoi == null) return;

        float khoangCachThucTe = Vector3.Distance(transform.position, nguoiChoi.position);

        if (dangTanCong)
        {
            demNguoc -= Time.deltaTime;

            // ĐẾM NGƯỢC XONG THÌ BẮN
            if (!daBanDan && demNguoc <= (thoiGianChayHoatAnh - delayBanDan))
            {
                ThucHienBan();
                daBanDan = true;
            }

            // KẾT THÚC ANIMATION -> NHẢ PHANH ĐI TIẾP
            if (demNguoc <= 0)
            {
                dangTanCong = false;

                if (Run != null) Run.enabled = true;

                if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
                {
                    agent.isStopped = false;
                }
            }
        }
        else
        {
            // --- KIỂM TRA TẦM BẮN DỰA VÀO BIẾN NGOÀI INSPECTOR ---
            if (khoangCachThucTe <= khoangCachBan)
            {
                BatDauTanCong();
            }
        }
    }

    void BatDauTanCong()
    {
        dangTanCong = true;
        daBanDan = false;
        demNguoc = thoiGianChayHoatAnh;

        if (Run != null) Run.enabled = false;

        if (agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        // XOAY MẶT TÌM CHỦ TỚ
        Vector3 huongNhin = nguoiChoi.position - transform.position;
        huongNhin.y = 0;

        if (huongNhin != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(huongNhin);
        }

        ani.SetTrigger("Attack");
    }

    void ThucHienBan()
    {
        // --- CHỐT CHẶN 2: CHẾT GIỮA CHỪNG THÌ KHÔNG ĐẺ ĐẠN ---
        if (mau != null && mau.CurrentHealth <= 0f) return;

        if (danPrefab != null && viTriBan != null)
        {
            Instantiate(danPrefab, viTriBan.position, viTriBan.rotation);
        }
        else
        {
            Debug.LogWarning("Ê mậy! Quên kéo Prefab Đạn hoặc Vị trí bắn kìa!");
        }
    }
}