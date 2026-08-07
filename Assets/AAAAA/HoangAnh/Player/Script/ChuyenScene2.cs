using UnityEngine;
using UnityEngine.SceneManagement; // Thêm thư viện để xử lý chuyển Scene
using System.Collections; // Thêm thư viện để sử dụng Coroutine

public class ChuyenScene2 : MonoBehaviour
{
    public GameObject caMera;
    public string tenSceneChuyen; // Tên scene bạn muốn chuyển tới (điền trong Inspector)
    public GameObject player; // Tham chiếu đến Player để có thể tắt đi
    
    private bool isPlayerInZone = false; // Biến kiểm tra Player có đang đứng trong vùng không
    private bool isTriggered = false; // Biến tránh việc bấm nút E nhiều lần

    void Update()
    {
        // Nếu Player đang ở trong vùng, bấm phím E, và sự kiện chưa được kích hoạt
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E) && !isTriggered)
        {
            isTriggered = true; // Đánh dấu là đã bấm rồi

            // Tắt GameObject Player
            if (player != null)
            {
                player.SetActive(false);
            }

            // Bật GameObject Camera và chạy hiệu ứng Zoom
            if (caMera != null)
            {
                caMera.SetActive(true);
                StartCoroutine(ZoomCamera()); // Chạy coroutine giảm FOV
            }

            // 5 giây sau sẽ chạy hàm chuyển scene
            Invoke("ThucHienChuyenScene", 5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }

    private void ThucHienChuyenScene()
    {
        // Kiểm tra xem đã điền tên scene chưa để tránh lỗi
        if (!string.IsNullOrEmpty(tenSceneChuyen))
        {
            SceneManager.LoadScene(tenSceneChuyen);
        }
        else
        {
            Debug.LogWarning("Bạn chưa nhập tên Scene cần chuyển trong Inspector!");
        }
    }

    // Hàm Coroutine để tạo hiệu ứng giảm Field of View
    private IEnumerator ZoomCamera()
    {
        // Lấy component Camera từ GameObject caMera
        Camera camComponent = caMera.GetComponent<Camera>();
        
        if (camComponent != null)
        {
            float startFOV = 80f;
            float endFOV = 29f;
            float duration = 4.5f;
            float timeElapsed = 0f;

            camComponent.fieldOfView = startFOV;

            // Chạy vòng lặp trong 4.5 giây
            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                // Tính toán Field of View hiện tại mượt mà (Lerp)
                camComponent.fieldOfView = Mathf.Lerp(startFOV, endFOV, timeElapsed / duration);
                
                yield return null; // Chờ frame tiếp theo
            }

            // Đảm bảo kết thúc đúng con số 29
            camComponent.fieldOfView = endFOV;
        }
        else
        {
            Debug.LogWarning("GameObject caMera không có Component Camera!");
        }
    }
}