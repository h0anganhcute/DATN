using UnityEngine;

public class CloneVuNo : MonoBehaviour
{
    public float delay = 3f; // Thời gian trì hoãn trước khi phát nổ
    public GameObject vuNoPrefab; // Prefab của Vũ Nổ
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("PhatNo", delay); // Gọi phương thức PhatNo sau delay giây
    }

    // Update is called once per frame
    public void PhatNo()
    {
        vuNoPrefab.SetActive(true);
    }
    
}
