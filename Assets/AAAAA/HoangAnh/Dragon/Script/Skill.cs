using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skill : MonoBehaviour
{
    private Animator ani;
    private DashSkill dashSkill;
    private RunEnemy runEnemy;

    void Start()
    {
        ani = GetComponent<Animator>();
        dashSkill = GetComponent<DashSkill>();

        // Bắt buộc phải có dòng này để tìm thấy Script RunEnemy
        runEnemy = GetComponent<RunEnemy>();
    }

    void Update()
    {
        // KIỂM TRA LIÊN TỤC: Nếu script RunEnemy đang bật thì bắt buộc tắt DashSkill
        if (runEnemy != null && dashSkill != null)
        {
            if (runEnemy.enabled == true)
            {
                dashSkill.enabled = false;
            }
        }
    }

    //Skill 1: Flame Attack
    public void Skill1()
    {
        ani.SetTrigger("FlameAttack");
    }

    //Skill 2: Ice Attack
    public void Skill2()
    {
        ani.SetTrigger("TakeOff");
        ani.SetTrigger("FlyGlide");
        ani.SetTrigger("Land");
        dashSkill.enabled = true;
    }
}