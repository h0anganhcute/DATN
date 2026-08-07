using UnityEngine;

public class VungHanChe : MonoBehaviour
{
    private Collider myCollider;
    void Start()
    {
        myCollider = GetComponent<Collider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Nếu object va chạm KHÔNG có tag "Player", thì bỏ qua va chạm (cho đi xuyên qua)
        if (!collision.gameObject.CompareTag("Player"))
        {
            Physics.IgnoreCollision(collision.collider, myCollider, true);
        }
    }

    
}