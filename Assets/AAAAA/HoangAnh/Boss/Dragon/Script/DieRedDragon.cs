using Unity.FPS.Game;
using UnityEngine;

public class DieRedDragon : MonoBehaviour
{
    RunRedDragon runRedDragon;
    RedController redController;
    Health health;
    Animator ani;
    Rigidbody rb;
    MenuSkillDragon menu;

    // Biến cờ để đánh dấu Boss đã chết, ngăn code chạy nhiều lần
    private bool isDead = false;

    void Start()
    {
        runRedDragon = GetComponent<RunRedDragon>();
        redController = GetComponent<RedController>();
        health = GetComponent<Health>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        menu = GetComponent<MenuSkillDragon>();
    }

    void Update()
    {
        if (health == null) return;

        // Nếu máu tụt xuống 0 hoặc âm, VÀ Boss chưa bị đánh dấu là đã chết
        if (health.CurrentHealth <= 0f && !isDead)
        {
            // Bật cờ "Đã chết" ngay lập tức để khoá đoạn code này lại, 
            // các frame sau sẽ không bị lọt vào đây nữa.
            isDead = true;
            menu.enabled = false;  // Tắt menu skill của Boss
            // 1. Tắt các kịch bản di chuyển và tấn công để Boss nằm im
            if (runRedDragon != null) runRedDragon.enabled = false;
            if (redController != null) redController.enabled = false;

            // 2. Tắt isKinematic và bật trọng lực để xác Boss bị kéo rớt xuống mặt đất
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;   // Kéo Boss rơi xuống 
            }

            // 3. Kích hoạt Animation ngã gục
            if (ani != null)
            {
                ani.SetTrigger("StartDie");
                ani.SetTrigger("Die");
            }

            // [!] CHÚ Ý: Đã xoá bỏ dòng lệnh tắt Collider ở đây. 
            // Vì nếu tắt Collider, xác con rồng sẽ rơi xuyên thủng qua mặt đất và biến mất luôn!

            // 4. Tiêu huỷ (xoá) Boss hoàn toàn khỏi màn chơi sau 5 giây 
            // (để có thời gian 5s cho animation chết chạy xong)
            Destroy(gameObject, 5f);
        }
    }
}