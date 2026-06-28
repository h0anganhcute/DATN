using StarterAssets;
using System.Collections;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    private Animator ani;
    private ThirdPersonController playerController;
    void Start()
    {
        ani = GetComponent<Animator>();
        
        playerController = GetComponent<ThirdPersonController>();
        StartCoroutine(SitDown());
    }

    void Update()
    {
        
    }
    private IEnumerator SitDown()
    {
        yield return new WaitForSeconds(1f);
        ani.SetTrigger("Sit");
        yield return new WaitForSeconds(1f);
        playerController.enabled = true;
    }
}
