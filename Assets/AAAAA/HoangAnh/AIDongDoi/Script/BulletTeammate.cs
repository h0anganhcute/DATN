using UnityEngine;

public class BulletTeammate : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 30f; // Tốc độ bay của đạn

    private Transform targetDragon;

    void Start()
    {
        // Tự động tìm gameObj có tag là Dragon ngay khi đạn vừa sinh ra
        GameObject dragon = GameObject.FindGameObjectWithTag("Dragonn");
        if (dragon != null)
        {
            targetDragon = dragon.transform;
        }

        // Hủy viên đạn sau 5 giây để tránh làm nặng game (trong trường hợp bắn trượt đạn bay mãi)
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        if (targetDragon != null)
        {
            // 1. Tính toán hướng đi từ viên đạn tới rồng
            Vector3 direction = (targetDragon.position - transform.position).normalized;

            // 2. Di chuyển viên đạn dần về phía rồng
            transform.position += direction * speed * Time.deltaTime;

            // 3. Xoay đầu viên đạn luôn hướng về phía rồng
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        else
        {
            // Nếu không tìm thấy rồng (hoặc rồng vừa bị tiêu diệt), đạn sẽ bay thẳng về phía trước
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}