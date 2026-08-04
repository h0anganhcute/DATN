using UnityEngine;

public class RunRedDragon : MonoBehaviour
{
    public GameObject diem1;
    public GameObject diem2;
    public GameObject diem3;
    public GameObject diem4;

    // Các biến điều chỉnh tốc độ
    public float speed = 5f;          // Tốc độ di chuyển
    public float rotationSpeed = 10f; // Tốc độ xoay (càng nhỏ xoay càng chậm)

    Animator ani;

    // Dùng một mảng để lưu các điểm đến
    private Transform[] points;
    private int currentPointIndex = 0;

    void Start()
    {
        ani = GetComponent<Animator>();

        // Gán transform của các GameObject vào mảng
        points = new Transform[] { diem1.transform, diem2.transform, diem3.transform, diem4.transform };

        // Kích hoạt animation chạy ngay khi bắt đầu
        
    }

    void Update()
    {
        ani.SetTrigger("Run");
        // Tránh lỗi nếu chưa gán các điểm
        if (points == null || points.Length == 0) return;

        // Điểm đích hiện tại
        Transform targetPoint = points[currentPointIndex];

        // 1. XỬ LÝ DI CHUYỂN
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // 2. XỬ LÝ XOAY MƯỢT MÀ
        // Tính vector hướng từ vị trí hiện tại đến đích
        Vector3 direction = (targetPoint.position - transform.position).normalized;

        // Đảm bảo direction không bằng 0 để tránh lỗi (khi object đang đứng trùng với đích)
        if (direction != Vector3.zero)
        {
            // Tính toán góc xoay cần thiết để nhìn về hướng direction
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            // Nội suy góc quay hiện tại tới góc quay mới một cách mượt mà (Slerp)
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }

        // 3. KIỂM TRA ĐẾN ĐÍCH
        // Kiểm tra xem object đã đến gần đích chưa (khoảng cách < 0.1f)
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            // Chuyển sang điểm tiếp theo. Vòng lặp sẽ quay trở lại 0 khi tới điểm cuối cùng.
            currentPointIndex = (currentPointIndex + 1) % points.Length;
        }
    }
}