using UnityEngine;

public class Skill : MonoBehaviour
{

    private Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();       
    }
    //Skill 1: Flame Attack
    public void CastSkill1()
    {
        ani.SetTrigger("FlameAttack");
    }
    //Skill 2: Ice Attack
    public void CastSkill2()
    {

    }
}