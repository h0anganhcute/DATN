using UnityEngine;
using UnityEngine.AI;

public class Skill3Target : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;

    private void Start()
    {
        // 1. Tự động tìm vị trí của Player (Yêu cầu Player phải được gắn Tag là "Player")
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy Object nào có Tag là 'Player' trong Scene!");
        }

        agent = GetComponent<NavMeshAgent>();

        // 2. Tắt tính năng tự động di chuyển và xoay của NavMeshAgent
        // Điều này giúp object KHÔNG di chuyển, chúng ta sẽ tự code phần xoay ở Update
        if (agent != null)
        {
            agent.updatePosition = false;
            agent.updateRotation = false;
        }
    }

    private void Update()
    {
        // Đảm bảo đã tìm thấy Player thì mới thực hiện xoay
        if (player != null)
        {
            // Tính toán hướng vector từ vị trí hiện tại đến vị trí của Player
            Vector3 direction = (player.position - transform.position).normalized;

            // Đặt Y = 0 để object chỉ xoay ngang (trái/phải), không bị ngẩng lên hay cúi xuống
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                // Xác định góc xoay mục tiêu
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // 3. Sử dụng thông số Angular Speed của NavMeshAgent (như bạn khoanh đỏ) làm tốc độ xoay
                float rotationSpeed = (agent != null) ? agent.angularSpeed : 120f;

                // Xoay mặt mượt mà về phía Player theo thời gian
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}