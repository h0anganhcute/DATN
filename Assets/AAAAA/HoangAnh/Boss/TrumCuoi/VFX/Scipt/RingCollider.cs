using UnityEngine;

public class RingCollider : MonoBehaviour
{
    [Header("Kích thước vòng nhẫn")]
    public float radius = 11.5f;
    public float thickness = 7f;
    public float height = 3f;

    [Header("Cấu hình Collider")]
    [Range(6, 36)]
    public int segments = 16;
    public bool isTrigger = false; // Tắt Trigger để làm tường cản 

    // CHỈ CẦN THÊM HÀM NÀY VÀO:
    private void Start()
    {
        GenerateRing();
    }
    private void OnEnable()
    {
        GenerateRing();
    }
    

    [ContextMenu("Tạo / Cập nhật vòng Collider")]
    public void GenerateRing()
    {
        // ... (Giữ nguyên code phần này của bạn)
        // 1. Xoá các khối Collider cũ đi (nếu có) để vẽ lại
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child.name.StartsWith("RingSegment"))
            {
                // Thay đổi nhỏ: Dùng Destroy thay vì DestroyImmediate để an toàn hơn khi chạy trong Game
                if (Application.isPlaying)
                    Destroy(child.gameObject);
                else
                    DestroyImmediate(child.gameObject);
            }
        }

        // 2. Tính toán kích thước mỗi mảnh ghép
        float angleStep = 360f / segments;
        float segmentWidth = (2f * Mathf.PI * radius) / segments;

        // 3. Xếp các Box Collider thành hình tròn
        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleStep;

            GameObject segment = new GameObject($"RingSegment_{i}");
            segment.transform.SetParent(transform);
            segment.transform.localPosition = Vector3.zero;
            segment.transform.localRotation = Quaternion.Euler(0, angle, 0);

            BoxCollider box = segment.AddComponent<BoxCollider>();
            box.isTrigger = isTrigger;

            box.size = new Vector3(segmentWidth * 1.1f, height, thickness);
            box.center = new Vector3(0, height / 2f, radius);
        }
    }
}