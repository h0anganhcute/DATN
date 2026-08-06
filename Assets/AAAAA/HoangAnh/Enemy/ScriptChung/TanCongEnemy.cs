using UnityEngine;
using UnityEngine.AI;

public class TanCongEnemy : MonoBehaviour
{
    NavMeshAgent agent;
    Animator ani;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ani.SetTrigger("Attack"); 
        }
    }
    public void TatDiChuyen()
    {
        agent.enabled = false;
    }
    public void BatDiChuyen()
    {
        agent.enabled = true;
    }
}