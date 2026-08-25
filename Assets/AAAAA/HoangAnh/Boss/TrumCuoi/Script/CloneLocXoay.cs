using UnityEngine;

public class CloneLocXoay : MonoBehaviour
{
    public GameObject LocXoay;
    public GameObject ViTriClone1;
    public GameObject ViTriClone2;
    public GameObject ViTriClone3;
    public GameObject ViTriClone4;
    public GameObject CanhBao;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Hàm thực hiện việc Clone
    public void SpawnLocXoay()
    {
        // Kiểm tra xem đã gán LocXoay chưa
        if (LocXoay == null)
        {
            Debug.LogWarning("Chưa gán GameObject LocXoay trong Inspector!");
            return; // Dừng hàm lại, không chạy tiếp nữa
        }

        // Đưa 4 vị trí vào một mảng để dễ dàng xử lý hàng loạt
        GameObject[] danhSachViTri = { ViTriClone1, ViTriClone2, ViTriClone3, ViTriClone4 };

        // Dùng vòng lặp chạy qua 4 vị trí
        for (int i = 0; i < danhSachViTri.Length; i++)
        {
            GameObject viTriHienTai = danhSachViTri[i];

            if (viTriHienTai != null)
            {
                // Lấy vị trí
                Vector3 spawnPosition = viTriHienTai.transform.position;

                // Cố định trục Y là 0.8
                spawnPosition.y = 0.8f;

                // Clone LocXoay
                Instantiate(LocXoay, spawnPosition, LocXoay.transform.rotation);
            }
            else
            {
                // Báo lỗi chính xác vị trí nào chưa được gán (i + 1 để in ra số 1, 2, 3, 4)
                Debug.LogWarning($"Chưa gán GameObject ViTriClone{i + 1} trong Inspector!");
            }
        }
    }
    public void OnCanhBao()
    {
        CanhBao.SetActive(true);
    }
    public void TatCanhBao()
    {
        CanhBao.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Không cần dùng Update cho việc này
    }
}