using UnityEngine;

public class DestroyObj : MonoBehaviour
{
    public float delay = 3f; // thời gian trì hoãn trước khi hủy đối tượng
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, delay); // delay 3 giây
    }
}
