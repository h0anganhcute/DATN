using UnityEngine;

public class MenuBoss : MonoBehaviour
{
    public GameObject menuBoss;
    public GameObject QuaiVat;
    public GameObject Dragon;

    // Biến cờ (flag) để đảm bảo người chơi chỉ được chọn Boss 1 lần duy nhất
    private bool daChonBoss = false;

    void Start()
    {
        // (Tùy chọn) Ẩn quái vật và rồng lúc mới vào game để chắc chắn chúng chưa xuất hiện
        if (QuaiVat != null) QuaiVat.SetActive(false);
        if (Dragon != null) Dragon.SetActive(false);

        // Gọi hàm HienMenuBoss sau 3 giây
        Invoke("HienMenuBoss", 3f);
    }

    void Update()
    {
        // Điều kiện: Chỉ nhận nút bấm khi MenuBoss đang hiển thị VÀ chưa chọn Boss lần nào
        if (menuBoss.activeInHierarchy && !daChonBoss)
        {
            // Nếu người dùng nhấn phím Q
            if (Input.GetKeyDown(KeyCode.Q))
            {
                daChonBoss = true;       // Đặt cờ thành true khóa lại, không cho bấm nữa
                QuaiVat.SetActive(true); // Bật Quái Vật

                // Tắt Menu Boss đi sau khi chọn xong
                menuBoss.SetActive(false);
            }
            // Nếu người dùng nhấn phím E
            else if (Input.GetKeyDown(KeyCode.E))
            {
                daChonBoss = true;       // Đặt cờ thành true khóa lại, không cho bấm nữa
                Dragon.SetActive(true);  // Bật Rồng

                // Tắt Menu Boss đi sau khi chọn xong
                menuBoss.SetActive(false);
            }
        }
    }

    void HienMenuBoss()
    {
        menuBoss.SetActive(true);
    }
}