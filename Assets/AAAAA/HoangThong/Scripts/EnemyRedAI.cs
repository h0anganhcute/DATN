using UnityEngine;
using UnityEngine.AI; // Bắt buộc phải có thư viện này để dùng NavMeshAgent
using System.Collections;

public class EnemyRedAI : MonoBehaviour
{
    public enum RedState { Spawn, Fire1, Chasing, BasicAttack, Fire2, FlyUp, Dead }
    [Header("Trạng thái hiện tại")]
    public RedState currentState = RedState.Spawn;

    [Header("Cấu hình chung")]
    public Transform player;
    public float maxHealth = 150f;
    private float currentHealth;

    [Header("Hiệu ứng lửa (VFX Prefabs)")]
    public GameObject fireBallPrefab;    // Hiệu ứng 1 (Cục lửa)
    public GameObject fireStreamPrefab;  // Hiệu ứng 2 (Lửa chùm)
    public Transform mouthPoint;         // Vị trí ở miệng Red

    private Animator anim;
    private NavMeshAgent agent;          // Khai báo NavMeshAgent
    private int attackCount = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>(); // Lấy Component NavMeshAgent
        currentHealth = maxHealth;

        if (player == null)
            player = GameObject.FindWithTag("Player")?.transform;

        // Tắt tính năng tự xoay của NavMeshAgent để ta tự xoay bằng Code cho mượt, không bị khựng
        agent.updateRotation = false;

        // Bắt đầu chuỗi kịch bản tuần tự
        StartCoroutine(BossLogicRoutine());
    }

    void Update()
    {
        if (currentState == RedState.Dead || player == null) return;

        // Tính khoảng cách đến Player và đẩy lên Animator
        float distance = Vector3.Distance(transform.position, player.position);
        anim.SetFloat("distanceToPlayer", distance);

        // Xử lý xoay mặt: Luôn luôn khóa mục tiêu hướng về phía Player (Trừ lúc đang bay quá cao hoặc chết)
        if (currentState != RedState.Dead)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // Giữ quái đứng thẳng trên mặt sàn NavMesh
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
            }
        }
    }

    IEnumerator BossLogicRoutine()
    {
        // === BƯỚC 1: KHỞI ĐẦU (SPAWN & SCREAM) ===
        currentState = RedState.Spawn;
        agent.isStopped = true;          // Đứng yên tại chỗ để hét
        anim.SetInteger("bossState", 1);
        yield return new WaitForSeconds(1.2f);

        // === BƯỚC 2: PHUN CỤC LỬA 1 (FIREBALL SHOOT) ===
        currentState = RedState.Fire1;
        yield return new WaitForSeconds(0.4f); // Nhịp há mồm
        SpawnFireEffect(fireBallPrefab);
        yield return new WaitForSeconds(1.2f); // Chờ phun xong cự ly 1

        // Vòng lặp chiến đấu chính
        while (currentHealth > 0)
        {
            // === BƯỚC 3: ĐUỔI THEO (RUN) & ĐÁNH CẬN CHIẾN ===
            currentState = RedState.Chasing;
            anim.SetInteger("bossState", 2);
            agent.isStopped = false;         // Bật Agent cho phép chạy đuổi theo Player
            attackCount = 0;

            // Đuổi theo cho tới khi vung đủ 2 đòn Basic Attack thành công
            while (attackCount < 2)
            {
                // Ra lệnh cho NavMeshAgent liên tục bám đuổi theo vị trí của Player
                if (agent.isOnNavMesh)
                {
                    agent.SetDestination(player.position);
                }

                // Kiểm tra khoảng cách thực tế của Agent tới mục tiêu
                float distance = Vector3.Distance(transform.position, player.position);

                // Nếu đã vào tầm đánh cận chiến
                if (distance <= agent.stoppingDistance)
                {
                    currentState = RedState.BasicAttack;
                    agent.isStopped = true; // Dừng di chuyển lập tức để vung tay đánh, tránh bị trượt chân
                }
                else if (currentState == RedState.BasicAttack && distance > agent.stoppingDistance)
                {
                    // Nếu người chơi bỏ chạy ra xa khi đang đánh, bắt quái đuổi theo tiếp
                    currentState = RedState.Chasing;
                    agent.isStopped = false;
                }

                yield return null;
            }

            // === BƯỚC 4: PHUN LỬA CHÙM (TAIL ATTACK) ===
            currentState = RedState.Fire2;
            agent.isStopped = true;          // Dừng Agent để tập trung phun lửa chùm
            anim.SetInteger("bossState", 3);
            yield return new WaitForSeconds(0.5f);
            SpawnFireEffect(fireStreamPrefab);
            yield return new WaitForSeconds(2.0f);

            // === BƯỚC 5: BAY LÊN TRỜI (FLY UP) ===
            currentState = RedState.FlyUp;
            anim.SetInteger("bossState", 4);

            // LƯU Ý: Khi quái bay lên không trung, nó sẽ rời khỏi mặt sàn NavMesh. 
            // Ta cần tắt hẳn Agent đi để tránh lỗi Agent giật ngược quái xuống đất.
            agent.enabled = false;

            yield return new WaitForSeconds(4.0f); // Bay lượn khè lửa trên không trong 4 giây

            // === BƯỚC 6: RESET HẠ CÁNH VỀ IDLE ===
            anim.SetInteger("bossState", 0);
            yield return new WaitForSeconds(1.5f); // Chờ Land đáp đất xong xuôi

            // Đáp đất an toàn -> Bật lại NavMeshAgent để chuẩn bị đuổi theo Player tiếp
            agent.enabled = true;
            yield return null; // Chờ 1 khung hình để Agent ổn định lại trên NavMesh
        }
    }

    public void OnBasicAttackHit()
    {
        attackCount++;
        Debug.Log("NavMeshAI: Đòn đánh cận chiến số " + attackCount);
    }

    void SpawnFireEffect(GameObject prefab)
    {
        if (prefab != null && mouthPoint != null)
        {
            Instantiate(prefab, mouthPoint.position, mouthPoint.rotation);
        }
    }

    public void TakeDamage(float amount)
    {
        if (currentState == RedState.Dead) return;

        currentHealth -= amount;
        anim.SetTrigger("takeDamage");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        StopAllCoroutines();
        currentState = RedState.Dead;

        if (agent.isActiveAndEnabled)
            agent.isStopped = true; // Dừng di chuyển ngay khi chết

        anim.SetTrigger("isDead");
        Destroy(gameObject, 4f);
    }
}