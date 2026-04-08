using UnityEngine;
using System.Collections;

public class BossDieuKhien : MonoBehaviour
{
    Animator ani;

    [Header("Kết nối Script")]
    [Tooltip("Kéo cái Script 'RunBay' của con rồng vào ô này")]
    public MonoBehaviour DragonBay;

    void Start()
    {
        ani = GetComponent<Animator>();

        // 1. Khóa chân con rồng lại, không cho nó đi bậy
        if (DragonBay != null)
        {
            DragonBay.enabled = false;
        }

        // 2. Bắt đầu kịch bản
        StartCoroutine(KichBanBatDau());
    }

    IEnumerator KichBanBatDau()
    {
        // Gọi lệnh nhồi 1 đống Trigger để cho thằng Animator bắt đầu tự chuyển đổi trạng thái
        ChayAnimation();

        // BẮT BUỘC CÓ DÒNG NÀY: Chờ 1 frame để Animator kịp tiếp thu mấy cái Trigger mày vừa bắn
        yield return null;

        // --- CƠ CHẾ DÒ TÌM TỰ ĐỘNG ---
        // Vòng lặp này sẽ hỏi liên tục: "Ê mậy, đang ở trạng thái 'Bay' chưa?"
        // Chừng nào chưa tới cục có tên là "Bay", nó sẽ tiếp tục chờ.
        while (!ani.GetCurrentAnimatorStateInfo(0).IsName("Bay"))
        {
            yield return null;
        }

        // Thoát được vòng lặp ở trên nghĩa là nó ĐÃ CHÍNH THỨC BƯỚC VÀO TRẠNG THÁI "Bay"
        // 3. DIỄN XONG RỒI! Bật Script cho nó bay đi phá làng phá xóm
        if (DragonBay != null)
        {
            DragonBay.enabled = true;
            Debug.Log("Boss đã chuyển sang trạng thái Bay. Tiến lên!");
        }
    }

    public void ChayAnimation()
    {
        ani.SetTrigger("ThucDay");
        ani.SetTrigger("DungIm");
        ani.SetTrigger("CatCanh");
        ani.SetTrigger("BayTaiCho");
        ani.SetBool("Bay", true);
    }
}