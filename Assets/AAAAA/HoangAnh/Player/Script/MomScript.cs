using UnityEngine;
using UnityEngine.AI;

public class MomScript : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Animator ani;
    MomRun momRun;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        momRun = GetComponent<MomRun>();
        ani = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("OpenDoor"))
        {
            ani.SetBool("Run", false);
            momRun.enabled = false;
            navMeshAgent.enabled = false;
            ani.SetTrigger("Open");
        }
    }
    public void BatLaiDiChuyen()
    {
        ani.SetBool("Run", true);
        momRun.enabled = true;
        navMeshAgent.enabled = true;
    }

}
