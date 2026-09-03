using UnityEngine;

public class MenuBoss : MonoBehaviour
{
    public GameObject menuBoss;
    public GameObject QuaiVat;
    public GameObject Dragon;
    public GameObject Boss;
    public GameObject panelCanhBao;
    public GameObject VuKhi;

    bool isBossDead = false;

    // Biến cờ (flag) để đảm bảo người chơi chỉ được chọn ở menu 1 lần duy nhất
    private bool daChonBoss = false;

    // Biến cờ để đảm bảo việc bật Boss cuối cùng chỉ diễn ra 1 lần
    private bool bossCuoiCungDaDuocGoi = false;

    void Start()
    {
        // Ẩn quái vật và rồng lúc mới vào game để chắc chắn chúng chưa xuất hiện
        if (QuaiVat != null) QuaiVat.SetActive(false);
        if (Dragon != null) Dragon.SetActive(false);

        // Gọi hàm HienMenuBoss sau 3 giây
        Invoke("HienMenuBoss", 3f);
    }

    void Update()
    {
        // ---------------------------------------------------------
        // PHẦN 1: CHỌN TỪ MENU
        // Chỉ nhận nút bấm khi MenuBoss đang hiển thị VÀ chưa chọn lần nào
        if (menuBoss.activeInHierarchy && !daChonBoss)
        {
            // Gộp chung điều kiện bấm Q hoặc E vì kết quả giống nhau
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
            {
                daChonBoss = true;       // Khóa chọn menu

                // Bật cả 2 Boss cùng lúc
                if (QuaiVat != null) QuaiVat.SetActive(true);
                if (Dragon != null) Dragon.SetActive(true);

                menuBoss.SetActive(false);
            }
        }

        // ---------------------------------------------------------
        // PHẦN 2: KIỂM TRA NẾU CẢ 2 BOSS ĐỀU BỊ DESTROY ĐỂ GỌI BOSS CUỐI
        if (daChonBoss == true && bossCuoiCungDaDuocGoi == false)
        {
            if (QuaiVat == null && Dragon == null)
            {
                bossCuoiCungDaDuocGoi = true; // Lập tức khóa cờ lại để tránh Invoke nhiều lần
                Invoke("BatPanelCanhBao", 5f);
                Invoke("BatBossCuoi", 7f);    // 5 giây bật panel + 2 giây sau đó bật boss = 7 giây
            }
        }

        // ---------------------------------------------------------
        // PHẦN 3: BẬT VŨ KHÍ KHI BOSS CUỐI BỊ TIÊU DIỆT
        if (Boss == null && !isBossDead)
        {
            isBossDead = true; // Đánh dấu là Boss cuối đã chết
            if (VuKhi != null) VuKhi.SetActive(true);
        }
    }

    void HienMenuBoss()
    {
        if (menuBoss != null) menuBoss.SetActive(true);
    }

    void BatPanelCanhBao()
    {
        if (panelCanhBao != null)
        {
            panelCanhBao.SetActive(true);
        }
    }

    void BatBossCuoi()
    {
        if (Boss != null)
        {
            Boss.SetActive(true);
        }
    }
}