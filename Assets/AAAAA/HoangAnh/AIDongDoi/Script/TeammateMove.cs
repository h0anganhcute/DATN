using UnityEngine;
using UnityEngine.AI;

public class TeammateMove : MonoBehaviour
{
    [Header("=== Cài đặt AI Né Tránh ===")]
    [Tooltip("Khoảng cách nguy hiểm: Rồng vào gần hơn mức này AI sẽ bỏ chạy")]
    public float safeDistance = 5f;

    [Tooltip("Khoảng cách cần duy trì: AI sẽ chạy đến khi cách rồng đủ mức này thì mới đứng lại bắn")]
    public float maintainDistance = 8f;

    [Tooltip("Tốc độ xoay người khi chạy/bắn")]
    public float rotationSpeed = 10f;

    [Header("=== Cài đặt Tấn Công ===")]
    public MonoBehaviour attack;

    private NavMeshAgent agent;
    private Transform nearestDragon;
    private bool isFleeing = false;

    // Timer giúp AI không bị "cà giật" do thay đổi điểm đến liên tục
    private float updatePathTimer = 0f;
    private float updatePathInterval = 0.5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("TeammateMove: Cần thêm component NavMeshAgent!");
            enabled = false;
            return;
        }

        // Đặt stoppingDistance = 0 để AI không bị khựng lại sớm
        agent.stoppingDistance = 0f;
    }

    void Update()
    {
        nearestDragon = FindNearestDragon();

        // Không có rồng -> Đứng yên và ngắm bắn
        if (nearestDragon == null)
        {
            if (isFleeing)
            {
                isFleeing = false;
                agent.ResetPath();
            }
            if (attack != null) attack.enabled = true;
            return;
        }

        float distanceToDragon = Vector3.Distance(transform.position, nearestDragon.position);

        // 1. Rồng ở quá gần -> Bỏ chạy
        if (distanceToDragon <= safeDistance || (isFleeing && distanceToDragon < maintainDistance))
        {
            SmartFlee(nearestDragon);
            if (attack != null) attack.enabled = false; // Tắt bắn khi đang cắm cổ chạy
        }
        // 2. Rồng đã ở xa -> Đứng lại xoay mặt và bắn
        else
        {
            if (isFleeing)
            {
                isFleeing = false;
                agent.ResetPath();
            }
            LookAtDragon(nearestDragon);
            if (attack != null) attack.enabled = true; // Bật bắn
        }
    }

    /// <summary>
    /// Chạy trốn thông minh: Biết tìm đường lách khi kẹt tường
    /// </summary>
    void SmartFlee(Transform dragon)
    {
        isFleeing = true;
        updatePathTimer -= Time.deltaTime;

        // Cứ mỗi 0.5s AI sẽ "suy nghĩ" tìm hướng chạy tốt nhất 1 lần
        if (updatePathTimer <= 0f)
        {
            updatePathTimer = updatePathInterval;

            // Hướng đối diện với rồng
            Vector3 dirFromDragon = (transform.position - dragon.position).normalized;

            // Các góc AI sẽ thử quét để tìm đường chạy (Thẳng lùi, chéo trái, chéo phải, đi ngang, lách tới)
            float[] escapeAngles = { 0f, 45f, -45f, 90f, -90f, 135f, -135f };

            Vector3 bestTarget = transform.position;
            float bestDist = 0f;

            foreach (float angle in escapeAngles)
            {
                // Tính toán hướng thoát hiểm
                Vector3 escapeDir = Quaternion.Euler(0, angle, 0) * dirFromDragon;
                Vector3 potentialTarget = transform.position + escapeDir * maintainDistance;

                NavMeshHit hit;
                // Kiểm tra xem vị trí đó có nằm trong map không (bán kính kiểm tra 3f)
                if (NavMesh.SamplePosition(potentialTarget, out hit, 3f, NavMesh.AllAreas))
                {
                    float distToDragon = Vector3.Distance(hit.position, dragon.position);

                    // Lưu lại điểm đến giúp AI cách xa rồng nhất
                    if (distToDragon > bestDist)
                    {
                        bestDist = distToDragon;
                        bestTarget = hit.position;
                    }
                }
            }

            // Ra lệnh di chuyển tới điểm an toàn nhất tìm được
            if (bestDist > 0)
            {
                agent.SetDestination(bestTarget);
            }
        }

        // Xoay mặt về hướng đang chạy (để dáng chạy tự nhiên)
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Vector3 moveDirection = agent.velocity.normalized;
            moveDirection.y = 0;
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            }
        }
    }

    void LookAtDragon(Transform dragon)
    {
        Vector3 directionToDragon = (dragon.position - transform.position).normalized;
        directionToDragon.y = 0;

        if (directionToDragon != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToDragon);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

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
}