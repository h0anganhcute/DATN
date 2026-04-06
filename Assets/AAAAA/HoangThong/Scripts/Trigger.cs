using UnityEngine;

public class Trigger : MonoBehaviour
{
    [Header("Điểm di chuyển")]
    public Transform topPoint;          // Điểm đích phía trên

    [Header("Tốc độ")]
    public float moveSpeed = 2f;

    [Header("Tag kích hoạt")]
    public string playerTag = "Player";

    private bool shouldMove = false;
    private bool reachedTop = false;

    void Update()
    {
        if (shouldMove && !reachedTop)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                topPoint.position,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, topPoint.position) < 0.01f)
            {
                transform.position = topPoint.position;
                reachedTop = true;
                shouldMove = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !reachedTop)
        {
            shouldMove = true;
        }
    }
}
