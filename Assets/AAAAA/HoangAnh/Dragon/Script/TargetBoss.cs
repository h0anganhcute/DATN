using UnityEngine;
using UnityEngine.AI; // Gọi thư viện AI để dùng NavMesh

public class TargerBoss : MonoBehaviour
{
    Animator ani;

    [Header("Mục tiêu")]
    public Transform nguoiChoi;
    public Transform nguoiChoi2; // Vị trí của người chơi thứ 2 mà quái sẽ đuổi theo

    [Header("Cài đặt Di chuyển")]
    public float tocDo = 3.5f;   // Tốc độ di chuyển của kẻ địch

    // Biến này đóng vai trò là "bộ não" giúp kẻ địch tự tìm đường và né vật cản
    private NavMeshAgent aiDiChuyen;

    // Biến dùng để đếm thời gian (giây)
    private float thoiGianDem = 0f;

    // Biến kiểm tra xem Boss đang ưu tiên đuổi ai (true = nguoiChoi, false = nguoiChoi2)
    private bool dangDuoiNguoiChoi1 = true;

    void Start()
    {
        ani = GetComponent<Animator>();
        // Lấy thành phần NavMeshAgent đã gắn trên Kẻ địch
        aiDiChuyen = GetComponent<NavMeshAgent>();

        // Gán tốc độ di chuyển
        aiDiChuyen.speed = tocDo;

        // Nếu bạn quên chưa kéo Player vào ô Nguoi Choi, code sẽ tự tìm vật thể có tag "Player"
        if (nguoiChoi == null)
        {
            GameObject timNguoiChoi = GameObject.FindGameObjectWithTag("Player");
            if (timNguoiChoi != null)
            {
                nguoiChoi = timNguoiChoi.transform;
            }
        }
        if (nguoiChoi2 == null)
        {
            GameObject timNguoiChoi2 = GameObject.FindGameObjectWithTag("Player2");
            if (timNguoiChoi2 != null)
            {
                nguoiChoi2 = timNguoiChoi2.transform;
            }
        }
    }

    void Update()
    {
        // Cộng dồn thời gian đếm mỗi khung hình (tính bằng giây thực)
        thoiGianDem += Time.deltaTime;

        // Kiểm tra xem đã đủ 10 giây chưa
        if (thoiGianDem >= 10f)
        {
            // Đảo ngược mục tiêu (ví dụ đang là true thì thành false, false thành true)
            dangDuoiNguoiChoi1 = !dangDuoiNguoiChoi1;

            // Reset thời gian đếm về 0 để bắt đầu chu kỳ 10 giây mới
            thoiGianDem = 0f;
        }

        // Chọn mục tiêu hiện tại:
        // Nếu dangDuoiNguoiChoi1 là true -> chọn nguoiChoi
        // Nếu dangDuoiNguoiChoi1 là false -> chọn nguoiChoi2
        Transform mucTieuHienTai = dangDuoiNguoiChoi1 ? nguoiChoi : nguoiChoi2;

        // Kiểm tra xem đã có mục tiêu chưa và Kẻ địch có đang đứng trên vùng di chuyển (NavMesh) không
        if (mucTieuHienTai != null && aiDiChuyen.isOnNavMesh)
        {
            ani.SetTrigger("Run");

            // Ra lệnh cho bộ não AI liên tục đi đến vị trí hiện tại của mục tiêu đã được chọn ở trên.
            aiDiChuyen.SetDestination(mucTieuHienTai.position);
        }
    }
}