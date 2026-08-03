using UnityEngine;
using UnityEngine.AI;

public class Attack360 : MonoBehaviour
{
    [Header("Cấu hình Đạn")]
    public GameObject bulletPrefab;
    public int numberOfBullets = 8;
    public float bulletSpeed = 15f;
    public float spawnRadius = 2f;
    public float heightOffset = 1.5f;

    [Header("Cấu hình Hồi chiêu")]
    public float cooldownTime = 3f;

    private float timer = 0f;
    private Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
        timer = cooldownTime; // Gán bằng cooldownTime để gọi chiêu ngay khi bắt đầu
        
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= cooldownTime)
        {
            timer = 0f;

            if (ani != null)
            {
                ani.SetTrigger("Attack360");
            }
        }
    }

    public void Fire360()
    {
        if (bulletPrefab == null) return;

        Vector3 spawnCenter = transform.position + Vector3.up * heightOffset;
        float angleStep = 360f / numberOfBullets;

        for (int i = 0; i < numberOfBullets; i++)
        {
            float currentAngle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0f, currentAngle, 0f);
            Vector3 direction = rotation * Vector3.forward;
            Vector3 spawnPos = spawnCenter + (direction * spawnRadius);

            GameObject bullet = Instantiate(bulletPrefab, spawnPos, rotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * bulletSpeed;
            }

            Destroy(bullet, 5f);
        }
    }
    
}