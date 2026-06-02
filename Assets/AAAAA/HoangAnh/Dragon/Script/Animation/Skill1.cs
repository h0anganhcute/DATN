using UnityEngine;

public class Skill1 : MonoBehaviour
{
    Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();
       
    }

    // Tạo một hàm public để DragonController có thể gọi được
    public void CastSkill()
    {
        // 1. Kích hoạt Animation
        ani.SetTrigger("FlameAttack");

        // 2. Xoay mặt về phía Player
       
    }
}