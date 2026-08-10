using UnityEngine;

public class StartNguoiSat : MonoBehaviour
{
    Animator ani;
    public GameObject caMera;
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TatCamera()
    {
        caMera.SetActive(false);
    }
}
