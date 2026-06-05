using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    private Skill skill;

    [Header("Thời gian chờ giữa các chiêu")]
    public float cooldownTime = 5f;

    [Header("Thời gian đứng im khi tung chiêu")]
    public float skillDuration = 3f;

    private TargerBoss targetBoss;

    private void Start()
    {
        targetBoss = GetComponent<TargerBoss>();
        skill = GetComponent<Skill>();

        // Gọi Coroutine để bắt đầu vòng lặp kỹ năng
        StartCoroutine(SkillController());
    }

    private IEnumerator SkillController()
    {
        // Dùng vòng lặp while (true) để lặp lại liên tục quá trình này
        while (true)
        {
            // ================== LƯỢT SKILL 1 ==================
            yield return new WaitForSeconds(cooldownTime);

            // THÊM "yield return" VÀO ĐÂY ĐỂ VÒNG LẶP CHỜ USESKILL1 CHẠY XONG
            yield return StartCoroutine(UseSkill1());

            // ================== LƯỢT SKILL 2 ==================
            yield return new WaitForSeconds(cooldownTime);

            // THÊM "yield return" VÀO ĐÂY ĐỂ VÒNG LẶP CHỜ USESKILL2 CHẠY XONG
            yield return StartCoroutine(UseSkill2());
            // ================== LƯỢT SKILL 3 ==================
            yield return new WaitForSeconds(cooldownTime);

            yield return StartCoroutine(UseSkill3());

            yield return new WaitForSeconds(cooldownTime);

            yield return StartCoroutine(UseSkill4()); 

            yield return new WaitForSeconds(cooldownTime);
        }
    }

    private IEnumerator UseSkill1()
    {
        targetBoss.enabled = false;
        skill.Skill1();
        yield return new WaitForSeconds(skillDuration);
        targetBoss.enabled = true;
    }

    private IEnumerator UseSkill2()
    {
        targetBoss.enabled = false;
        skill.Skill2();
        yield return new WaitForSeconds(skillDuration);
        targetBoss.enabled = true;
    }
    private IEnumerator UseSkill3()
    {
        targetBoss.enabled = false;
        skill.Skill3();
        yield return new WaitForSeconds(skillDuration);
        targetBoss.enabled = true;
    }
    private IEnumerator UseSkill4()
    {
        targetBoss.enabled = false;
        skill.Skill4();
        yield return new WaitForSeconds(skillDuration);
        targetBoss.enabled = true;
    }
}