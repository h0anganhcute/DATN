using UnityEngine;
using UnityEngine.AI;

public class DragonBay : MonoBehaviour
{
    NavMeshAgent agent;
    Animator ani;

    [Header("Cài đặt CheckPoint")]
    public Transform[] danhSachDiem;

    [Header("Cài đặt Nghỉ Ngơi")]
    public float thoiGianNghi = 5f;
    [Tooltip("Thời gian chờ trước khi khạc lửa tại điểm số 5")]
    public float delayTruocKhiBan = 2f;

    [Header("Cài đặt Tấn Công")]
    [Tooltip("Thời gian chạy xong clip AttackBay (tính bằng giây) để code tự động tắt Bool")]
    public float thoiGianClipAttack = 2.5f;

    private int soLanDaBay = 0;
    private int diemHienTai = -1;

    private float demNguocNghi = 0f;
    private bool dangNghiDiem5 = false;
    private bool daTanCong = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();

        if (danhSachDiem.Length != 5)
        {
            Debug.LogError("Ê mậy! Kéo chưa đủ 5 điểm kìa!");
            return;
        }

        ChonDiemRandomTruDiem5();
    }

    void Update()
    {
        // Đồng bộ animation Bay
        if (ani != null && agent != null && agent.isActiveAndEnabled)
        {
            bool dangDiChuyen = agent.velocity.sqrMagnitude > 0.01f && !agent.isStopped;
            ani.SetBool("Bay", dangDiChuyen);
        }

        if (danhSachDiem.Length != 5 || agent == null || !agent.isActiveAndEnabled || agent.isStopped)
            return;

        // XỬ LÝ LÚC ĐANG NGHỈ TẠI ĐIỂM SỐ 5
        if (dangNghiDiem5)
        {
            demNguocNghi -= Time.deltaTime;

            float thoiGianDaDungYen = thoiGianNghi - demNguocNghi;

            // --- KÍCH HOẠT BẬT CÔNG TẮC BẮN ---
            if (!daTanCong && thoiGianDaDungYen >= delayTruocKhiBan)
            {
                if (ani != null)
                {
                    // 1. Bật công tắc cho đánh
                    ani.SetBool("AttackBay", true);

                    // 2. Hẹn giờ: Sau đúng khoảng thời gian của Clip thì tự chạy hàm tắt công tắc
                    Invoke("TatAnimationAttack", thoiGianClipAttack);
                }
                daTanCong = true;
                Debug.Log("Sau 2 giây nghỉ, Boss bắt đầu khạc lửa!");
            }

            // --- HẾT GIỜ NGHỈ THÌ BAY TIẾP ---
            if (demNguocNghi <= 0)
            {
                dangNghiDiem5 = false;
                soLanDaBay = 0;

                // MỘT LỚP BẢO HIỂM: Ép tắt Bool một lần nữa lỡ thời gian không khớp
                TatAnimationAttack();

                ChonDiemRandomTruDiem5();
            }
            return;
        }

        // KIỂM TRA ĐÍCH ĐẾN
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            soLanDaBay++;

            if (soLanDaBay < 4)
            {
                ChonDiemRandomTruDiem5();
            }
            else if (soLanDaBay == 4)
            {
                diemHienTai = 4;
                agent.SetDestination(danhSachDiem[4].position);
            }
            else if (soLanDaBay >= 5)
            {
                dangNghiDiem5 = true;
                demNguocNghi = thoiGianNghi;
                daTanCong = false;
            }
        }
    }

    // --- HÀM MỚI ĐỂ TẮT ANIMATION ---
    void TatAnimationAttack()
    {
        if (ani != null)
        {
            ani.SetBool("AttackBay", false);
        }
    }

    void ChonDiemRandomTruDiem5()
    {
        int diemMoi = Random.Range(0, 4);
        if (diemMoi == diemHienTai)
        {
            diemMoi = (diemMoi + 1) % 4;
        }
        diemHienTai = diemMoi;
        agent.SetDestination(danhSachDiem[diemHienTai].position);
    }
}