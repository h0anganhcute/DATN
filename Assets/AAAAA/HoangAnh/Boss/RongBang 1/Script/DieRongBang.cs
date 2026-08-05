using Unity.FPS.Game;
using UnityEngine;

public class DieRongBang : MonoBehaviour
{
    Health health;
    RongBangController rongBangController;
    ControllerSkillBoss controllerSkillBoss;
    Animator ani;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<Health>();
        rongBangController = GetComponent<RongBangController>();
        controllerSkillBoss = GetComponent<ControllerSkillBoss>();
        ani = GetComponent<Animator>();
    }

    bool isDead = false;
    void Update()
    {
        // Nếu máu <= 0 và Boss chưa chết
        if (health.CurrentHealth <= 0f && !isDead)
        {
            isDead = true; // Đánh dấu là đã chết để không gọi đoạn code này nhiều lần

            // Tắt 2 Script
            if (rongBangController != null) rongBangController.enabled = false;
            if (controllerSkillBoss != null) controllerSkillBoss.enabled = false;

            // Chạy animation Die
            if (ani != null) ani.SetTrigger("Die");
        }
    }
}