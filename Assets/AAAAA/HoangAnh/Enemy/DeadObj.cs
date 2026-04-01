using UnityEngine;
using Unity.FPS.Game;

public class CapsuleDie : MonoBehaviour
{
    Health mau;

    void Start()
    {
        mau = GetComponent<Health>();

        if (mau != null)
        {
            mau.OnDie += KhiChet;
        }
    }

    void KhiChet()
    {
        Debug.Log("Capsule chết rồi");

        Destroy(gameObject, 2f); // delay 2 giây
    }
}