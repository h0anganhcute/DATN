using UnityEngine;
using Unity.FPS.Game;

public class DieEnemy : MonoBehaviour
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
        Debug.Log("Capsule chết rồi");
        ani.SetTrigger("Die");
        Destroy(gameObject, 5f); // delay 2 giây
    }

}