using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Range")]
    public float detectRange = 6f;
    public float attackRange = 2f;

    [Header("Attack")]
    public int damage = 10;
    public float attackCooldown = 1.2f;
    private float nextAttackTime;

    [Header("Components")]
    public Animator animator;
    public NavMeshAgent agent;

    private PlayerHealth playerHealth;

    void Start()
    {
        agent.stoppingDistance = attackRange - 0.1f;
    }

    void Update()
    {
        if (target == null)
        {
            GoIdle();
            return;
        }

        if (playerHealth == null)
            playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth == null || playerHealth.GetCurrentHealth() <= 0)
        {
            ClearTarget();
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange)
        {
            Attack();
        }
        else if (distance <= detectRange)
        {
            Chase();
        }
        else
        {
            GoIdle();
        }
    }

    void Chase()
    {
        animator.SetBool("IsRun", true);
        animator.SetBool("IsAttack", false);

        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    void Attack()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        animator.SetBool("IsRun", false);
        animator.SetBool("IsAttack", true);

        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            DealDamage();
        }
    }

    void DealDamage()
    {
        if (playerHealth == null) return;

        playerHealth.TakeDamage(damage);
        Debug.Log("[Enemy] Đánh Player");
    }

    void GoIdle()
    {
        animator.SetBool("IsRun", false);
        animator.SetBool("IsAttack", false);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
    }

    void ClearTarget()
    {
        Debug.Log("[Enemy] Player chết → Idle");

        target = null;
        playerHealth = null;
        GoIdle();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}