using GLTFast.Schema;
using UnityEngine;

public class CloneRuong : MonoBehaviour
{
    public GameObject TrumCuoi;
    public GameObject Ruong;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (TrumCuoi == null)
        {
            Ruong.SetActive(true);
        }
    }
}
