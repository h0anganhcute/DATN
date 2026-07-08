using UnityEngine;
using Unity.FPS.Game;
using UnityEngine.AI; // Bắt buộc phải có dòng này để gọi NavMeshAgent

public class DieDragon : MonoBehaviour
{
    private Health mau;
    private Animator ani;
    private TargerBoss targerBoss;
    private DragonController dragonController;
    private Skill skill;
    private BulletBoss bulletBoss;
    private NavMeshAgent navAgent; // Thêm biến chứa NavMeshAgent

    void Start()
    {
        ani = GetComponent<Animator>();
        mau = GetComponent<Health>();
        targerBoss = GetComponent<TargerBoss>();
        dragonController = GetComponent<DragonController>();
        skill = GetComponent<Skill>();
        bulletBoss = GetComponent<BulletBoss>();
        navAgent = GetComponent<NavMeshAgent>(); // Tìm component NavMeshAgent

        // Ở Start, CHỈ đăng ký: "Khi nào hết máu thì hãy gọi hàm KhiChet nhé!"
        if (mau != null)
        {
            mau.OnDie += KhiChet;
        }
    }

    // Hàm này sẽ tự động được gọi khi Health thông báo máu đã về 0
    void KhiChet()
    {
        Debug.Log("Boss is died");

        // 1. Chạy hoạt ảnh chết
        if (ani != null)
        {
            ani.SetTrigger("Die");
        }

        // 2. TẮT TÌM ĐƯỜNG NGAY LẬP TỨC (Boss sẽ ngừng trượt/di chuyển)
        if (navAgent != null)
        {
            navAgent.enabled = false;
        }

        // 3. Tắt toàn bộ AI và kỹ năng để boss nằm im
        if (targerBoss != null) targerBoss.enabled = false;
        if (dragonController != null) dragonController.enabled = false;
        if (skill != null) skill.enabled = false;
        if (bulletBoss != null) bulletBoss.enabled = false;

        // 4. Chờ 5 giây cho boss diễn xong cảnh chết rồi xóa xác khỏi bản đồ
        Destroy(gameObject, 5f);
    }
}