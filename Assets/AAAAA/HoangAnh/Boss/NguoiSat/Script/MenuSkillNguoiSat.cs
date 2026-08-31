using System.Collections;
using UnityEngine;

public class MenuSkillNguoiSat : MonoBehaviour
{
    private Animator ani;
    private bool isAnimationDone = false;

    // Biến cờ để tránh spam liên tục trong Update
    private bool TranhSpamLienTucTrongUpdate = false;

    [SerializeField] private float cooldownBetweenSkills = 3f;

    public AudioSource audioSkill1;
    public AudioSource audioSkill2;
    public AudioSource audioLuot;

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
            ani.SetTrigger("Skill1");

            yield return new WaitUntil(() => isAnimationDone == true);

            // ===================================
            yield return new WaitForSeconds(cooldownBetweenSkills);

            isAnimationDone = false;
            ani.SetTrigger("Skill2");

            yield return new WaitUntil(() => isAnimationDone == true);
            // ===================================
        }
    }

    // Gắn hàm này vào Animation Event ở frame cuối của mỗi clip animation
    public void OnSkillAnimationFinished()
    {
        isAnimationDone = true;
    }
    public void AudioSkill1()
    {
        audioSkill1.Play();
    }
    public void AudioSkill2()
    {
        audioSkill2.Play();
    }
    public void AudioLuot()
    {
        audioLuot.Play();
    }
}