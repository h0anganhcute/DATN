using System.Collections;
using UnityEngine;

public class MenuSkill : MonoBehaviour
{
    private Animator ani;
    private bool isAnimationDone = false;

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
            ani.SetTrigger("BayLen");
            ani.SetTrigger("FlyAttack");
            ani.SetTrigger("DapXuong");

            yield return new WaitUntil(() => isAnimationDone == true);

            // ===================================
            yield return new WaitForSeconds(cooldownBetweenSkills);

            isAnimationDone = false;
            ani.SetTrigger("DonTho");
            ani.SetTrigger("ChuiLen");

            yield return new WaitUntil(() => isAnimationDone == true);
        }
    }

    // Gắn hàm này vào Animation Event ở frame cuối của mỗi clip animation
    public void OnSkillAnimationFinished()
    {
        isAnimationDone = true;
    }
}