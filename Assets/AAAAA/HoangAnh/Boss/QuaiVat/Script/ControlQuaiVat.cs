using UnityEngine;
using UnityEngine.AI;

public class ControlQuaiVat : MonoBehaviour
{
    StartBoss startBoss;
    TargetBoss1 targetBoss1;
    Animator ani;
    public GameObject cameraBoss;
    public GameObject cameraPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        startBoss = GetComponent<StartBoss>();
        targetBoss1 = GetComponent<TargetBoss1>();
        ani.SetTrigger("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BatDau()
    {
              startBoss.enabled = true;
    }
    public void Dung()
    {
        startBoss.enabled = false;
    }
    public void BatDauTarget()
    {
        targetBoss1.enabled = true;
    }
    public void TatCamera()
    {
               cameraBoss.SetActive(false);
    }
}
