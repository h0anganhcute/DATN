using Unity.FPS.Game;
using UnityEngine;

public class ControllerSkillBoss : MonoBehaviour
{
    Health bossHealth;
    Animator ani;

    private bool triggered90 = false;
    private bool triggered80 = false;
    private bool triggered70 = false;
    private bool triggered60 = false;
    private bool triggered50 = false;
    private bool triggered40 = false;
    private bool triggered30 = false;
    private bool triggered20 = false;
    private bool triggered10 = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossHealth = GetComponent<Health>();
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        if (bossHealth == null) return;

        float healthRatio = bossHealth.CurrentHealth / bossHealth.MaxHealth;

        if (healthRatio <= 0.9f && !triggered90)
        {
            triggered90 = true;
            if (ani != null) ani.SetTrigger("Attack1");
        }
        else if (healthRatio <= 0.8f && !triggered80)
        {
            triggered80 = true;
            ani.SetTrigger("BayAttack2");
            ani.SetTrigger("Attack2");
            ani.SetTrigger("DownAttack2");
        }
        else if (healthRatio <= 0.7f && !triggered70)
        {
            triggered70 = true;
            if (ani != null) ani.SetTrigger("Attack1");
        }
        else if (healthRatio <= 0.6f && !triggered60)
        {
            triggered60 = true;
            //Chưa điền Skill
        }
        else if (healthRatio <= 0.5f && !triggered50)
        {
            triggered50 = true;
            if (ani != null) ani.SetTrigger("Attack1");
        }
        else if (healthRatio <= 0.4f && !triggered40)
        {
            triggered40 = true;
            ani.SetTrigger("BayAttack2");
            ani.SetTrigger("Attack2");
            ani.SetTrigger("DownAttack2"); 
        }
        else if (healthRatio <= 0.3f && !triggered30)
        {
            triggered30 = true;
            if (ani != null) ani.SetTrigger("Attack1");
        }
        else if (healthRatio <= 0.2f && !triggered20)
        {
            triggered20 = true;
            //Chưa điền Skill
        }
        else if (healthRatio <= 0.1f && !triggered10)
        {
            triggered10 = true;
            if (ani != null) ani.SetTrigger("Attack1");
        }
    }
}
