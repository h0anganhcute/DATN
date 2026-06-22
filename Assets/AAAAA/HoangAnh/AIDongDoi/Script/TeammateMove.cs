using UnityEngine;
using UnityEngine.AI;

public class TeammateMove : MonoBehaviour
{
    [Header("=== Cài đặt NavMeshAgent ===")]
    [Tooltip("Khoảng cách an toàn tối thiểu với Dragon")]
    public float safeDistance = 5f;

    [Tooltip("Khoảng cách chạy trốn (nhân với safeDistance)")]
    public float fleeMultiplier = 2f;

    [Tooltip("Tốc độ xoay người khi bỏ chạy (độ/giây)")]
    public float rotationSpeed = 10f;

    private NavMeshAgent agent;
    private Transform nearestDragon;
    private bool isFleeing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("TeammateMove: Cần thêm component NavMeshAgent vào GameObject này!");
            enabled = false;
            return;
        }

        // Đặt stoppingDistance bằng safeDistance
        agent.stoppingDistance = safeDistance;
    }

    void Update()
    {
        // Tìm Dragon gần nhất
        nearestDragon = FindNearestDragon();

        if (nearestDragon == null)
        {
            // Không có Dragon nào → đứng yên
            isFleeing = false;
            return;
        }

        float distanceToDragon = Vector3.Distance(transform.position, nearestDragon.position);

        // Nếu Dragon lại gần hơn safeDistance → bỏ chạy
        if (distanceToDragon <= safeDistance)
        {
            Flee(nearestDragon);
        }
        else
        {
            // Dragon ở xa → dừng lại, không cần chạy nữa
            if (isFleeing && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                isFleeing = false;
                agent.ResetPath();
            }

            // Nếu không còn đang bỏ chạy nữa thì mới xoay mặt về phía Dragon
            // (Tránh tình trạng vừa bỏ chạy vừa ngoái đầu lại nhìn)
            if (!isFleeing)
            {
                LookAtDragon(nearestDragon);
            }
        }
    }

    /// <summary>
    /// Bình thường: xoay mặt nhìn về phía Dragon
    /// </summary>
    void LookAtDragon(Transform dragon)
    {
        Vector3 directionToDragon = (dragon.position - transform.position).normalized;
        directionToDragon.y = 0; // Giữ nguyên trục Y

        if (directionToDragon != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToDragon);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Xoay người và chạy theo hướng ngược lại với Dragon
    /// </summary>
    void Flee(Transform dragon)
    {
        isFleeing = true;

        // Tính hướng chạy trốn = hướng ngược lại từ Dragon đến mình
        Vector3 fleeDirection = (transform.position - dragon.position).normalized;

        // Điểm đến = vị trí hiện tại + hướng chạy * khoảng cách chạy
        Vector3 fleeTarget = transform.position + fleeDirection * (safeDistance * fleeMultiplier);

        // Tìm điểm hợp lệ trên NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleeTarget, out hit, safeDistance * fleeMultiplier, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        // Xoay mặt về hướng chạy trốn (quay lưng lại Dragon)
        Vector3 lookDirection = fleeDirection;
        lookDirection.y = 0; // Giữ nguyên trục Y, không xoay lên/xuống

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Tìm Dragon gần nhất trong scene
    /// </summary>
    Transform FindNearestDragon()
    {
        GameObject[] dragons = GameObject.FindGameObjectsWithTag("Dragon");

        if (dragons.Length == 0) return null;

        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject dragon in dragons)
        {
            float dist = Vector3.Distance(transform.position, dragon.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closest = dragon.transform;
            }
        }

        return closest;
    }

    /// <summary>
    /// Vẽ Gizmos trong Editor để dễ debug
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Vòng tròn vàng = safeDistance (khoảng cách an toàn)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, safeDistance);

        // Vòng tròn đỏ = vùng chạy trốn
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, safeDistance * fleeMultiplier);
    }
}
