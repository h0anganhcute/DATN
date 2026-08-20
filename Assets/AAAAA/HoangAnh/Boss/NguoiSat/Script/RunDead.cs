using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.AI;

public class RunDead : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("LionClone");
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
