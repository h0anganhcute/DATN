using UnityEngine;

public class BossPhaseController : MonoBehaviour
{
    [Header("Skybox")]
    [SerializeField] private Material skyboxPhase1;
    [SerializeField] private Material skyboxPhase2;

    private void Start()
    {
        // Skybox ban đầu
        RenderSettings.skybox = skyboxPhase1;
        DynamicGI.UpdateEnvironment();
    }

    public void ChangeToPhase2()
    {
        // Đổi Skybox ngay lập tức
        RenderSettings.skybox = skyboxPhase2;
        DynamicGI.UpdateEnvironment();
    }
}
