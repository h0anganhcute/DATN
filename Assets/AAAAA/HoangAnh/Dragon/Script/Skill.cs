using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skill : MonoBehaviour
{
    private Animator ani;
    private BulletBoss bulletBoss;

    [Header("====== KỸ NĂNG 5 (AFTERSHOCK) ======")]
    public GameObject Aftershock;

    [Tooltip("Chỉnh tọa độ X, Y, Z này để dịch chuyển tâm của chiêu Aftershock")]
    public Vector3 offsetSkill5 = new Vector3(0, 0, 0);

    [Tooltip("Bán kính vòng tròn hiển thị (Chỉ để nhìn trong cửa sổ Scene cho dễ căn chỉnh)")]
    public float banKinhVongTron = 3f;

    void Start()
    {
        ani = GetComponent<Animator>();
        bulletBoss = GetComponent<BulletBoss>();
    }

    void Update()
    {
        // KIỂM TRA LIÊN TỤC: Nếu script RunEnemy đang bật thì bắt buộc tắt DashSkill

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
        ani.SetTrigger("Land");
    }

    public void Skill3()
    {
        ani.SetTrigger("ClawAttack");
    }

    public void Skill4()
    {
        ani.SetTrigger("BasicAttack");
    }

    // Skill 5: Gọi Aftershock
    public void Skill5()
    {
        if (Aftershock != null)
        {
            // Vị trí tạo ra = Vị trí hiện tại của Rồng + cộng thêm độ lệch (Offset) bạn đã chỉnh
            Vector3 viTriTao = transform.position + offsetSkill5;

            // Tạo ra Aftershock
            Instantiate(Aftershock, viTriTao, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Chưa kéo thả Aftershock vào mục Skill 5!");
        }
    }

    public IEnumerator EnableBullet()
    {
        bulletBoss.enabled = true;
        yield return new WaitForSeconds(0.1f);
        bulletBoss.enabled = false;
    }

    // HÀM NÀY GIÚP VẼ RA MỘT VÒNG TRÒN MÀU ĐỎ TRONG CỬA SỔ SCENE
    // Nó chỉ hiện khi bạn nhấp chuột chọn con Boss, giúp bạn căn chỉnh thông số Offset cực dễ
    private void OnDrawGizmosSelected()
    {
        // Chọn màu đỏ cho vòng tròn
        Gizmos.color = Color.red;

        // Vị trí vẽ vòng tròn = Vị trí Boss + Offset
        Vector3 viTriVe = transform.position + offsetSkill5;

        // Vẽ vòng tròn bằng dạng lưới (WireSphere)
        Gizmos.DrawWireSphere(viTriVe, banKinhVongTron);
    }
}