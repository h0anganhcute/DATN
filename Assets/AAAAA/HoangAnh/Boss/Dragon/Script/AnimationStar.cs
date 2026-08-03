using UnityEngine;
using UnityEngine.AI;

public class AnimationStar : MonoBehaviour
{
    Animator ani;
    public GameObject cameraBoss;
    StartBoss startBoss;
    NavMeshAgent navMeshAgent;
    TargetBoss1 targetBoss1;
    DragonController dragonController;
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("Start");
        ani.SetTrigger("Stop");
        startBoss = GetComponent<StartBoss>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        targetBoss1 = GetComponent<TargetBoss1>();
        dragonController = GetComponent<DragonController>();
    }


    void Update()
    {
        
    }
    public void tatCamera()
    {
               cameraBoss.SetActive(false);
    }
    public void batDau()
    {
        startBoss.enabled = true;
    }
    public void dung()
    {
        startBoss.enabled = false;
    }
    public void TatAI()
    {
        navMeshAgent.enabled = false;
    }
    public void batAI()
    {
               navMeshAgent.enabled = true;
    }
    public void batTarget()
    {
        targetBoss1.enabled = true;
    }
    public void batController()
    {
        dragonController.enabled = true;
    }
}
