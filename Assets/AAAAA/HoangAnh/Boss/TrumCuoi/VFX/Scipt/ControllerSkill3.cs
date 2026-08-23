using UnityEngine;

public class ControllerSkill3 : MonoBehaviour
{
    public GameObject BanChuong;
    public GameObject GongSkill3;
    
    private bool isScaling = false;
    public float timeToScale = 2f;

    void Start()
    {
        
    }

    void Update()
    {
        if (isScaling && GongSkill3 != null)
        {
            Vector3 scale = GongSkill3.transform.localScale;
            if (scale.x < 1f)
            {
                // Tăng scale X lên 1 trong 2 giây (1/2 = 0.5 đơn vị mỗi giây)
                scale.x += Time.deltaTime / timeToScale;
                
                if (scale.x >= 1f)
                {
                    scale.x = 1f;
                    isScaling = false;
                    
                    // Bật GameObject BanChuong khi Scale X đạt tới 1
                    if (BanChuong != null)
                    {
                        BanChuong.SetActive(true);
                    }
                }
                
                GongSkill3.transform.localScale = scale;
            }
        }
    }

    private void OnEnable()
    {
        // Khi được bật, đặt lại Scale X về 0 và bắt đầu quá trình tăng Scale
        if (GongSkill3 != null)
        {
            Vector3 scale = GongSkill3.transform.localScale;
            scale.x = 0f;
            GongSkill3.transform.localScale = scale;
        }
        
        // Tắt BanChuong đi để chờ Scale xong mới bật lên
        if (BanChuong != null)
        {
            BanChuong.SetActive(false);
        }
        
        isScaling = true;
    }
    
    private void OnDisable()
    {
        // Khi bị tắt, đặt lại Scale X về 0
        isScaling = false;
        if (GongSkill3 != null)
        {
            Vector3 scale = GongSkill3.transform.localScale;
            scale.x = 0f;
            GongSkill3.transform.localScale = scale;
        }
    }
}
