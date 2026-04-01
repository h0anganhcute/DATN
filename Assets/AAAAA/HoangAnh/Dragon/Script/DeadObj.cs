using UnityEngine;
using Unity.FPS.Game;

public class CapsuleDie : MonoBehaviour
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
        //ChayAnimationChet();
        Destroy(gameObject, 5f); // delay 2 giây
    }
    //void ChayAnimationChet()
    //{
    //    ani.SetTrigger("DapXuong");
    //    ani.SetTrigger("Die");
    //}
}