using UnityEngine;

public class ControllerTrum : MonoBehaviour
{
    public GameObject Aoe1Skill2;
    public GameObject Aoe2Skill2;
    public GameObject Aoe3Skill2;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Bật AOE Skill2
    public void BatAoe1Skill2()
    {
        Aoe1Skill2.SetActive(true);
    }
    public void BatAoe2Skill2()
    {
        Aoe2Skill2.SetActive(true);
    }
    public void BatAoe3Skill2()
    {
        Aoe3Skill2.SetActive(true);
    }
    // Tắt AOE Skill 2
    public void TatAoe1Skill2()
    {
        Aoe1Skill2.SetActive(false);
    }
    public void TatAoe2Skill2()
    {
        Aoe2Skill2.SetActive(false);
    }
    public void TatAoe3Skill2()
    {
        Aoe3Skill2.SetActive(false);
    }
}
