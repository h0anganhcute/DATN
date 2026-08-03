using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Bắt buộc phải có dòng này để gọi được NavMeshAgent

public class DragonController : MonoBehaviour
{
    private Skill skill;

    [Header("Thời gian chờ giữa các chiêu")]
    public float cooldownTime = 5f;

    [Header("Thời gian đứng im khi tung chiêu")]
    public float skillDuration = 3f;

    // Khai báo NavMeshAgent
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        // Lấy component NavMeshAgent trên cùng GameObject
        navMeshAgent = GetComponent<NavMeshAgent>();
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
            yield return StartCoroutine(UseSkill1());

            // ================== LƯỢT SKILL 2 ==================
            yield return new WaitForSeconds(cooldownTime);
            yield return StartCoroutine(UseSkill2());

            // ================== LƯỢT SKILL 3 ==================
            yield return new WaitForSeconds(cooldownTime);
            yield return StartCoroutine(UseSkill3());

            // ================== LƯỢT SKILL 4 ==================
            yield return new WaitForSeconds(cooldownTime);
            yield return StartCoroutine(UseSkill4());
        }
    }

    private IEnumerator UseSkill1()
    {
        // Dừng NavMeshAgent di chuyển khi tung chiêu
       navMeshAgent.enabled = false;

        skill.Skill1();
        yield return new WaitForSeconds(skillDuration);

        // Cho phép NavMeshAgent tiếp tục di chuyển sau khi dùng xong chiêu
        navMeshAgent.enabled = true;
    }

    private IEnumerator UseSkill2()
    {
        navMeshAgent.enabled = false;

        skill.Skill2();
        yield return new WaitForSeconds(skillDuration);

        navMeshAgent.enabled = true;
    }

    private IEnumerator UseSkill3()
    {
        navMeshAgent.enabled = false;

        skill.Skill3();
        yield return new WaitForSeconds(skillDuration);

        navMeshAgent.enabled = true;
    }

    private IEnumerator UseSkill4()
    {
        navMeshAgent.enabled = false;

        skill.Skill4();
        yield return new WaitForSeconds(skillDuration);

        navMeshAgent.enabled = true;
    }
}