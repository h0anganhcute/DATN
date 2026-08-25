using UnityEngine;

public class XoaSkillBoss : MonoBehaviour
{
     GameObject BossCuoi;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BossCuoi = GameObject.FindGameObjectWithTag("BossCuoi");
    }

    // Update is called once per frame
    void Update()
    {
        if (BossCuoi != null && BossCuoi.transform.localScale == Vector3.one)
        {
            Destroy(gameObject);
        }
    }
}
