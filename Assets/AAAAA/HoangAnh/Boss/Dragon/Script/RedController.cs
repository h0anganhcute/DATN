using System.Collections;
using Unity.FPS.Game;
using UnityEngine;

public class RedController : MonoBehaviour
{
    RunRedDragon runRedDragon;
    Health health;
    Animator ani;
    public Transform player;
    
    public float turnSpeed = 10f;
    private bool triggered80 = false;
    private bool triggered60 = false;
    private bool triggered40 = false;
    private bool triggered20 = false;
    public AudioSource sTart;
    

    void Start()
    {
        runRedDragon = GetComponent<RunRedDragon>();
        health = GetComponent<Health>();
        ani = GetComponent<Animator>();
        ani.SetTrigger("Start");
    }

    void Update()
    {
        //if (health == null) return;

        //float healthRatio = health.CurrentHealth / health.MaxHealth;

        //if (healthRatio <= 0.8f && !triggered80)
        //{
        //    triggered80 = true;
        //    ExecuteSkill1();
        //}
        //else if (healthRatio <= 0.6f && !triggered60)
        //{
        //    triggered60 = true;
        //    ExecuteSkill1();
        //}
        //// Giả sử mốc 40% bạn muốn xài Skill 2 (Ví dụ)
        //else if (healthRatio <= 0.4f && !triggered40)
        //{
        //    triggered40 = true;
        //    ExecuteSkill1(); // Đổi sang xài Skill 2
        //}
        //else if (healthRatio <= 0.2f && !triggered20)
        //{
        //    triggered20 = true;
        //    ExecuteSkill1();
        //}
    }

    // ==========================================
    // CÁC HÀM KÍCH HOẠT SKILL CỤ THỂ
    // ==========================================

    private void ExecuteSkill1()
    {
        if (runRedDragon != null) runRedDragon.enabled = false;
        // Bắt đầu quy trình truyền vào tên trigger là "Skill1"
        StartCoroutine(TurnAndCast("Attack1"));
    }

    // Mẫu Skill 2 để bạn dễ copy xài luôn
    private void ExecuteSkill2()
    {
        if (runRedDragon != null) runRedDragon.enabled = false;
        StartCoroutine(TurnAndCast("Skill2"));
    }


    // ==========================================
    // KỊCH BẢN CHUNG (ĐỢI XOAY MẶT -> TUNG CHIÊU)
    // ==========================================
    private IEnumerator TurnAndCast(string triggerName)
    {
        // 1. Dùng YIELD RETURN để ép code phải ĐỨNG ĐỢI Coroutine RotateTowardsPlayer xoay mặt xong
        yield return StartCoroutine(RotateTowardsPlayer());

        // 2. Chờ xoay xong xuôi rồi thì mới chạy lệnh gọi Animator dưới đây
        if (ani != null)
        {
            ani.SetTrigger(triggerName);
        }
    }

    // ==========================================
    // HÀM ĐỘC LẬP: CHUYÊN XỬ LÝ VIỆC XOAY MẶT
    // ==========================================
    private IEnumerator RotateTowardsPlayer()
    {
        if (player == null) yield break; // Dừng nếu chưa có dữ liệu player

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            while (Quaternion.Angle(transform.rotation, targetRotation) > 5f)
            {
                directionToPlayer = player.position - transform.position;
                directionToPlayer.y = 0;

                if (directionToPlayer != Vector3.zero)
                {
                    targetRotation = Quaternion.LookRotation(directionToPlayer);
                }

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

                yield return null;
            }
        }
    }
    public void BatLaiDiChuyen()
    {
        runRedDragon.enabled = true;
    }
    public void TatDiChuyen()
    {
        runRedDragon.enabled = false;
    }
    
    public void AudioStart()
    {
        sTart.Play();
    }
}