using System.Collections.Generic;
using UnityEngine;

public class SkillAOE : MonoBehaviour
{
    public GameObject aoe;
    public Terrain terrain; // Reference to the Terrain component
    
    [Header("Cài đặt AOE")]
    [Tooltip("Số lượng AOE được tạo ra")]
    public int soLuongAoe = 15;
    
    [Tooltip("Khoảng cách tối thiểu giữa TÂM các AOE. TĂNG SỐ NÀY LÊN NẾU CÁC AOE VẪN BỊ ĐÈ LÊN NHAU")]
    public float khoangCachMin = 15f; 
    
    [Tooltip("Khoảng cách lề Terrain. Giúp các AOE luôn nằm trọn trong lòng Terrain, không bị lọt ra mép ngoài")]
    public float khoangCachLe = 5f;

    [Header("Khu vực xuất hiện")]
    [Tooltip("Nếu tích vào, sẽ lấy toàn bộ kích thước của Terrain làm khu vực xuất hiện. Nếu bỏ tích, sẽ dùng khu vực xung quanh Boss.")]
    public bool suDungKichThuocTerrain = true;
    public float chieuRongKhuVuc = 50f; // Trục X quanh Boss
    public float chieuDaiKhuVuc = 50f;  // Trục Z quanh Boss

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoiAoe()
    {
        if (aoe == null)
        {
            Debug.LogWarning("Chưa gán aoe trong Inspector!");
            return;
        }

        List<Vector3> viTriDaSpawn = new List<Vector3>();
        
        float startX = 0f;
        float startZ = 0f;
        float width = 0f;
        float length = 0f;
        
        // Tính toán giới hạn an toàn của Terrain (bên trong lòng Terrain, trừ đi lề an toàn)
        float tXMin = float.MinValue;
        float tXMax = float.MaxValue;
        float tZMin = float.MinValue;
        float tZMax = float.MaxValue;

        if (terrain != null)
        {
            tXMin = terrain.transform.position.x + khoangCachLe;
            tXMax = terrain.transform.position.x + terrain.terrainData.size.x - khoangCachLe;
            tZMin = terrain.transform.position.z + khoangCachLe;
            tZMax = terrain.transform.position.z + terrain.terrainData.size.z - khoangCachLe;
        }

        if (suDungKichThuocTerrain && terrain != null)
        {
            // Rải ngẫu nhiên trên toàn bộ Terrain
            startX = tXMin;
            startZ = tZMin;
            width = tXMax - tXMin;
            length = tZMax - tZMin;
        }
        else
        {
            // Rải ngẫu nhiên trong một khu vực xung quanh vị trí của Boss
            width = chieuRongKhuVuc;
            length = chieuDaiKhuVuc;
            startX = transform.position.x - (width / 2f);
            startZ = transform.position.z - (length / 2f);
            
            if (suDungKichThuocTerrain && terrain == null)
            {
                Debug.LogWarning("Chưa gán Terrain! Sẽ tự động chuyển sang sử dụng khu vực xung quanh Boss.");
            }
        }

        int attempts = 0; // Tránh lặp vô hạn
        int spawnedCount = 0;

        // Tăng số lần thử lên để có nhiều cơ hội tìm chỗ trống hơn khi khoangCachMin lớn
        while (spawnedCount < soLuongAoe && attempts < 2000)
        {
            attempts++;
            
            // Random vị trí X và Z
            float randomX = startX + Random.Range(0f, width);
            float randomZ = startZ + Random.Range(0f, length);
            
            // QUAN TRỌNG: Ép toạ độ phải nằm trọn trong lòng Terrain
            if (terrain != null)
            {
                randomX = Mathf.Clamp(randomX, tXMin, tXMax);
                randomZ = Mathf.Clamp(randomZ, tZMin, tZMax);
            }

            // Tính toán vị trí Y để hiển thị đúng trên mặt đất
            float yPos = transform.position.y; // Mặc định dùng độ cao Boss nếu không có Terrain
            if (terrain != null)
            {
                yPos = terrain.SampleHeight(new Vector3(randomX, 0, randomZ)) + terrain.transform.position.y;
            }

            Vector3 viTriMoi = new Vector3(randomX, yPos, randomZ);

            // Kiểm tra xem vị trí mới có nằm cách xa các vị trí đã tạo một khoảng >= khoangCachMin không
            bool hopLe = true;
            foreach (Vector3 pos in viTriDaSpawn)
            {
                // Tính khoảng cách trên mặt phẳng XZ (bỏ qua độ cao Y)
                float distance = Vector2.Distance(new Vector2(viTriMoi.x, viTriMoi.z), new Vector2(pos.x, pos.z));
                if (distance < khoangCachMin)
                {
                    hopLe = false;
                    break;
                }
            }

            // Nếu vị trí hợp lệ thì tiến hành clone
            if (hopLe)
            {
                Instantiate(aoe, viTriMoi, Quaternion.identity);
                viTriDaSpawn.Add(viTriMoi);
                spawnedCount++;
            }
        }

        if (spawnedCount < soLuongAoe)
        {
            Debug.LogWarning($"Chỉ có thể tạo {spawnedCount}/{soLuongAoe} AOE. Hãy thử GIẢM khoảng cách tối thiểu, hoặc TĂNG kích thước khu vực spawn.");
        }
    }
}
