using Unity.FPS.Game;
using UnityEngine;

public class DieLion : MonoBehaviour
{
    Health health;
    Animator ani;
    ControllerLion controllerLion;
    MenuSkill skill;
    bool isDead = false;
    LionStart lionStart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = GetComponent<Health>();
        ani = GetComponent<Animator>();
        controllerLion = GetComponent<ControllerLion>();
        skill = GetComponent<MenuSkill>();
        lionStart = GetComponent<LionStart>();
    }

    // Update is called once per frame
    void Update()
    {
        // Kiểm tra xem máu <= 0 và con sư tử chưa được đánh dấu là đã chết
        if (health.CurrentHealth <= 0 && !isDead)
        {
            isDead = true;
            controllerLion.enabled = false;
            skill.enabled = false;
            lionStart.enabled = false;
            ani.SetTrigger("Die");
             
        }
    }
}