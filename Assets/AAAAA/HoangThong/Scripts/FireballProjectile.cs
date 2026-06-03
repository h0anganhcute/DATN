using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    public float speed = 18f;      // Tốc độ bay của cầu lửa
    public float lifeTime = 3f;    // Thời gian tự hủy (tránh rác game)

    void Start()
    {
        // Tự động xóa cục lửa sau 3 giây nếu không trúng ai
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Liên tục bay thẳng về phía trước
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Nếu chạm vào Player thì gây sát thương và tự nổ
        if (other.CompareTag("Player"))
        {
            Debug.Log("Cầu lửa trúng Player!");
            // Gọi hàm trừ máu Player ở đây

            Destroy(gameObject); // Hủy cục lửa
        }
    }
}