using UnityEngine;
using UnityEngine.AI;
using Unity.FPS.Game; // Thêm dòng này để gọi được script Health

public class DieDragon : MonoBehaviour
{
    NavMeshAgent navAgent;
    TargerBoss targerBoss;
    Skill skill;
    BulletBoss bulletBoss;
    DragonController dragonController;
    Animator ani;

    Health mau; // Thêm biến chứa script Health
    bool isDead = false; // Biến đánh dấu để đảm bảo code chết chỉ chạy đúng 1 lần

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        targerBoss = GetComponent<TargerBoss>();
        skill = GetComponent<Skill>();
        bulletBoss = GetComponent<BulletBoss>();
        dragonController = GetComponent<DragonController>();
        ani = GetComponent<Animator>();

        mau = GetComponent<Health>(); // Lấy script Health
    }

    void Update()
    {
        // Kiểm tra nếu boss có script Health, máu <= 0 và chưa bị đánh dấu là đã chết
        if (mau != null && mau.CurrentHealth <= 0 && isDead == false)
        {
            isDead = true; // Lập tức đánh dấu là đã chết để Frame sau không chạy lại các lệnh bên dưới nữa

            Debug.Log("Boss is died");

            // Tắt NavMeshAgent (ngừng di chuyển)
            if (navAgent != null) navAgent.enabled = false;

            // Tắt các Script hành vi, tấn công
            if (targerBoss != null) targerBoss.enabled = false;
            if (skill != null) skill.enabled = false;
            if (bulletBoss != null) bulletBoss.enabled = false;
            if (dragonController != null) dragonController.enabled = false;

            // Gọi Animation chết
            if (ani != null) ani.SetTrigger("Die");

            // Hủy Game Object boss sau 5 giây
            Destroy(gameObject, 5f);
        }
    }
}