using System.Collections;
using System.Collections.Generic; // Bắt buộc phải thêm thư viện này để dùng List<>
using UnityEngine;

public class MenuSkillTrum : MonoBehaviour
{
    private Animator ani;
    private bool isAnimationDone = false;

    // Biến cờ để tránh spam liên tục trong Update
    private bool TranhSpamLienTucTrongUpdate = false;

    public GameObject CauLua;
    public BoxCollider boxAOE;

    [SerializeField] private float cooldownBetweenSkills = 3f;

    [Header("--- Cấu Hình Cầu Lửa ---")]
    [Tooltip("Số lượng cầu lửa được tạo ra")]
    public int soLuongCauLua = 5;
    [Tooltip("Khoảng cách tối thiểu giữa các cầu lửa để không bị dính nhau")]
    public float khoangCachToiThieu = 3f;
    [Tooltip("Tốc độ bay của cầu lửa")]
    public float tocDoCauLua = 15f;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    void OnEnable()
    {
        // Hạ cờ xuống để cho phép hàm Update kích hoạt lại Coroutine
        TranhSpamLienTucTrongUpdate = false;
        isAnimationDone = false;
    }

    // ==============================================================
    // THÊM HÀM NÀY ĐỂ FIX LỖI TẮT SCRIPT MÀ VẪN CHẠY
    // ==============================================================
    void OnDisable()
    {
        // Khi bỏ dấu tick tắt Script, ép buộc mọi Coroutine đang chạy ngầm phải CHẾT NGAY.
        StopAllCoroutines();
    }
    // ==============================================================

    void Update()
    {
        // Nếu cờ đang hạ (false), nghĩa là chưa chạy Coroutine
        if (!TranhSpamLienTucTrongUpdate)
        {
            // LẬP TỨC dựng cờ lên (true) để chặn spam
            TranhSpamLienTucTrongUpdate = true;
            StartCoroutine(SkillLoopRoutine());
        }
    }

    IEnumerator SkillLoopRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldownBetweenSkills);

            isAnimationDone = false;
            ani.SetTrigger("Skill1");

            yield return new WaitUntil(() => isAnimationDone == true);

            yield return new WaitForSeconds(cooldownBetweenSkills);

            isAnimationDone = false;
            ani.SetTrigger("LangCauTuyet");

            yield return new WaitUntil(() => isAnimationDone == true);
        }
    }

    // Gắn hàm này vào Animation Event ở frame cuối của mỗi clip animation
    public void OnSkillAnimationFinished()
    {
        isAnimationDone = true;
    }

    public void LangCauLua()
    {
        if (CauLua == null || boxAOE == null)
        {
            Debug.LogWarning("Chưa gán Prefab Cầu Lửa hoặc BoxCollider AOE!");
            return;
        }

        // Lấy kích thước không gian của BoxCollider (World Space)
        Bounds bounds = boxAOE.bounds;

        // Trục X là chiều ngang, trục Z là chiều dọc (dựa theo ảnh)
        float startX = bounds.min.x; // Mép trái của box
        float endX = bounds.max.x;   // Mép phải của box
        float minZ = bounds.min.z;   // Mép dưới của box
        float maxZ = bounds.max.z;   // Mép trên của box
        float spawnY = bounds.center.y; // Độ cao Y đặt ở giữa Box

        List<Vector3> dsViTriSpawn = new List<Vector3>();
        int soLanThuToiDa = 100; // Tránh loop vô hạn nếu Box quá nhỏ mà khoảng cách lại quá to

        for (int i = 0; i < soLuongCauLua; i++)
        {
            Vector3 viTriRandom = Vector3.zero;
            bool timDuocViTri = false;
            int soLanThu = 0;

            // Thuật toán: Random vị trí và kiểm tra khoảng cách với các vị trí đã chọn
            while (!timDuocViTri && soLanThu < soLanThuToiDa)
            {
                float randomZ = Random.Range(minZ, maxZ);
                viTriRandom = new Vector3(startX, spawnY, randomZ);
                timDuocViTri = true;

                // Kiểm tra xem vị trí mới có bị sát với các vị trí cũ không
                foreach (Vector3 viTriCu in dsViTriSpawn)
                {
                    if (Vector3.Distance(viTriRandom, viTriCu) < khoangCachToiThieu)
                    {
                        timDuocViTri = false;
                        break;
                    }
                }
                soLanThu++;
            }

            // Nếu tìm được vị trí hợp lệ
            if (timDuocViTri)
            {
                dsViTriSpawn.Add(viTriRandom);

                // Clone cầu lửa tại vị trí ngẫu nhiên ở mép trái
                GameObject luaClone = Instantiate(CauLua, viTriRandom, Quaternion.identity);

                // Tính điểm đến: chạy thẳng sang mép phải, giữ nguyên trục Z của nó
                Vector3 diemDen = new Vector3(endX, spawnY, viTriRandom.z);

                // Quay mặt quả cầu lửa hướng về đích đến
                luaClone.transform.LookAt(diemDen);

                // Kích hoạt Coroutine di chuyển quả cầu lửa đó
                StartCoroutine(DiChuyenCauLua(luaClone, diemDen));
            }
            else
            {
                Debug.LogWarning($"Cầu lửa thứ {i + 1} không tìm được vị trí phù hợp. Thử giảm biến 'khoangCachToiThieu' xuống.");
            }
        }
    }

    // Coroutine giúp di chuyển riêng biệt từng Cầu Lửa
    IEnumerator DiChuyenCauLua(GameObject cauLuaClone, Vector3 diemDen)
    {
        // Chạy tới khi nào khoảng cách đến đích < 0.1
        while (cauLuaClone != null && Vector3.Distance(cauLuaClone.transform.position, diemDen) > 0.1f)
        {
            // Di chuyển thẳng
            cauLuaClone.transform.position = Vector3.MoveTowards(cauLuaClone.transform.position, diemDen, tocDoCauLua * Time.deltaTime);
            yield return null;
        }

        // Tới nơi thì tiêu hủy (Bạn có thể bỏ dòng này nếu muốn tự xóa bằng hiệu ứng nổ)
        if (cauLuaClone != null)
        {
            Destroy(cauLuaClone);
        }
    }
}