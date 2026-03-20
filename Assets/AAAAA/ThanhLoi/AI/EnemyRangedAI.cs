using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedAI : MonoBehaviour
{
    public Transform target;

    [Header("Range")]
    public float detectRange = 15f;
    public float attackRange = 8f;

    [Header("Attack")]
    public float fireRate = 1f;
    private float nextFireTime;

    [Header("Shoot")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Components")]
    public Animator animator;
    public NavMeshAgent agent;

    private PlayerHealth playerHealth;

    private enum EnemyState { Idle, Chase, Attack }
    private EnemyState currentState;

    void Start()
    {
        agent.stoppingDistance = attackRange - 0.2f;
        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        if (target == null)
        {
            ChangeState(EnemyState.Idle);
            return;
        }

        if (playerHealth == null)
            playerHealth = target.GetComponent<PlayerHealth>();

        if (playerHealth == null || playerHealth.GetCurrentHealth() <= 0)
        {
            ChangeState(EnemyState.Idle);
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > detectRange)
        {
            ChangeState(EnemyState.Idle);
        }
        else if (distance > attackRange)
        {
            ChangeState(EnemyState.Chase);
        }
        else
        {
            ChangeState(EnemyState.Attack);
        }


        StateUpdate();
    }

    void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (currentState)
        {
            case EnemyState.Idle:
                animator.SetBool("IsRun", false);
                animator.SetBool("Shoot", false);
                agent.isStopped = true;
                break;

            case EnemyState.Chase:
                animator.SetBool("IsRun", true);
                animator.SetBool("Shoot", false);
                agent.isStopped = false;
                break;

            case EnemyState.Attack:
                animator.SetBool("IsRun", false);
                animator.SetBool("Shoot", true);
                agent.isStopped = true;
                break;
        }
    }

    void StateUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Chase:
                agent.SetDestination(target.position);
                break;

            case EnemyState.Attack:
                AttackBehaviour();
                break;
        }
    }

    void AttackBehaviour()
    {

        Vector3 lookPos = target.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);

        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        Vector3 dir = (target.position - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<Bullets>().SetDirection(dir);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}