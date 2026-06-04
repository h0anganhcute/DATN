using UnityEngine;

public class DragonController : MonoBehaviour
{
    private Skill skill_1;
    Transform player;
    // Biến dùng để đếm thời gian
    private float timer = 0f;

    // Thời gian hồi chiêu (3f tương đương với 3 giây)
    [Header("Cooldown Settings")]
    public float cooldownTime = 3f;
    private void Start()
    {
        skill_1 = GetComponent<Skill>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }
    void Update()
    {
        // Khoá vị trí của Dragon vào vị trí của Player
        if (player != null)
        {
            transform.LookAt(player);
        }
        // Time.deltaTime là thời gian trôi qua giữa mỗi khung hình
        // Cộng dồn Time.deltaTime vào timer để đếm thời gian thực
        timer += Time.deltaTime;

        // Nếu thời gian đếm được lớn hơn hoặc bằng 3 giây
        if (timer >= cooldownTime)
        {
            UseSkill1();

            // Reset lại bộ đếm về 0 để bắt đầu đếm lại chu kỳ 3 giây mới
            timer = 0f;
        }
    }

    void UseSkill1()
    {
        if (skill_1 != null)
        {
            skill_1.CastSkill1();
        }
    }
}