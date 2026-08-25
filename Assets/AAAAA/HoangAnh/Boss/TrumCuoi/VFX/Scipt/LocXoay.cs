using UnityEngine;

public class LocXoay : MonoBehaviour
{
    [Tooltip("Lực hút của lốc xoáy")]
    public float pullForce = 5f;

    public GameObject Tam;

    void Start()
    {

    }

    void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Tam == null)
            {
                Debug.LogWarning("Bạn chưa kéo Object vào biến Tam trong Inspector của LocXoay!");
                return;
            }

            // Tính hướng từ Player đến vị trí của Tam
            Vector3 directionToCenter = (Tam.transform.position - other.transform.position).normalized;

            // Thử lấy CharacterController trước (vì Player của bạn dùng CharacterController)
            CharacterController cc = other.GetComponent<CharacterController>();

            if (cc != null)
            {
                // Dùng CharacterController.Move() để kéo Player về tâm
                cc.Move(directionToCenter * pullForce * Time.deltaTime);
            }
            else
            {
                // Dự phòng: Nếu không có CharacterController, thử Rigidbody
                Rigidbody playerRb = other.GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    playerRb.AddForce(directionToCenter * pullForce, ForceMode.Force);
                }
                else
                {
                    other.transform.position = Vector3.MoveTowards(other.transform.position, Tam.transform.position, pullForce * Time.deltaTime);
                }
            }
        }
    }
}