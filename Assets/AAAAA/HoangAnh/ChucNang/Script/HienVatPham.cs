using UnityEngine;

public class HienVatPham : MonoBehaviour
{
    [Tooltip("Kéo thả quái vật đang có trên Scene vào đây")]
    [SerializeField] private GameObject quaiVat;

    [Tooltip("Kéo thả Prefab vũ khí (hoặc vật phẩm) vào đây")]
    [SerializeField] private GameObject vuKhi;

    private Vector3 viTriCuoiCung; // Biến để "nhớ" vị trí của quái vật
    private bool daRotDo = false;  // Biến chốt chặn để chỉ rớt đồ đúng 1 lần

    void Start()
    {

    }

    void Update()
    {
        // 1. Kiểm tra xem quái vật còn sống không
        if (quaiVat != null)
        {
            // Nếu còn sống, liên tục cập nhật tọa độ mới nhất của nó
            viTriCuoiCung = quaiVat.transform.position;
        }
        // 2. Nếu quaiVat == null (tức là đã bị Destroy) VÀ đồ chưa rớt
        else if (!daRotDo)
        {
            daRotDo = true; // Chốt hạ là đã rớt đồ rồi, không đẻ thêm nữa

            // Lôi Prefab vũ khí ra, đặt đúng vào vị trí cuối cùng đã nhớ, góc xoay mặc định (Quaternion.identity)
            Instantiate(vuKhi, viTriCuoiCung, Quaternion.identity);

            // Xong nhiệm vụ thì tự hủy luôn cái script này đi cho nhẹ game
            Destroy(this);
        }
    }
}