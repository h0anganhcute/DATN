using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [Header("Cài ??t C?a")]
    public float moveDistance = 2f;
    public float moveSpeed = 5f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool isOpen = false;       // C?a ?ang m? hay ?óng?
    private bool isPlayerNear = false; // Ng??i ch?i có ?ang ? g?n c?a không?

    void Start()
    {
        closedPosition = transform.position;

        // C?a di chuy?n theo tr?c Z
        openPosition = closedPosition + transform.forward * moveDistance;
    }

    void Update()
    {
        // Ki?m tra n?u ng??i ch?i ?ang ? g?n VÀ b?m phím E
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen; // ??o ng??c tr?ng thái m?/?óng
        }

        // C?a t? ??ng di chuy?n ??n v? trí ?ích m??t mà
        Vector3 targetPosition = isOpen ? openPosition : closedPosition;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    // Khi ng??i ch?i b??c vào vùng Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true; // Cho phép b?m E
        }
    }

    // Khi ng??i ch?i b??c ra kh?i vùng Trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false; // Không cho phép b?m E n?a
        }
    }
}