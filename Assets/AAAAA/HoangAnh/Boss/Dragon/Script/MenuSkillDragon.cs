using System.Collections;
using UnityEngine;

public class MenuSkillDragon : MonoBehaviour
{
    private Animator ani;
    private bool isAnimationDone = false;
    public Transform player;
    public float turnSpeed = 10f;
    // Biến cờ để tránh spam liên tục trong Update
    private bool TranhSpamLienTucTrongUpdate = false;
    public RunRedDragon run;

    [SerializeField] private float cooldownBetweenSkills = 3f;

    void Start()
    {
        ani = GetComponent<Animator>();
        run = GetComponent<RunRedDragon>();
    }

    void OnEnable()
    {
        // Hạ cờ xuống để cho phép hàm Update kích hoạt lại Coroutine
        TranhSpamLienTucTrongUpdate = false;
        isAnimationDone = false;
    }

    // ==============================================================
    // THÊM HÀM NÀY ĐỂ FIX LỖI TẮT SCRIPT MÀ VẪN CHẠY
    // ==============================================================
    void OnDisable()
    {
        // Khi bỏ dấu tick tắt Script, ép buộc mọi Coroutine đang chạy ngầm phải CHẾT NGAY.
        StopAllCoroutines();
    }
    // ==============================================================

    void Update()
    {
        if (run != null && !run.enabled)
        {
            TurnTowardsPlayer();
        }

        if (!TranhSpamLienTucTrongUpdate)
        {
            // LẬP TỨC dựng cờ lên (true) để chặn spam
            TranhSpamLienTucTrongUpdate = true;
            StartCoroutine(SkillLoopRoutine());
        }
    }

    IEnumerator SkillLoopRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldownBetweenSkills);

            isAnimationDone = false;
            ani.SetTrigger("Attack1");

            yield return new WaitUntil(() => isAnimationDone == true);  
        }
    }

    // Gắn hàm này vào Animation Event ở frame cuối của mỗi clip animation
    public void OnSkillAnimationFinished()
    {
        isAnimationDone = true;
    }
    public void TurnTowardsPlayer()
    {
        if (player == null) return;
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f; // Giữ nguyên trục Y để tránh xoay lên/xuống
        if (directionToPlayer.sqrMagnitude > 0.01f) // Kiểm tra nếu khoảng cách đủ lớn
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }
}