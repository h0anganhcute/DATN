using UnityEngine;

public class CloneFire : MonoBehaviour
{
    public GameObject firePrefab;

    // Dùng OnTriggerEnter thay vì OnCollisionEnter
    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem vật thể xuyên qua có phải là Terrain không
        Terrain terrain = other.GetComponent<Terrain>();

        if (terrain != null)
        {
            // Lấy toạ độ X, Z hiện tại của viên đạn khi vừa xuyên qua đất
            Vector3 spawnPoint = transform.position;

            // Phép thuật ở đây: Hỏi Terrain xem tại toạ độ X, Z này thì mặt đất cao bao nhiêu (trục Y)?
            // Cộng thêm terrain.transform.position.y để đề phòng bản thân cái Terrain không nằm ở toạ độ Y = 0
            spawnPoint.y = terrain.SampleHeight(transform.position) + terrain.transform.position.y;

            // Clone hiệu ứng lửa tại điểm đã được canh chuẩn trên mặt đất
            if (firePrefab != null)
            {
                GameObject fire = Instantiate(firePrefab, spawnPoint, Quaternion.identity);
            }          
        }
    }
}