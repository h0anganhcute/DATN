using UnityEngine;

public class ThoaiMe : MonoBehaviour
{
    public GameObject thoaiMom;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Mom"))
        {
            thoaiMom.SetActive(true);
        }
    }
}
