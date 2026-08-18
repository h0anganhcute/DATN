using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.AI;

public class ControllerLion : MonoBehaviour
{
    RunLion run;
    NavMeshAgent AI;
    Animator ani;
    Health bossHealth;

    public float ThoiGianDelayAnimation = 6f;
    public float ThoiGianDelayAnimation2 = 6f;

    private bool triggered90 = false;
    private bool triggered80 = false;
    private bool triggered70 = false;
    private bool triggered60 = false;
    private bool triggered50 = false;
    private bool triggered40 = false;
    private bool triggered30 = false;
    private bool triggered20 = false;
    private bool triggered10 = false;
    void Start()
    {
        AI = GetComponent<NavMeshAgent>();
        run = GetComponent<RunLion>();
        ani = GetComponent<Animator>();
        bossHealth = GetComponent<Health>();
    }

    void Update()
    {
        if (bossHealth == null) return;

        float healthRatio = bossHealth.CurrentHealth / bossHealth.MaxHealth;

        if (healthRatio <= 0.9f && !triggered90)
        {
            triggered90 = true;
            ani.SetTrigger("DonTho");
            ani.SetTrigger("ChuiLen");

        }
        else if (healthRatio <= 0.8f && !triggered80)
        {
            triggered80 = true;
            ani.SetTrigger("DonTho");
            ani.SetTrigger("ChuiLen");
        }
        else if (healthRatio <= 0.7f && !triggered70)
        {
            triggered70 = true;
            ani.SetTrigger("DonTho");
            ani.SetTrigger("ChuiLen");
        }
        else if (healthRatio <= 0.6f && !triggered60)
        {
            triggered60 = true;
            ani.SetTrigger("Skill2");
        }
        else if (healthRatio <= 0.5f && !triggered50)
        {
            triggered50 = true;
            
        }
        else if (healthRatio <= 0.4f && !triggered40)
        {
            triggered40 = true;
            ani.SetTrigger("Skill3");
        }
        else if (healthRatio <= 0.3f && !triggered30)
        {
            triggered30 = true;
            ani.SetTrigger("Skill4");
        }
        else if (healthRatio <= 0.2f && !triggered20)
        {
            triggered20 = true;
            ani.SetTrigger("Skill3");
        }
        else if (healthRatio <= 0.1f && !triggered10)
        {
            triggered10 = true;
            ani.SetTrigger("Skill4");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bossHealth != null && (bossHealth.CurrentHealth / bossHealth.MaxHealth) <= 0.5f)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                ani.SetTrigger("Attack");
            }
        }
    }

    public void TatDiChuyen()
    {
        run.enabled = false;
        AI.enabled = false;
    }

    public void BatDiChuyen()
    {
        AI.enabled = true;
        run.enabled = true;
    }
    public void tangToc()
    {
               AI.speed = 15f;
    }
    public void giamToc()
    {
        AI.speed = 0.1f;
    }
    public IEnumerator StopAnimation()
    {
        ani.enabled = false;
        yield return new WaitForSeconds(ThoiGianDelayAnimation);
        ani.enabled = true;
    }
    public IEnumerator StopAnimation2()
    {
        ani.enabled = false;
        yield return new WaitForSeconds(ThoiGianDelayAnimation2);
        ani.enabled = true;
    }

}
