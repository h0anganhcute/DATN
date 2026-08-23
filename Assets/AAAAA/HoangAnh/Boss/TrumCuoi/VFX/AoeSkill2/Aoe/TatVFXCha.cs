using UnityEngine;

public class TatVFXCha : MonoBehaviour
{
    ParticleSystem _particle;

    // Dùng OnEnable thay vì Start để đảm bảo dùng Object Pooling (bật/tắt tái sử dụng) vẫn chạy đúng
    private void OnEnable()
    {
        _particle = GetComponent<ParticleSystem>();

        if (_particle != null)
        {
            // 1. Tắt chế độ sinh hạt (Cách viết chuẩn của Unity mới)
            var emission = _particle.emission;
            emission.enabled = false;

            // 2. Xóa các hạt lỡ bắn ra ở mili-giây đầu tiên, giữ nguyên các con (false)
            _particle.Clear(false);
        }
    }
}