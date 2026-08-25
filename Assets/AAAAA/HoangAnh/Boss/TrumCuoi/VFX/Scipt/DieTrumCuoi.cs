using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.AI;

public class DieTrumCuoi : MonoBehaviour
{
    public GameObject LightTim;
    Health healBoss;
    NavMeshAgent navMeshAgent;
    Skill3Target target;
    ComBoSkillTrum comBoSkillTrum;
    ControllerTrum controllerTrum;
    Animator ani;
    public GameObject DestroySkill;
    public GameObject DestroySkill3;
    void Start()
    {
        healBoss = GetComponent<Health>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GetComponent<Skill3Target>();
        comBoSkillTrum = GetComponent<ComBoSkillTrum>();
        controllerTrum = GetComponent<ControllerTrum>(); 
        ani = GetComponent<Animator>();
    }

    bool isDead = false;

    // Update is called once per frame
    void Update()
    {
        if (healBoss.CurrentHealth <= 0 && !isDead)
        {
            isDead = true;
            Destroy(DestroySkill);
            Destroy(DestroySkill3);
            if (navMeshAgent != null) navMeshAgent.enabled = false;
            if (target != null) target.enabled = false;
            if (comBoSkillTrum != null) comBoSkillTrum.enabled = false;
            if (controllerTrum != null) controllerTrum.enabled = false;

            if (ani != null) ani.SetTrigger("Die");
            LightTim.SetActive(true);
        }
    }
}
