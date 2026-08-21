using UnityEngine;

// Đảm bảo object chứa script này luôn phải có Component ParticleSystem
[ExecuteAlways] // Dòng này giúp script chạy được cả khi bạn CHƯA bấm nút Play game
[RequireComponent(typeof(ParticleSystem))]
public class StopVFX : MonoBehaviour
{
    private ParticleSystem vfx;

    void Start()
    {
        vfx = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        // 1. Thay đổi vị trí Y = 1.1 ngay khi bật lên hoặc clone ra
        Vector3 newPos = transform.position;
        newPos.y = 1.1f; // Chỉnh trục Y thành 1.1
        transform.position = newPos;

        // 2. Gọi hàm "TamDungVFX" sau 1 giây
        Invoke("TamDungVFX", 1f);
    }

    void OnDisable()
    {
        // Việc xoá lệnh chờ bằng CancelInvoke đặc biệt quan trọng 
        // khi dùng [ExecuteAlways] để tránh lỗi trong Editor
        CancelInvoke("TamDungVFX");
    }

    public void TamDungVFX()
    {
        if (vfx != null)
        {
            vfx.Pause();
        }
    }
}