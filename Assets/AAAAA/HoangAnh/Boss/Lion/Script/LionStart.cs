using UnityEngine;

public class LionStart : MonoBehaviour
{
    Animator ani;
    public GameObject Trum;
    public GameObject caMera;
    MenuSkill menuSkill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("Start");
        ani.SetTrigger("Start1");
        ani.SetTrigger("Start2");
        menuSkill = GetComponent< MenuSkill>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CloneTrum()
    {
        Trum.SetActive(true);
    }
    public void ActiveCamera()
    {
        caMera.SetActive(true);
    }
    public void DeactiveCamera()
    {
        caMera.SetActive(false);
    }
    public void MoMenuSkill()
    {
        menuSkill.enabled = true;
    }
}
