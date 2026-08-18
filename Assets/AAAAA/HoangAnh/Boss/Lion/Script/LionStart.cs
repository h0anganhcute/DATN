using UnityEngine;

public class LionStart : MonoBehaviour
{
    Animator ani;
    public GameObject Trum;
    public GameObject caMera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("Start");
        ani.SetTrigger("Start1");
        ani.SetTrigger("Start2");
        
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
}
