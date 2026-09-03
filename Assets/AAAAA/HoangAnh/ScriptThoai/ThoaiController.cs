using UnityEngine;

public class ThoaiController : MonoBehaviour
{
    public GameObject thoaiCon1;
    public GameObject ChuongBao;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("ThoaiCon1", 2f);
    }

    // Update is called once per frame
    public void ThoaiCon1()
    {
        thoaiCon1.SetActive(true);
        Invoke("BatChuongBao", 3f);
    }

    private void BatChuongBao()
    {
        if (ChuongBao != null)
        {
            ChuongBao.SetActive(true);
            Invoke("TatChuongBao", 3f);
        }
    }

    private void TatChuongBao()
    {
        if (ChuongBao != null)
        {
            ChuongBao.SetActive(false);
        }
    }
}
