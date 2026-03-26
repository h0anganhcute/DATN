using UnityEngine;

public class BossDieuKhien : MonoBehaviour
{
    Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
        Invoke("ChayAnimation", 2f); // gọi sau 0.5 giây
    }

    public void ChayAnimation()
    {
        ani.SetTrigger("BayLen");
        ani.SetTrigger("Baytaicho");
    }
}
