using UnityEngine;

public class ColliderOBJ : MonoBehaviour
{
    void Start()
    {
        // Lấy toàn bộ Collider trong object cha + con
        Collider[] colliders = GetComponentsInChildren<Collider>();

        // Cho tất cả collider ignore lẫn nhau
        for (int i = 0; i < colliders.Length; i++)
        {
            for (int j = i + 1; j < colliders.Length; j++)
            {
                Physics.IgnoreCollision(colliders[i], colliders[j], true);
            }
        }
    }
}