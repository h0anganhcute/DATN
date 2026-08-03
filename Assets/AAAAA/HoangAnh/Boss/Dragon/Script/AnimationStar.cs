using UnityEngine;

public class AnimationStar : MonoBehaviour
{
    Animator ani;
    public GameObject cameraBoss;
    StartBoss startBoss;
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("Start");
        ani.SetTrigger("Stop");
        startBoss = GetComponent<StartBoss>();
    }


    void Update()
    {
        
    }
    public void tatCamera()
    {
               cameraBoss.SetActive(false);
    }
    public void batDau()
    {
        startBoss.enabled = true;
    }
    public void dung()
    {
        startBoss.enabled = false;
    }
}
