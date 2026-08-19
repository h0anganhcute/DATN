using Unity.FPS.Game;
using UnityEngine;

public class DieCrystal : MonoBehaviour
{
    Health health;

    // Thêm cờ đánh dấu đã chết để không chạy lặp lại logic trong Update
    private bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        // Nếu đã chết rồi thì không cần kiểm tra nữa
        if (isDead) return;

        // Kiểm tra lượng máu bằng thuộc tính CurrentHealth giống y như DeadNguoiSat
        if (health != null && health.CurrentHealth <= 0f)
        {
            HandleDeath();
        }
    }

    void HandleDeath()
    {
        // Đánh dấu là đã chết
        isDead = true;

        // Hủy GameObject ngay lập tức thay vì đợi thời gian như Boss
        Destroy(gameObject);
    }
}