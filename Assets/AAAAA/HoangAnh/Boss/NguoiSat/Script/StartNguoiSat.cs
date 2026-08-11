using UnityEngine;

public class StartNguoiSat : MonoBehaviour
{
    Animator ani;
    public GameObject caMera;
    ControllerNguoiSat controllerNguoiSat;
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("Start");
        controllerNguoiSat = GetComponent<ControllerNguoiSat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TatCamera()
    {
        caMera.SetActive(false);
    }
    public void BatConTroller()
    {
        controllerNguoiSat.enabled = true;
    }
}
