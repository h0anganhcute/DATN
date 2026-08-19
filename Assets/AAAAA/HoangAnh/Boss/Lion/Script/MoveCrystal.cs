using UnityEngine;

public class MoveCrystal : MonoBehaviour
{
    private BoxCollider box;
    public float speed = 5f; // Tốc độ di chuyển của crystal

    private Vector3 targetPosition; // Vị trí đích đến tiếp theo

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 1. Tìm GameObject trong Scene đang được gắn tag "BoxClone"
        GameObject boxObject = GameObject.FindGameObjectWithTag("BoxClone");

        // 2. Nếu tìm thấy object, tiếp tục lấy component BoxCollider của object đó
        if (boxObject != null)
        {
            box = boxObject.GetComponent<BoxCollider>();
        }

        // Kiểm tra xem có lấy được box thành công không
        if (box == null)
        {
            Debug.LogWarning("Lỗi: Không tìm thấy GameObject nào có tag 'BoxClone' hoặc GameObject đó không có component BoxCollider!");
            return;
        }

        // Chọn vị trí bay ngẫu nhiên đầu tiên
        SetRandomTargetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (box == null) return;

        // Di chuyển Crystal tiến dần về phía targetPosition
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Kiểm tra xem Crystal đã bay đến đích chưa (hoặc gần đến đích với sai số 0.1f)
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Đã đến nơi -> Tìm một điểm ngẫu nhiên mới để tiếp tục bay
            SetRandomTargetPosition();
        }
    }

    // Hàm dùng để lấy ngẫu nhiên 1 điểm nằm trong giới hạn của BoxCollider
    void SetRandomTargetPosition()
    {
        Bounds bounds = box.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        targetPosition = new Vector3(randomX, randomY, randomZ);
    }
}