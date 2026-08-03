using UnityEngine;
using UnityEngine.AI;
using Unity.FPS.Game; // Thêm dòng này để gọi được script Health

public class DieQuaiVat : MonoBehaviour
{
    NavMeshAgent navAgent;
    TargetBoss1 targetBoss1;
    SkillQuaiVat skillQuaiVat;
    Attack360 attack360;
    ControlQuaiVat controlQuaiVat;
    Animator ani;

    Health health; // Thêm biến chứa script Health
    bool isDead = false; // Biến đánh dấu để đảm bảo code chết chỉ chạy đúng 1 lần

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        targetBoss1 = GetComponent<TargetBoss1>();
        skillQuaiVat = GetComponent<SkillQuaiVat>();
        attack360 = GetComponent<Attack360>();
        controlQuaiVat = GetComponent<ControlQuaiVat>();
        ani = GetComponent<Animator>();

        health = GetComponent<Health>(); // Lấy script Health
    }

    void Update()
    {
        // Kiểm tra nếu quái có script Health, máu <= 0 và chưa bị đánh dấu là đã chết
        if (health != null && health.CurrentHealth <= 0 && isDead == false)
        {
            isDead = true; // Lập tức đánh dấu là đã chết để Frame sau không chạy lại các lệnh bên dưới nữa

            // Tắt NavMeshAgent (ngừng di chuyển)
            if (navAgent != null) navAgent.enabled = false;

            // Tắt các Script hành vi, tấn công
            if (targetBoss1 != null) targetBoss1.enabled = false;
            if (skillQuaiVat != null) skillQuaiVat.enabled = false;
            if (attack360 != null) attack360.enabled = false;
            if (controlQuaiVat != null) controlQuaiVat.enabled = false;

            // Gọi Animation chết
            if (ani != null) ani.SetTrigger("Die");

            // Hủy Game Object quái vật sau 5 giây
            Destroy(gameObject, 5f);
        }
    }
}