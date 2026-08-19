using System.Collections;
using UnityEngine;

public class MenuSkill : MonoBehaviour
{
    private Animator ani;
    private bool isAnimationDone = false;
    [SerializeField] private float cooldownBetweenSkills = 3f;

    void Start()
    {
        ani = GetComponent<Animator>();
        StartCoroutine(SkillLoopRoutine());
    }

    IEnumerator SkillLoopRoutine()
    {
        while (true)
        {
            // 1. Đợi 3 giây trước khi đánh
            yield return new WaitForSeconds(cooldownBetweenSkills);

            // Đánh Skill 1 và báo là đang múa (false = chưa xong)
            isAnimationDone = false;
            ani.SetTrigger("BayLen");
            ani.SetTrigger("FlyAttack");
            ani.SetTrigger("DapXuong");
            // ĐỨNG LẠI ĐÂY CHỜ! Chờ cho đến khi Event gán isAnimationDone = true thì mới được chạy tiếp.
            // Dù animation dài 1s hay 10s thì code vẫn sẽ đợi chuẩn xác.
            yield return new WaitUntil(() => isAnimationDone == true);

            // ===================================
            // Sau khi Skill 1 đã MÚA XONG hoàn toàn, mới bắt đầu đếm 3 giây tiếp theo.
            yield return new WaitForSeconds(cooldownBetweenSkills);

            isAnimationDone = false;
            ani.SetTrigger("DonTho");
            ani.SetTrigger("ChuiLen");
            yield return new WaitUntil(() => isAnimationDone == true);

            // ... Tương tự cho Skill 3 ...
        }
    }

    public void OnSkillAnimationFinished()
    {
        isAnimationDone = true; // Event gán lại thành true (đã xong) để lệnh WaitUntil đi tiếp
    }
    // Gắn hàm này vào Animation Event ở frame cuối của mỗi clip animation
   
}