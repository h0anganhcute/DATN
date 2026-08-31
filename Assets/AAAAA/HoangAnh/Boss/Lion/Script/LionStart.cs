using UnityEngine;

public class LionStart : MonoBehaviour
{
    Animator ani;
    public GameObject Trum;
    public GameObject caMera;
    MenuSkill menuSkill;
    public AudioSource audioStart;
    public AudioSource audioDapDat;
    public AudioSource audioDonTho;
    public AudioSource audioChoang;
    
    public void AudioChoang()
    {
        audioChoang.Play();
    }
    public void AudioDonTho()
    {
        audioDonTho.Play();
    }
    public void AudioDapDat()
    {
        audioDapDat.Play();
    }
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("Start");
        ani.SetTrigger("Start1");
        ani.SetTrigger("Start2");
        menuSkill = GetComponent< MenuSkill>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CloneTrum()
    {
        Trum.SetActive(true);
    }
    public void ActiveCamera()
    {
        caMera.SetActive(true);
    }
    public void DeactiveCamera()
    {
        caMera.SetActive(false);
    }
    public void MoMenuSkill()
    {
        menuSkill.enabled = true;
    }
    public void AudioStart()
    {
        audioStart.Play();
    }
}
