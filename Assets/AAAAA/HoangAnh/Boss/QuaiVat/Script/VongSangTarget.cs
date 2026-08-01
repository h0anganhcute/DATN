using UnityEngine;

public class VongSangTarget : MonoBehaviour
{
    public float speed = 20f;
    Transform player;
    bool isStopped = false; // Biến kiểm tra xem đã dừng lại chưa
    public GameObject boom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Tự động tìm GameObject có tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        // Nếu đã tìm thấy Player và vòng sáng chưa bị yêu cầu dừng lại
        if (player != null && !isStopped)
        {
            // Di chuyển vị trí của Vòng Sáng đuổi theo vị trí của Player
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Khi chạm vào Player thì đánh dấu là đã dừng lại, không đuổi theo nữa
            isStopped = true;
            // Tìm TẤT CẢ object có tag "VongSang" và ẩn từng cái một
            System.Array.ForEach(GameObject.FindGameObjectsWithTag("VongSang"), vs => vs.SetActive(false));
            Invoke("phatno", 0.2f); // Gọi hàm phatno sau 0.2 giây
            Destroy(gameObject, 1.5f); // Hủy Vòng Sáng sau 1 giây
        }
    }
    void phatno()
    {
        boom.SetActive(true);
    }
    
}
