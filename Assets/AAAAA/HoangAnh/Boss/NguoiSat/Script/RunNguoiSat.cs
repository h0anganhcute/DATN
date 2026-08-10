using UnityEngine;
using UnityEngine.AI;

public class RunNguoiSat : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    Animator ani;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && agent != null)
        {
            ani.SetBool("Walk", true);
            agent.SetDestination(player.position);
                      
        }
    }
}
