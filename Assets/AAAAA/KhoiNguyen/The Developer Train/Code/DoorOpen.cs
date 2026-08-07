using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [Header("Cài Đặt Cửa")]
    [Tooltip("Kéo thả GameObject hình ảnh cánh cửa vào đây")]
    public Transform doorMesh; // THÊM BIẾN NÀY ĐỂ TÁCH BIỆT CÁNH CỬA VÀ TRIGGER

    public float moveDistance = 2f;
    public float moveSpeed = 5f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool isOpen = false;
    private bool isPlayerNear = false;

    void Start()
    {
        // Nếu đã gán doorMesh thì mới lấy vị trí
        if (doorMesh != null)
        {
            closedPosition = doorMesh.position;
            // Tính toán vị trí mở dựa trên trục Z của cánh cửa
            openPosition = closedPosition + doorMesh.forward * moveDistance;
        }
        else
        {
            Debug.LogError("Vui lòng kéo thả cánh cửa vào ô Door Mesh trong Inspector!");
        }
    }

    void Update()
    {
        if (doorMesh == null) return; // Nếu quên chưa gán cánh cửa thì không làm gì cả để tránh lỗi

        // Kiểm tra nếu người chơi đang ở gần VÀ bấm phím E
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen; // Đảo ngược trạng thái mở/đóng
        }

        // Cửa tự động di chuyển đến vị trí đích mượt mà
        Vector3 targetPosition = isOpen ? openPosition : closedPosition;

        // CHÚ Ý: Bây giờ chúng ta chỉ di chuyển doorMesh, còn vùng Trigger vẫn đứng im tại chỗ
        doorMesh.position = Vector3.MoveTowards(doorMesh.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
        else if (other.CompareTag("Mom"))
        {
            isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
        else if (other.CompareTag("Mom"))
        {
            isOpen = false;
        }
    }
}