using System.Collections;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.UIElements;

public class StartTrumCuoi : MonoBehaviour
{
    public LionStart lionStart;
    Animator ani;
    public GameObject caMera;
    MenuSkillTrum menuSkillTrum;
    public GameObject diemTele;
    public GameObject LionBoss;
    
    private bool choPhepChayAnimation = false;
    public float TimeDelayAni = 3f;
    BoxCollider box;
    ComBoSkillTrum comb;
    private Health TrumCuoi;
    public GameObject LightTim;
    AudioSource nhacNen;

    private bool triggered50 = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("Start");
        menuSkillTrum = GetComponent<MenuSkillTrum>();
        box = GetComponent<BoxCollider>();
        comb = GetComponent<ComBoSkillTrum>();
        TrumCuoi=GetComponent<Health>();
        nhacNen = GetComponent<AudioSource>();
        
    }
     void Update()
    {

        if (TrumCuoi == null) return;

        float healthRatio = TrumCuoi.CurrentHealth / TrumCuoi.MaxHealth;

        if (healthRatio <= 0.5f && !triggered50)
        {
            triggered50 = true;
            menuSkillTrum.enabled = false;
            ani.SetTrigger("StartTele");
            ani.SetTrigger("Tele");
            LightTim.SetActive(true);
        }
        

        if (LionBoss == null && !choPhepChayAnimation)
        {
            nhacNen.enabled = true;
            choPhepChayAnimation |= true;
            TatBoxCollider();
        }
    }
    public void ThamChieuMoMenu()
    {
        lionStart.MoMenuSkill();
    }
    public void TatCamera()
    {
        caMera.SetActive(false);
    }
    public void MoMenuSkillTrum()
    {
        menuSkillTrum.enabled = true;
    }
    public void BatCamera()
    {
        caMera.SetActive(true);
    }
    public IEnumerator DelayAnimation()
    {
        ani.enabled = false;
        yield return new WaitForSeconds(TimeDelayAni);
        ani.enabled=true;
    }
    
    public void TatBoxCollider()
    {
        menuSkillTrum.enabled = true;
        box.enabled=false;
    }
    public void BatComBoSkill()
    {
        comb.enabled = true;
    }

    //TeleBoss

    public void TeleTrum()
    {
        // 2. Đổi scale và dịch chuyển vị trí của object
        transform.localScale = new Vector3(1, 1, 1);
        if (diemTele != null)
        {
            transform.position = diemTele.transform.position;
        }
    }
}
