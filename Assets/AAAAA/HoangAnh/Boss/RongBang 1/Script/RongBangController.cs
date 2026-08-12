using UnityEngine;

public class RongBangController : MonoBehaviour
{
    Animator ani;
    public GameObject followBoss;
    public AudioSource sTart;
    public AudioSource Attack1;
    public AudioSource TuNangLuong;
    public AudioSource Attack2;
    public AudioSource TiengNo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("Start");
        ani.SetTrigger("Down");
        ani.SetTrigger("Play");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FollowBoss()
    {
        followBoss.SetActive(false);
    }
    public void StartAudio()
    {
        sTart.Play();
    }
    public void AttackAudio1()
    {
        Attack1.Play();
    }
    public void AudioTuNangLuong()
    {
        TuNangLuong.Play();
    }
    public void AudioAttack2()
    {
        Attack2.Play();
    }
    public void AudioTiengNo()
    {
        TiengNo.Play();
    }
}
