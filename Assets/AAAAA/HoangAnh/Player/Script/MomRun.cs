using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MomRun : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    Animator ani;
    public GameObject ChuyenScene;
    [Tooltip("Kéo thả GameObject chứa bảng thoại của Mom vào đây")]
    public GameObject thoaiMom;
    [Tooltip("Kéo thả GameObject chứa bảng thoại của Con Trai vào đây")]
    public GameObject thoaiConTrai;
    [Tooltip("Kéo thả GameObject chứa bảng thoại thứ 2 của Con Trai vào đây")]
    public GameObject thoaiConTrai2;
    private Vector3 viTriBanDau;

    // Đã thêm và đổi tên trạng thái cho rõ ràng
    private enum TrangThai { DangDiDenCua, DangChoThoaiConTrai, DangChoThoaiMom, DangDiVe, DaVeDenNha }
    private TrangThai trangThaiHienTai = TrangThai.DangDiDenCua;

    void Start()
    {
        ani = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        viTriBanDau = transform.position;

        // Tắt sẵn thoại lúc bắt đầu game cho chắc chắn
        if (thoaiMom != null)
        {
            thoaiMom.SetActive(false);
        }

        // Tắt luôn thoại 2 của con trai để chờ đến đúng thời điểm mới bật
        if (thoaiConTrai2 != null)
        {
            thoaiConTrai2.SetActive(false);
        }

        GameObject door = GameObject.FindGameObjectWithTag("Door");
        if (door != null)
        {
            navMeshAgent.SetDestination(door.transform.position);
            trangThaiHienTai = TrangThai.DangDiDenCua;

            // Bắt đầu chạy đến cửa -> Bật animation chạy
            ani.SetBool("Run", true);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy GameObject nào có tag là 'Door' trong Scene!");
        }
    }

    void Update()
    {
        // TRẠNG THÁI 1: ĐANG CHẠY ĐẾN CỬA
        if (trangThaiHienTai == TrangThai.DangDiDenCua)
        {
            if (DaDenDichChua())
            {
                // Đã đến cửa -> Dừng lại, tắt animation chạy
                ani.SetBool("Run", false);

                // Chuyển sang bước tiếp theo: Chờ thoại của con trai tắt
                trangThaiHienTai = TrangThai.DangChoThoaiConTrai;
            }
        }
        // TRẠNG THÁI 2: ĐỨNG CHỜ CON TRAI NÓI XONG
        else if (trangThaiHienTai == TrangThai.DangChoThoaiConTrai)
        {
            // Kiểm tra xem thoại con trai ĐÃ TẮT chưa (hoặc bị bỏ trống không gán)
            if (thoaiConTrai == null || !thoaiConTrai.activeSelf)
            {
                // Khi thoại con trai đã tắt -> Tới lượt Mom nói
                if (thoaiMom != null)
                {
                    thoaiMom.SetActive(true);
                }

                // Chuyển sang bước tiếp theo: Chờ thoại của Mom tắt
                trangThaiHienTai = TrangThai.DangChoThoaiMom;
            }
        }
        // TRẠNG THÁI 3: ĐANG HIỂN THỊ THOẠI CỦA MOM
        else if (trangThaiHienTai == TrangThai.DangChoThoaiMom)
        {
            // Liên tục kiểm tra xem bảng thoại của Mom đã bị tắt chưa 
            if (thoaiMom != null && !thoaiMom.activeSelf)
            {
                // Thoại của Mom ĐÃ TẮT -> Bắt đầu đi về
                navMeshAgent.SetDestination(viTriBanDau);
                trangThaiHienTai = TrangThai.DangDiVe;

                // Bật lại animation chạy
                ani.SetBool("Run", true);
            }
        }
        // TRẠNG THÁI 4: ĐANG ĐI VỀ 
        else if (trangThaiHienTai == TrangThai.DangDiVe)
        {
            if (DaDenDichChua())
            {
                trangThaiHienTai = TrangThai.DaVeDenNha;

                // Đã về đến nhà và dừng lại -> Tắt animation chạy
                ani.SetBool("Run", false);

                // --- THÊM MỚI Ở ĐÂY ---
                // Bật bảng thoại lần 2 của con trai lên
                if (thoaiConTrai2 != null)
                {
                    thoaiConTrai2.SetActive(true);
                }

                // Bật GameObject ChuyenScene khi đã quay về vị trí ban đầu
                if (ChuyenScene != null)
                {
                    ChuyenScene.SetActive(true);
                }

                Debug.Log("Đã quay về vị trí ban đầu thành công và bật thoaiConTrai2 cùng ChuyenScene!");
            }
        }
    }

    // Hàm kiểm tra xem NavMeshAgent đã đi đến đích chưa
    private bool DaDenDichChua()
    {
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
}