using UnityEngine;

public class ThoaiController : MonoBehaviour
{
    public GameObject thoaiCon1;
    public GameObject ChuongBao;
    public GameObject DienThoai;
    public GameObject ThoaiCon2;
    public GameObject CuaXo;
    public GameObject ThoaiCon3;

    bool isThoaiCon1Active = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("ThoaiCon1", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        // Kiểm tra nếu Điện Thoại đã được gán và đang ở trạng thái tắt
        if (DienThoai != null && !DienThoai.activeSelf)
        {
            // Kiểm tra Thoại Con 2 để đảm bảo chỉ bật 1 lần
            if (ThoaiCon2 != null && !ThoaiCon2.activeSelf)
            {
                ThoaiCon2.SetActive(true); // Bật Thoại Con 2
                isThoaiCon1Active = false; // Cập nhật cờ khi đã bật Thoại Con 2

                // Gọi hàm bật Cửa Sổ sau 6 giây
                Invoke("BatCuaXo", 6f);
            }
        }
    }

    public void ThoaiCon1()
    {
        if (thoaiCon1 != null)
        {
            thoaiCon1.SetActive(true);
            isThoaiCon1Active = true; // (Tuỳ chọn) Bạn có thể set thành true ở đây khi bật Thoại Con 1
        }
        Invoke("BatChuongBao", 3f);
    }

    private void BatChuongBao()
    {
        if (ChuongBao != null)
        {
            ChuongBao.SetActive(true);
            Invoke("TatChuongBao", 3f);
        }
    }

    private void TatChuongBao()
    {
        if (ChuongBao != null)
        {
            ChuongBao.SetActive(false);
        }
    }

    // Hàm dùng để bật Cửa Sổ
    private void BatCuaXo()
    {
        if (CuaXo != null)
        {
            CuaXo.SetActive(true);

            // Bật ThoaiCon3 ngay khi Cửa sổ được bật
            if (ThoaiCon3 != null)
            {
                ThoaiCon3.SetActive(true);
            }
        }
    }
}