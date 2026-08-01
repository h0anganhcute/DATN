using UnityEngine;
using UnityEngine.AI; // Cần thêm thư viện này để dùng NavMeshAgent
using Unity.FPS.Game;

public class SkillQuaiVat : MonoBehaviour
{
    Animator ani;
    Health health;
    NavMeshAgent navAgent; // Khai báo NavMeshAgent

    // Các biến đánh dấu để skill chỉ gọi 1 lần khi đạt mốc máu
    bool triggered80 = false;
    bool triggered60 = false;
    bool triggered40 = false;
    bool triggered20 = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        health = GetComponent<Health>();
        navAgent = GetComponent<NavMeshAgent>(); // Lấy component NavMeshAgent

        if (health != null)
        {
            // Đăng ký sự kiện khi máu thay đổi
            health.OnDamaged += OnTakeDamage;
            health.OnHealed += OnHealed;
        }
    }

    void OnDestroy()
    {
        if (health != null)
        {
            // Gỡ sự kiện khi script bị hủy để tránh rò rỉ bộ nhớ
            health.OnDamaged -= OnTakeDamage;
            health.OnHealed -= OnHealed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTakeDamage(float damage, GameObject damageSource)
    {
        CheckHealthThresholds();
    }

    void OnHealed(float amount)
    {
        if (health == null) return;
        
        // Tính toán lại phần trăm máu, nếu máu hồi lên trên mức quy định thì reset lại cờ
        float hpPercentage = health.GetRatio();
        if (hpPercentage > 0.8f) triggered80 = false;
        if (hpPercentage > 0.6f) triggered60 = false;
        if (hpPercentage > 0.4f) triggered40 = false;
        if (hpPercentage > 0.2f) triggered20 = false;
    }

    void CheckHealthThresholds()
    {
        if (health == null) return;

        // Lấy tỷ lệ máu (từ 0.0 đến 1.0) không quan trọng lượng máu tối đa là bao nhiêu
        float hpPercentage = health.GetRatio();
        bool shouldTrigger = false;

        // Kiểm tra mốc 80%
        if (hpPercentage <= 0.8f && !triggered80)
        {
            triggered80 = true;
            shouldTrigger = true;
        }
        // Kiểm tra mốc 60%
        if (hpPercentage <= 0.6f && !triggered60)
        {
            triggered60 = true;
            shouldTrigger = true;
        }
        // Kiểm tra mốc 40%
        if (hpPercentage <= 0.4f && !triggered40)
        {
            triggered40 = true;
            shouldTrigger = true;
        }
        // Kiểm tra mốc 20%
        if (hpPercentage <= 0.2f && !triggered20)
        {
            triggered20 = true;
            shouldTrigger = true;
        }

        // Nếu đạt mốc bất kỳ, thì gọi hàm Skill1() 1 lần duy nhất trong nhịp sát thương này
        if (shouldTrigger)
        {
            Skill1();
        }
    }

    void Skill1()
    {
        ani.SetTrigger("Boom");

        // Tắt di chuyển của NavMeshAgent khi dùng skill
        if (navAgent != null)
        {
            navAgent.isStopped = true; // Dừng di chuyển ngay lập tức
            navAgent.enabled = false;  // Tắt component NavMeshAgent
        }
    }

    // Hàm này để bật lại di chuyển (bạn có thể gọi hàm này từ Animation Event khi animation skill kết thúc)
    public void EnableMovement()
    {
        if (navAgent != null)
        {
            navAgent.enabled = true;
            navAgent.isStopped = false;
        }
    }
}
