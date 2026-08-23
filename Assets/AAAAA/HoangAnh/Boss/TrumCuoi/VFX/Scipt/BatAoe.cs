using UnityEngine;

public class BatAoe : MonoBehaviour
{
    public GameObject batAoe;
    public GameObject AnhSang;

    private void OnEnable()
    {
        Invoke("NhanDame", 2.2f);
    }

    private void OnDisable()
    {
        // Hủy việc đếm ngược nếu object này bị tắt
        CancelInvoke("NhanDame");
        TatNhamDame();
    }

    public void NhanDame()
    {
        batAoe.SetActive(true);
        AnhSang.SetActive(true);
    }
    public void TatNhamDame()
    {
        batAoe.SetActive(false);
        AnhSang.SetActive(false);
    }
}