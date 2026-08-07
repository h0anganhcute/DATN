using UnityEngine;

public class ThoaiController : MonoBehaviour
{
    public GameObject thoaiCon1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("ThoaiCon1", 2f);
    }

    // Update is called once per frame
    public void ThoaiCon1()
    {
               thoaiCon1.SetActive(true);
    }
}
