using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();       
    }

    //Skill 1: Flame Attack
    public void Skill1()
    {
        ani.SetTrigger("FlameAttack");
    }
    //Skill 2: Ice Attack
    public void Skill2()
    {
        ani.SetTrigger("TakeOff");
        ani.SetTrigger("FlyGlide");
    }
}