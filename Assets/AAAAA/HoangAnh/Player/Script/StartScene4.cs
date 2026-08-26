using System.Collections;
using UnityEngine;

public class StartScene4 : MonoBehaviour
{
    public Camera Camera;
    public GameObject canVas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("chayCamera",3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void chayCamera()
    {
        if (Camera != null)
        {
            StartCoroutine(ChangeFOVCoroutine());
        }
    }

    private IEnumerator ChangeFOVCoroutine()
    {
        float startFOV = Camera.fieldOfView;
        float targetFOV = 80f;
        float duration = 5f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            Camera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        Camera.fieldOfView = targetFOV;
        Camera.gameObject.SetActive(false);
        canVas.SetActive(true);
    }
}
