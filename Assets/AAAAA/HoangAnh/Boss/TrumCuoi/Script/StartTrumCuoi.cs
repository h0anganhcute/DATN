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
    public Health healthLion;
    private bool choPhepChayAnimation = false;
    public float TimeDelayAni = 3f;
    BoxCollider box;
    ComBoSkillTrum comb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("Start");
        menuSkillTrum = GetComponent<MenuSkillTrum>();
        box = GetComponent<BoxCollider>();
        comb = GetComponent<ComBoSkillTrum>();
        
        
    }
     void Update()
    {

        if (healthLion != null && healthLion.CurrentHealth <= 0)
        {
            menuSkillTrum.enabled = false;
        }

        if (LionBoss == null && !choPhepChayAnimation)
        {
            choPhepChayAnimation |= true;
            ani.SetTrigger("StartTele");
            ani.SetTrigger("Tele");
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
