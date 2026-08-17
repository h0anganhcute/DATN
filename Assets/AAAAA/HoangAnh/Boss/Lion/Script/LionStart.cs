using UnityEngine;

public class LionStart : MonoBehaviour
{
    Animator ani;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
