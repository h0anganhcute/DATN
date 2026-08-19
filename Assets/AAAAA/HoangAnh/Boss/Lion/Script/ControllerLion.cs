using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.AI;

public class ControllerLion : MonoBehaviour
{
    RunLion run;
    NavMeshAgent AI;
    Animator ani;
    Health bossHealth;

    public Health playerHeal;

    public float ThoiGianDelayAnimation = 6f;
    public float ThoiGianDelayAnimation2 = 6f;
    public float ThoiGianDelayAnimation3 = 6f;

    private bool triggered90 = false;
    private bool triggered80 = false;
    private bool triggered70 = false;
    private bool triggered60 = false;
    private bool triggered50 = false;
    private bool triggered40 = false;
    private bool triggered30 = false;
    private bool triggered20 = false;
    private bool triggered10 = false;

    void Start()
    {
        AI = GetComponent<NavMeshAgent>();
        run = GetComponent<RunLion>();
        ani = GetComponent<Animator>();
        bossHealth = GetComponent<Health>();

    }

    void Update()
    {
        if (bossHealth == null) return;

        float healthRatio = bossHealth.CurrentHealth / bossHealth.MaxHealth;

        if (healthRatio <= 0.9f && !triggered90)
        {
            triggered90 = true;

            ani.SetTrigger("Skill2");
            ani.SetTrigger("GongSkill2");
            
        }
        else if (healthRatio <= 0.8f && !triggered80)
        {
            triggered80 = true;
            
        }
        else if (healthRatio <= 0.7f && !triggered70)
        {
            triggered70 = true;

        }
        else if (healthRatio <= 0.6f && !triggered60)
        {
            triggered60 = true;
            
        }
        else if (healthRatio <= 0.5f && !triggered50)
        {
            triggered50 = true;
            ani.SetTrigger("DonTho");
            ani.SetTrigger("ChuiLen");
        }
        else if (healthRatio <= 0.4f && !triggered40)
        {
            triggered40 = true;
            
        }
        else if (healthRatio <= 0.3f && !triggered30)
        {
            triggered30 = true;
            
        }
        else if (healthRatio <= 0.2f && !triggered20)
        {
            triggered20 = true;
            
        }
        else if (healthRatio <= 0.1f && !triggered10)
        {
            triggered10 = true;
            
        }
    }

    public IEnumerator PlaySkill2_KiemTraLienTuc()
    {
        ani.enabled = false;

        float timer = 0f;
        bool daVoHetPhaLe = false;

        // Liên tục kiểm tra trong lúc delay
        while (timer < ThoiGianDelayAnimation3)
        {
            GameObject[] crystals = GameObject.FindGameObjectsWithTag("CrystalClone");

            if (crystals.Length <= 0)
            {
                daVoHetPhaLe = true;
                break; // Pha lê đã vỡ hết -> Thoát khỏi vòng lặp delay ngay lập tức!
            }

            timer += Time.deltaTime; // Tăng thời gian đếm
            yield return null; // Đợi đến frame tiếp theo rồi kiểm tra tiếp
        }

        // Kết thúc vòng lặp delay (do hết giờ, hoặc do pha lê đã vỡ hết)
        ani.enabled = true;

        if (daVoHetPhaLe)
        {
            // Bị choáng do pha lê vỡ hết
            Debug.Log("=> Pha lê đã vỡ hết! Boss bị rơi vào trạng thái (Te)!");

            // --- THÊM CODE TRỪ 10% MÁU Ở ĐÂY ---
            if (bossHealth != null)
            {
                // Tính toán lượng sát thương bằng 10% máu tối đa
                float satThuong = bossHealth.MaxHealth * 0.1f;

                // Cách 1: Nếu script Health của bạn CÓ viết hàm nhận sát thương (ví dụ tên là TakeDamage)
                // Thì bạn bỏ comment dòng dưới đây và dùng nó (Khuyên dùng để UI, hiệu ứng chết hoạt động đúng):
                // bossHealth.TakeDamage(satThuong);

                // Cách 2: Trừ thẳng vào biến CurrentHealth (nếu script Health cho phép)
                bossHealth.CurrentHealth -= satThuong;
            }
            // ------------------------------------

            ani.SetTrigger("Te");
            ani.SetTrigger("Loop");
            ani.SetTrigger("Loop1");
            ani.SetTrigger("Loop2");
            ani.SetTrigger("Loop3");
            ani.SetTrigger("ThucDay");
        }
        else
        {
            ani.SetTrigger("XaSkill2");

           
        }
    }
    // --- CÁC HÀM KHÁC ---

    private void OnTriggerEnter(Collider other)
    {
        if (bossHealth != null && (bossHealth.CurrentHealth / bossHealth.MaxHealth) <= 0.5f)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                ani.SetTrigger("Attack");
            }
        }
    }

    public void TatDiChuyen()
    {
        run.enabled = false;
        AI.enabled = false;
    }

    public void BatDiChuyen()
    {
        AI.enabled = true;
        run.enabled = true;
    }
    public void tangToc()
    {
        AI.speed = 15f;
    }
    public void giamToc()
    {
        AI.speed = 0.1f;
    }
    public IEnumerator StopAnimation()
    {
        ani.enabled = false;
        yield return new WaitForSeconds(ThoiGianDelayAnimation);
        ani.enabled = true;
    }
    public IEnumerator StopAnimation2()
    {
        ani.enabled = false;
        yield return new WaitForSeconds(ThoiGianDelayAnimation2);
        ani.enabled = true;
    }
    public void DesTroyCloneCystal()
    {
        GameObject[] crystals = GameObject.FindGameObjectsWithTag("CrystalClone");
        foreach (GameObject crystal in crystals)
        {
            Destroy(crystal);
        }
    }
    public void TruMauPlayer()
    {
        // --- THÊM CODE TRỪ 30% MÁU CỦA PLAYER Ở ĐÂY ---
        if (playerHeal != null)
        {
            // Tính toán lượng sát thương bằng 30% máu tối đa của Player
            float satThuongPlayer = playerHeal.MaxHealth * 0.3f;

            // Cách 2: Trừ thẳng vào biến CurrentHealth
            playerHeal.CurrentHealth -= satThuongPlayer;

            Debug.Log("=> Hết thời gian mà pha lê chưa vỡ! Player bị trừ 30% máu.");
        }
        // -----------------------------------------------
    }
}