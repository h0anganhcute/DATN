using UnityEngine;

public class HienVatPham : MonoBehaviour
{
    [Tooltip("Kéo thả quái vật đang có trên Scene vào đây")]
    [SerializeField] private GameObject quaiVat;

    [Tooltip("Kéo thả Vũ khí ĐÃ BỊ ẨN trên Scene vào đây")]
    [SerializeField] private GameObject vuKhi;

    private Vector3 viTriCuoiCung; // Biến để "nhớ" vị trí của quái vật
    private bool daRotDo = false;  // Biến chốt chặn để chỉ rớt đồ đúng 1 lần

    void Start()
    {
        // Có thể code thêm: Tự động ẩn vũ khí ngay từ lúc vào game cho chắc ăn
        // (Phòng trường hợp mày quên tắt nó bằng tay trong màn hình Unity)
        if (vuKhi != null)
        {
            vuKhi.SetActive(false);
        }
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
            daRotDo = true; // Chốt hạ là đã rớt đồ rồi

            // Kiểm tra xem vuKhi có bị lỡ tay xóa mất không (để tránh báo lỗi đỏ)
            if (vuKhi != null)
            {
                // Dịch chuyển (teleport) vũ khí tới vị trí quái chết
                vuKhi.transform.position = viTriCuoiCung;

                // Bật hiển thị vũ khí lên
                vuKhi.SetActive(true);
            }

            // Xong nhiệm vụ thì tự hủy cái script này đi
            Destroy(this);
        }
    }
}