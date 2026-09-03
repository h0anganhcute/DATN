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

    [SerializeField] private float cooldownBetweenSkills = 3f;

    void Start()
    {
        ani = GetComponent<Animator>();
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
        // Nếu cờ đang hạ (false), nghĩa là chưa chạy Coroutine
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
            StartCoroutine(TurnAndCast("Attack1"));

            yield return new WaitUntil(() => isAnimationDone == true);  
        }
    }

    // Gắn hàm này vào Animation Event ở frame cuối của mỗi clip animation
    public void OnSkillAnimationFinished()
    {
        isAnimationDone = true;
    }


    // ==========================================
    // KỊCH BẢN CHUNG (ĐỢI XOAY MẶT -> TUNG CHIÊU)
    // ==========================================
    private IEnumerator TurnAndCast(string triggerName)
    {
        // 1. Dùng YIELD RETURN để ép code phải ĐỨNG ĐỢI Coroutine RotateTowardsPlayer xoay mặt xong
        yield return StartCoroutine(RotateTowardsPlayer());

        // 2. Chờ xoay xong xuôi rồi thì mới chạy lệnh gọi Animator dưới đây
        if (ani != null)
        {
            ani.SetTrigger(triggerName);
        }
    }

    // ==========================================
    // HÀM ĐỘC LẬP: CHUYÊN XỬ LÝ VIỆC XOAY MẶT
    // ==========================================
    private IEnumerator RotateTowardsPlayer()
    {
        if (player == null) yield break; // Dừng nếu chưa có dữ liệu player

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            while (Quaternion.Angle(transform.rotation, targetRotation) > 5f)
            {
                directionToPlayer = player.position - transform.position;
                directionToPlayer.y = 0;

                if (directionToPlayer != Vector3.zero)
                {
                    targetRotation = Quaternion.LookRotation(directionToPlayer);
                }

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

                yield return null;
            }
        }
    }
}