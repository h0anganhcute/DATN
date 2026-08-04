using UnityEngine;

public class RongBangController : MonoBehaviour
{
    Animator ani;
    public GameObject followBoss;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("Start");
        ani.SetTrigger("Down");
        ani.SetTrigger("Play");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FollowBoss()
    {
        followBoss.SetActive(false);
    }
}
