using UnityEngine;
using Unity.FPS.Game;

public class DieDragon : MonoBehaviour
{
    Health mau;
    Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();
        mau = GetComponent<Health>();

        if (mau != null)
        {
            mau.OnDie += KhiChet;
        }
    }

    void KhiChet()
    {
        Debug.Log("Boss is died");
        Destroy(gameObject, 5f); // delay 2 giây
    }
}