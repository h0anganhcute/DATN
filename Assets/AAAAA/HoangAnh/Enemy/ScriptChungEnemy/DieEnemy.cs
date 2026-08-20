using UnityEngine;
using Unity.FPS.Game;
using UnityEngine.AI;

public class DieEnemy : MonoBehaviour
{
    Health mau;
    Animator ani;
    [SerializeField] MonoBehaviour Run;
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
        // Tắt não rượt đuổi
        if (Run != null) Run.enabled = false;

        // Tắt bộ di chuyển
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false; // Tắt thẳng mặt, 1 dòng gọn ơ!

        Debug.Log("Capsule chết rồi");
        ani.SetTrigger("Die");
        Destroy(gameObject, 5f);
    }

}