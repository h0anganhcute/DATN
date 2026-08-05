using System.Collections;
using UnityEngine;

public class BatCollider : MonoBehaviour
{
    private BoxCollider boxCollider;
    
    [Tooltip("Thời gian duy trì trạng thái BẬT của Collider")]
    public float thoiGianBat = 1f;
    
    [Tooltip("Thời gian chờ trước khi BẬT")]
    public float delay = 1f;
    
    public float thoiGianTat = 1f; // Biến này giữ nguyên theo code cũ của bạn

    // Awake được gọi TỚI TRƯỚC OnEnable và Start, giúp lấy BoxCollider an toàn
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // OnEnable TỰ ĐỘNG ĐƯỢC GỌI mỗi khi GameObject chuyển từ tắt (false) sang BẬT (true)
    void OnEnable()
    {
        if (boxCollider != null)
        {
            // Đảm bảo mặc định khi vừa được bật lại thì Collider luôn tắt trước
            boxCollider.enabled = false;
            
            // Bắt đầu đếm ngược thời gian delay -> bật -> tắt
            StartCoroutine(XuLyCollider());
        }
    }

    private IEnumerator XuLyCollider()
    {
        // 1. Chờ một khoảng thời gian Delay
        yield return new WaitForSeconds(delay);

        // 2. Bật Collider lên
        boxCollider.enabled = true;

        // 3. Giữ trạng thái bật trong khoảng thời gian thoiGianBat
        yield return new WaitForSeconds(thoiGianBat);

        // 4. Tắt Collider đi
        boxCollider.enabled = false;
    }
}
