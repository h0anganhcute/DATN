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
        
        // 2. Gọi hàm "TamDungVFX" sau 1 giây
        Invoke("TamDungVFX",1f);
    }

    

    public void TamDungVFX()
    {
        if (vfx != null)
        {
            vfx.Pause();
        }
    }
}