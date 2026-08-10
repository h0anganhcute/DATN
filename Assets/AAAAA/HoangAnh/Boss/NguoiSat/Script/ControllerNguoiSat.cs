using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControllerNguoiSat : MonoBehaviour
{
    RunNguoiSat run;
    NavMeshAgent AI;
    Animator ani;
    bool ChoPhep = true;
    void Start()
    {
        AI = GetComponent<NavMeshAgent>();
        run = GetComponent<RunNguoiSat>();
        ani = GetComponent<Animator>();

        StartCoroutine(TungSkill1());
    }

    void Update()
    {
        
    }

    private IEnumerator TungSkill1()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            ani.SetTrigger("Skill1");
            yield return new WaitForSeconds(9f);
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
}
