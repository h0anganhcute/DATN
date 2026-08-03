using UnityEngine;

public class AnimationStar : MonoBehaviour
{
    Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("Start");
        ani.SetTrigger("Stop");
    }


    void Update()
    {
        
    }
}
