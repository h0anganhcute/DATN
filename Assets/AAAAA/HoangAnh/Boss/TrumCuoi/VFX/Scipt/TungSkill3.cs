using UnityEngine;

public class TungSkill3 : MonoBehaviour
{
    public GameObject tungSkill3;
    [Tooltip("Tốc độ xoay của kỹ năng")]
    public float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tungSkill3 == null || !tungSkill3.activeSelf)
        {
            transform.Rotate(0, -speed * 60f * Time.deltaTime, 0);
        }
    }
}
