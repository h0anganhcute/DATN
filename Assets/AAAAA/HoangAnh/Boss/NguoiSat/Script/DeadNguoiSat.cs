using Unity.FPS.Game;
using UnityEngine;

public class DeadNguoiSat : MonoBehaviour
{
    Health health;
    ControllerNguoiSat controller;
    Animator ani;
    public GameObject Lion;
    public GameObject viTriXuatHien;
    BoxCollider box;
    // Thêm cờ đánh dấu đã chết để không chạy lặp lại logic trong Update
    private bool isDead = false;

    void Start()
    {
        health = GetComponent<Health>();
        controller = GetComponent<ControllerNguoiSat>();
        ani = GetComponent<Animator>();
        box = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // Nếu đã chết rồi thì không cần kiểm tra nữa
        if (isDead) return;

        // Kiểm tra lượng máu (tùy thuộc vào script Health của bạn, biến này có thể là CurrentHealth, currentHealth, máu...)
        // Ở đây mình ví dụ dùng thuộc tính CurrentHealth.
        if (health.CurrentHealth <= 0f)
        {
            HandleDeath();
        }
    }

    void HandleDeath()
    {
        // Đánh dấu là đã chết
        isDead = true;

        // Chạy animation "Die"
        if (ani != null)
        {
            ani.SetTrigger("Die");
            box.enabled = false;
        }

        // Tắt component controller
        if (controller != null)
        {
            controller.enabled = false;
        }

        // Hủy Boss sau 6 giây
        Destroy(gameObject, 10f);
    }
    public void TrieuHoiLion()
    {
        if (Lion != null && viTriXuatHien != null)
        {
            Lion.transform.position = viTriXuatHien.transform.position;
            // Đặt hướng mặt của Lion (trục Z) trùng với hướng trục X của viTriXuatHien
            Lion.transform.forward = viTriXuatHien.transform.right;
        }
        if (Lion != null)
        {
            Lion.SetActive(true);
        }
    }
}