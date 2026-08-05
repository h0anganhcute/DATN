using UnityEngine;

public class Skill2 : MonoBehaviour
{
    public GameObject skill2Prefab; // Prefab của Skill2
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    public void MoSkill2()
    {
        skill2Prefab.SetActive(true);       
    }

    public void TatSkill2()
    {
               skill2Prefab.SetActive(false);
    }
}
