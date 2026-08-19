using UnityEngine;

public class CloneCrystal : MonoBehaviour
{
    public BoxCollider box;
    public GameObject crystalPrefab;
    public int soLuongClone = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnCrystals()
    {
        // Kiểm tra xem đã gán box và prefab trên Inspector chưa để tránh lỗi
        if (box == null || crystalPrefab == null)
        {
            Debug.LogWarning("BoxCollider hoặc crystalPrefab chưa được gán trong Script CloneCrystal!");
            return;
        }

        // Lấy thông tin về giới hạn (Bounds) của BoxCollider trong không gian thế giới (World Space)
        Bounds bounds = box.bounds;

        for (int i = 0; i < soLuongClone; i++)
        {
            // Tính toán tọa độ X, Y, Z ngẫu nhiên nằm giữa giá trị min và max của BoxCollider
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);
            float randomZ = Random.Range(bounds.min.z, bounds.max.z);

            Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);

            // Sinh ra clone (Instantiate) tại vị trí ngẫu nhiên vừa tìm được
            Instantiate(crystalPrefab, randomPosition, Quaternion.identity);
        }
    }
}