using UnityEngine;

public class BossPhaseController : MonoBehaviour
{
    [Header("==== SKYBOX ====")]
    [SerializeField] private Material skyboxPhase1;
    [SerializeField] private Material skyboxPhase2;

    [Header("== DIRECTIONAL LIGHT ==")]
    [SerializeField] private Light directionalLight;

    [SerializeField] private Color phase1LightColor = Color.white;
    [SerializeField] private float phase1Intensity = 1f;

    [SerializeField] private Color phase2LightColor = Color.white;
    [SerializeField] private float phase2Intensity = 1f;

    [SerializeField] private Vector3 phase1LightRotation;
    [SerializeField] private Vector3 phase2LightRotation;

    [Header("===GLOBAL VOLUME ===")]
    [SerializeField] private GameObject globalVolumePhase1;
    [SerializeField] private GameObject globalVolumePhase2;

    private bool phase2Started = false;

    private void Start()
    {
        // Phase 1 ban đầu
        RenderSettings.skybox = skyboxPhase1;

        if (directionalLight != null)
        {
            directionalLight.color = phase1LightColor;
            directionalLight.intensity = phase1Intensity;
            directionalLight.transform.rotation =
                Quaternion.Euler(phase1LightRotation);
        }

        if (globalVolumePhase1 != null)
            globalVolumePhase1.SetActive(true);

        if (globalVolumePhase2 != null)
            globalVolumePhase2.SetActive(false);

        DynamicGI.UpdateEnvironment();
    }

    // Gọi hàm này khi Boss 1 chết
    public void ChangeToPhase2()
    {
        // Không cho gọi nhiều lần
        if (phase2Started)
            return;

        phase2Started = true;

        // =========================
        // SKYBOX
        // =========================

        if (skyboxPhase2 != null)
        {
            RenderSettings.skybox = skyboxPhase2;
        }

        // =========================
        // DIRECTIONAL LIGHT
        // =========================

        if (directionalLight != null)
        {
            directionalLight.color = phase2LightColor;
            directionalLight.intensity = phase2Intensity;

            directionalLight.transform.rotation =
                Quaternion.Euler(phase2LightRotation);
        }

        // =========================
        // GLOBAL VOLUME
        // =========================

        if (globalVolumePhase1 != null)
            globalVolumePhase1.SetActive(false);

        if (globalVolumePhase2 != null)
            globalVolumePhase2.SetActive(true);

        // Cập nhật môi trường ánh sáng
        DynamicGI.UpdateEnvironment();

        Debug.Log("Boss Phase 2 Started!");
    }
}
