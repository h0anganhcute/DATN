using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.AI;

public class RunLion : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    Animator ani;
    Health health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<Health>();
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
            //if (health != null && (health.CurrentHealth / health.MaxHealth) <= 0.5f)
            //{
            //    ani.SetBool("Walk", false);
            //    ani.SetTrigger("Down1");
            //    ani.SetTrigger("Down2");
            //    ani.SetTrigger("Down3");
            //    ani.SetBool("Run", true);

            //}
            //else
            //{
            //    ani.SetBool("Walk", true);
            //    ani.SetBool("Run", false);
            //}

            agent.SetDestination(player.position);
        }
    }
}
