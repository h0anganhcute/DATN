using UnityEngine;

public class DragonController : MonoBehaviour
{
    private Skill skill;
    // Biến dùng để đếm thời gian
    private float timer = 0f;
    private RunEnemy runEnemy;

    // Thời gian hồi chiêu (3f tương đương với 3 giây)
    [Header("Cooldown Settings")]
    public float cooldownTime = 3f;

    // THÊM BIẾN NÀY: Dùng để đánh dấu lượt (true = tới lượt chiêu 1, false = tới lượt chiêu 2)
    private int chieuTiepTheo = 1;
    private void Start()
    {
        runEnemy = GetComponent<RunEnemy>();
        skill = GetComponent<Skill>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= cooldownTime)
        {
            if (runEnemy != null) runEnemy.enabled = false;
            // KIỂM TRA BIẾN ĐẾM (Gần giống vòng lặp switch case)
            if (chieuTiepTheo == 1)
            {
                UseSkill_1();
                chieuTiepTheo = 2; // Tăng lên 2
            }
            else if (chieuTiepTheo == 2)
            {
                UseSkill_2();
                chieuTiepTheo = 1; // Reset về 1 để xoay vòng
            }

            // Giả sử có chiêu 3 thì viết thêm:
            // else if (chieuTiepTheo == 3) { UseSkill_3(); chieuTiepTheo = 1; }
            Invoke("EnableRunEnemy", 3f);
            timer = 0f;
        }
    }

    void UseSkill_1()
    {
        if (skill != null)
        {
            skill.Skill1();
        }
    }

    void UseSkill_2()
    {
        if (skill != null)
        {
            skill.Skill2();
        }
    }

    // Hàm này được gọi bởi lệnh Invoke ở trên
    void EnableRunEnemy()
    {
        if (runEnemy != null)
        {
            runEnemy.enabled = true; // Mở lại script RunEnemy
        }
    }
}