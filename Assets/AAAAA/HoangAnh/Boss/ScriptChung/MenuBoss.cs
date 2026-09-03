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
    // Biến cờ (flag) để đảm bảo người chơi chỉ được chọn Boss ở menu 1 lần duy nhất
    private bool daChonBoss = false;

    // --- CÁC BIẾN MỚI THÊM ---
    // Biến lưu lại xem người chơi đã chọn Boss nào lúc đầu (1: Quái vật, 2: Rồng)
    private int bossDaChon = 0;
    // Biến cờ để đảm bảo việc bật boss thứ 2 chỉ diễn ra 1 lần duy nhất, không lặp lại
    private bool bossThuHaiDaDuocGoi = false;
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
        // PHẦN 1: CHỌN BOSS TỪ MENU
        // Chỉ nhận nút bấm khi MenuBoss đang hiển thị VÀ chưa chọn Boss lần nào
        if (menuBoss.activeInHierarchy && !daChonBoss)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                daChonBoss = true;       // Khóa chọn menu
                bossDaChon = 1;          // Ghi nhớ là đã chọn Quái Vật

                if (QuaiVat != null) QuaiVat.SetActive(true); // Bật Quái Vật
                menuBoss.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                daChonBoss = true;       // Khóa chọn menu
                bossDaChon = 2;          // Ghi nhớ là đã chọn Rồng

                if (Dragon != null) Dragon.SetActive(true);  // Bật Rồng
                menuBoss.SetActive(false);
            }
        }

        // ---------------------------------------------------------
        // PHẦN 2: KIỂM TRA BOSS CHẾT ĐỂ GỌI BOSS CÒN LẠI SAU 5 GIÂY
        // Chỉ chạy nếu đã chọn boss đầu tiên rồi VÀ boss thứ 2 chưa được gọi
        if (daChonBoss == true && bossThuHaiDaDuocGoi == false)
        {
            // Trường hợp 1: Ban đầu chọn Quái Vật (Q), và Quái Vật đã bị Destroy
            if (bossDaChon == 1 && QuaiVat == null)
            {
                bossThuHaiDaDuocGoi = true; // Lập tức khóa cờ lại để Frame sau không chạy nữa
                Invoke("BatRong", 5f);      // Chờ 5 giây rồi gọi hàm Bật Rồng
            }
            // Trường hợp 2: Ban đầu chọn Rồng (E), và Rồng đã bị Destroy
            else if (bossDaChon == 2 && Dragon == null)
            {
                bossThuHaiDaDuocGoi = true; // Lập tức khóa cờ lại để Frame sau không chạy nữa
                Invoke("BatQuaiVat", 5f);   // Chờ 5 giây rồi gọi hàm Bật Quái Vật
            }
        }

        // ---------------------------------------------------------
        // PHẦN 3: KIỂM TRA NẾU CẢ 2 BOSS ĐỀU BỊ DESTROY ĐỂ GỌI BOSS CUỐI
        if (daChonBoss == true && bossCuoiCungDaDuocGoi == false)
        {
            if (QuaiVat == null && Dragon == null)
            {
                bossCuoiCungDaDuocGoi = true; // Lập tức khóa cờ lại để tránh Invoke nhiều lần
                Invoke("BatPanelCanhBao", 5f);
                Invoke("BatBossCuoi", 7f);    // 5 giây bật panel + 2 giây sau đó bật boss = 7 giây
            }
        }
        if (Boss == null && !isBossDead)
        {
            isBossDead = true; // Đánh dấu là Boss cuối đã chết
            VuKhi.SetActive(true);
        }
    }

    void HienMenuBoss()
    {
        menuBoss.SetActive(true);
    }

    // --- CÁC HÀM MỚI THÊM ĐỂ INVOKE SAU 5 GIÂY ---
    void BatRong()
    {
        // Chắc chắn Rồng chưa bị ai Destroy mất trước khi bật
        if (Dragon != null)
        {
            Dragon.SetActive(true);
        }
    }

    void BatQuaiVat()
    {
        // Chắc chắn Quái Vật chưa bị ai Destroy mất trước khi bật
        if (QuaiVat != null)
        {
            QuaiVat.SetActive(true);
        }
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
