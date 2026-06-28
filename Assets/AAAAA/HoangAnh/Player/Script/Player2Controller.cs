using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    private Animator ani;
    
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("Sit");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
