using GLTFast.Schema;
using UnityEngine;

public class StartBoss : MonoBehaviour
{
    Transform startBoss;
    public float speed = 30f;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        GameObject startBossObj = GameObject.FindGameObjectWithTag("StartBoss");
        if (startBossObj != null)
        {
            startBoss = startBossObj.transform;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, startBoss.position, speed * Time.deltaTime);

    }
    
}
