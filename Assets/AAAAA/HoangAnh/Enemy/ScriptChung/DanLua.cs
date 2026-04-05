using UnityEngine;

public class DanLua : MonoBehaviour
{
    [Header("Cài đặt Viên Đạn")]
    public float tocDo = 15f;
    public float thoiGianSong = 5f;

    [Tooltip("Tích vào nếu đây là đạn của QUÁI (Tự tìm Player để bắn). Bỏ tích nếu là đạn của PLAYER (Bay thẳng theo hướng nòng súng).")]
    public bool laDanCuaQuai = true;

    private Vector3 huongBay;

    void Start()
    {
        if (laDanCuaQuai)
        {
            // Nếu là đạn của quái: Tự tìm mặt Player mà phang
            GameObject nguoiChoi = GameObject.FindGameObjectWithTag("Player");
            if (nguoiChoi != null)
            {
                huongBay = (nguoiChoi.transform.position - transform.position).normalized;
                transform.LookAt(nguoiChoi.transform.position);
            }
            else
            {
                huongBay = transform.forward;
            }
        }
        else
        {
            // Nếu là đạn của Player: Cứ nòng súng chĩa hướng nào là bay thẳng hướng đó
            huongBay = transform.forward;
        }

        Destroy(gameObject, thoiGianSong);
    }

    void Update()
    {
        transform.position += huongBay * tocDo * Time.deltaTime;
    }
}