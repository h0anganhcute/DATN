using UnityEngine;

public class CloneLocXoay : MonoBehaviour
{
    public GameObject LocXoay;

    // Biến này giúp xác định những Layer nào tia sẽ đâm trúng. 
    // Mặc định ~0 (Everything) là đâm trúng mọi thứ có Collider.
    public LayerMask groundLayer = ~0;

    void Start()
    {
        Invoke(nameof(SpawnLocXoay), 3f);
    }

    void SpawnLocXoay()
    {
        if (LocXoay != null)
        {
            Vector3 spawnPosition = transform.position;

            // Tự động bắn 1 tia từ độ cao 100 mét cắm thẳng xuống đất tại toạ độ X, Z hiện tại
            Ray ray = new Ray(new Vector3(spawnPosition.x, spawnPosition.y + 100f, spawnPosition.z), Vector3.down);

            // Hàm Physics.Raycast sẽ quét toàn bộ không gian (thay vì chỉ quét 1 Terrain cụ thể)
            // Nó sẽ tự động chạm vào Terrain Collider của bạn ở dưới
            if (Physics.Raycast(ray, out RaycastHit hit, 200f, groundLayer))
            {
                // Lấy toạ độ Y của mặt đất vừa bị bắn trúng
                spawnPosition.y = hit.point.y;
            }
            else
            {
                Debug.LogWarning("Không tìm thấy mặt đất! Object đang nằm ngoài map hoặc dưới đất không có Collider.");
            }

            // Clone object ra
            Instantiate(LocXoay, spawnPosition, LocXoay.transform.rotation);
        }
        else
        {
            Debug.LogWarning("Chưa gán GameObject LocXoay trong Inspector!");
        }
    }

    void Update()
    {
        // Trống
    }
}