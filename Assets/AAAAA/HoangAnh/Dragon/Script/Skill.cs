using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skill : MonoBehaviour
{
    private Animator ani;
    private BanXaAttack banXaAttack;
    void Start()
    {
        ani = GetComponent<Animator>();
        banXaAttack = GetComponent<BanXaAttack>();
    }

    void Update()
    {
        // KIỂM TRA LIÊN TỤC: Nếu script RunEnemy đang bật thì bắt buộc tắt DashSkill
        
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
    }
    public void Skill3()
    {
        ani.SetTrigger("ClawAttack");
    }
    public IEnumerator Skill4()
    {
        banXaAttack.enabled = true;
        yield return new WaitForSeconds(1.5f);
        banXaAttack.enabled = false;
    }
}