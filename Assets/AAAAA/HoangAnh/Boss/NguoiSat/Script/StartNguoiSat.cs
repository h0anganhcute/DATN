using UnityEngine;
using System.Collections;

public class StartNguoiSat : MonoBehaviour
{
    Animator ani;
    public GameObject caMera;
    ControllerNguoiSat controllerNguoiSat;
    public BoxCollider DiemLuot;
    public GameObject panelLuot;
    public float tocDoLuot = 20f;
    public GameObject VFXLuot;
    public AudioSource start;

    public void audioStart()
    {
        start.Play();
    }
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("Start");
        controllerNguoiSat = GetComponent<ControllerNguoiSat>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TatCamera()
    {
        caMera.SetActive(false);
    }
    public void BatConTroller()
    {
        controllerNguoiSat.enabled = true;
    }
    public void Luot()
    {
        // Tính vị trí đầu BoxCollider (edge xa nhất theo trục X local)
        Vector3 localEdge = DiemLuot.center;
        localEdge.x -= DiemLuot.size.x / 2f;
        Vector3 worldEdge = DiemLuot.transform.TransformPoint(localEdge);

        // Đích đến giữ nguyên Y của boss
        Vector3 targetPos = worldEdge;
        targetPos.y = transform.position.y;

        StartCoroutine(LuotCoroutine(targetPos));
    }

    IEnumerator LuotCoroutine(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            // Lưu vị trí world của panelLuot trước khi di chuyển boss
            Vector3 panelWorldPos = panelLuot.transform.position;
            Quaternion panelWorldRot = panelLuot.transform.rotation;

            // Di chuyển boss về phía đích với tốc độ tocDoLuot
            transform.position = Vector3.MoveTowards(transform.position, targetPos, tocDoLuot * Time.deltaTime);

            // Giữ nguyên vị trí world của panelLuot (không bị kéo theo boss)
            panelLuot.transform.position = panelWorldPos;
            panelLuot.transform.rotation = panelWorldRot;

            yield return null;
        }
 
        // Đảm bảo boss đến đúng vị trí đích
        Vector3 finalPanelPos = panelLuot.transform.position;
        Quaternion finalPanelRot = panelLuot.transform.rotation;
        transform.position = targetPos;
        panelLuot.transform.position = finalPanelPos;
        panelLuot.transform.rotation = finalPanelRot;
    }

    public void ResetVitriPanelLuot()
    {
        // Đưa panelLuot về đúng vị trí trung tâm của BoxCollider DiemLuot
        Vector3 worldCenter = DiemLuot.transform.TransformPoint(DiemLuot.center);
        panelLuot.transform.position = worldCenter;
    }
    public void BatPanelLuot()
    {
        panelLuot.SetActive(true);
    }
    public void TatPanelLuot()
    {
        panelLuot.SetActive(false);
    }
    public void BatVFX()
    {
        VFXLuot.SetActive(true);
    }
    public void TatVFX()
    {
        VFXLuot.SetActive(false);
    }
}
