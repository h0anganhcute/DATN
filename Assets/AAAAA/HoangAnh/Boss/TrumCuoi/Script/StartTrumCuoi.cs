using UnityEngine;

public class StartTrumCuoi : MonoBehaviour
{
    public LionStart lionStart;
    Animator ani;
    public GameObject caMera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("Start");
    }

    public void ThamChieuMoMenu()
    {
        lionStart.MoMenuSkill();
    }
    public void TatCamera()
    {
        caMera.SetActive(false);
    }
}
