using System.Collections;
using UnityEngine;

public class AttackTeammate : MonoBehaviour
{
    [Header("Settings")]
    public GameObject vfxAttack; // Prefab đạn sẽ bắn ra
    public GameObject checkPoint; // Vị trí đầu nòng súng để đạn bay ra

    [Header("Ammo & Fire Rate")]
    public int maxAmmo = 30; // Số lượng đạn tối đa
    public float reloadTime = 2f; // Thời gian nạp đạn (giây)
    public float fireRate = 0.2f; // Tốc độ bắn (giãn cách giữa 2 viên)

    private int currentAmmo;
    private bool isReloading = false;
    private float nextFireTime = 0f;

    // Biến để lưu trữ mục tiêu rồng
    private GameObject targetDragon;

    void Start()
    {
        // Bắt đầu với băng đạn đầy
        currentAmmo = maxAmmo;

        // Tìm rồng ngay khi bắt đầu game
        targetDragon = GameObject.FindGameObjectWithTag("Dragon");
    }

    void Update()
    {
        // Kiểm tra xem mục tiêu rồng còn tồn tại không
        if (targetDragon == null)
        {
            // Cố gắng tìm lại 1 lần nữa phòng khi có rồng mới xuất hiện
            targetDragon = GameObject.FindGameObjectWithTag("Dragon");

            // Nếu vẫn không tìm thấy (Rồng đã bị tiêu diệt hoàn toàn)
            if (targetDragon == null)
            {
                // Tự động tắt Script này đi (nhân vật sẽ ngừng bắn)
                this.enabled = false;
                return;
            }
        }

        // Nếu đang nạp đạn thì dừng, không làm gì cả
        if (isReloading)
        {
            return;
        }

        // Hết đạn thì bắt đầu gọi hàm nạp đạn
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Tự động bắn theo tốc độ (fireRate)
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Trừ 1 viên đạn mỗi lần bắn
        currentAmmo--;

        // Xác định vị trí bắn: dùng checkPoint nếu có, nếu chưa kéo vào thì dùng vị trí của object này
        Transform spawnTransform = checkPoint != null ? checkPoint.transform : transform;

        // Clone (sinh ra) đạn tại vị trí checkPoint
        // Việc bay và tìm mục tiêu sẽ do Script BulletTeammate gắn trên viên đạn tự lo
        Instantiate(vfxAttack, spawnTransform.position, spawnTransform.rotation);
    }

    IEnumerator Reload()
    {
        isReloading = true;

        // Chờ 2 giây
        yield return new WaitForSeconds(reloadTime);

        // Nạp đầy đạn lại và cho phép bắn tiếp
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}