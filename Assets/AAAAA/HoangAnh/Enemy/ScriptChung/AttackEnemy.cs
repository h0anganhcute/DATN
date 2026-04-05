using UnityEngine;
using Unity.FPS.Game; // Bắt buộc phải gọi thư viện này để nhận diện được lớp Damageable

public class GaySatThuong : MonoBehaviour
{
    [Header("Cài đặt Sát thương")]
    [Tooltip("Lượng máu sẽ trừ của mục tiêu")]
    public float luongSatThuong = 25f;

    [Tooltip("Tích vào nếu đây là sát thương nổ (bom, lựu đạn) để bỏ qua tính toán Headshot")]
    public bool laSatThuongNo = false;

    [Header("Cài đặt Va chạm")]
    [Tooltip("Tích vào nếu muốn vật thể này biến mất sau khi gây dame (ví dụ: viên đạn). Bỏ tích nếu là bãi chông (gây dame liên tục).")]
    public bool bienMatKhiCham = true;

    // --- TRƯỜNG HỢP 1: VA CHẠM XUYÊN QUA (Collider được tích Is Trigger) ---
    // Ví dụ: Lửa, Khí độc, Vùng nổ, hoặc Đạn bay xuyên mục tiêu
    private void OnTriggerEnter(Collider other)
    {
        XuLyGayDame(other.gameObject);
    }

    // --- TRƯỜNG HỢP 2: VA CHẠM VẬT LÝ NẢY RA (Collider KHÔNG tích Is Trigger) ---
    // Ví dụ: Búa tạ, Thiên thạch rơi trúng, Mũi tên cắm vào tường
    private void OnCollisionEnter(Collision collision)
    {
        XuLyGayDame(collision.gameObject);
    }

    // --- HÀM XỬ LÝ CHÍNH ---
    private void XuLyGayDame(GameObject mucTieuVaCham)
    {
        // Lục soát xem trên người mục tiêu có Component Damageable không
        Damageable keXauSo = mucTieuVaCham.GetComponent<Damageable>();

        // Nếu có (khác null), thì tiến hành giáng đòn
        if (keXauSo != null)
        {
            // Gọi hàm InflictDamage từ kịch bản của mục tiêu để trừ máu
            // Truyền vào: (Số dame, Có phải nổ không, Đứa nào vừa đánh (chính là bản thân vật thể này))
            keXauSo.InflictDamage(luongSatThuong, laSatThuongNo, this.gameObject);

            // Xóa vật thể này nếu đã tích chọn (thường dùng cho viên đạn)
            if (bienMatKhiCham)
            {
                Destroy(gameObject);
            }
        }
    }
}