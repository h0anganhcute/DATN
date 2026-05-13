using UnityEngine;

[ExecuteAlways] // Dòng này giúp script chạy được cả khi bạn CHƯA bấm nút Play game
[RequireComponent(typeof(ParticleSystem))]
public class AutoFire : MonoBehaviour
{
    private ParticleSystem vfx;

    private void Start()
    {
        vfx = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        if (vfx != null)
        {
            // Bật lại VFX mỗi khi GameObject được tick xanh
            vfx.Play(true);
        }
    }
}