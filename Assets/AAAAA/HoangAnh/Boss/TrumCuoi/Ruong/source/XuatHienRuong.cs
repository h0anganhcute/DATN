using UnityEngine;

public class XuatHienRuong : MonoBehaviour
{
    public GameObject TrumCuoi;
    public GameObject LoadScene;
    [Tooltip("Tốc độ xoay của kỹ năng")]
    public float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if( TrumCuoi == null)
        {
            gameObject.SetActive(true);
        }
        
            transform.Rotate(0, -speed * 60f * Time.deltaTime, 0);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadScene.SetActive(true);
        }
    }
}
