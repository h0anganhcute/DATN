using UnityEngine;

public class BossPhaseController : MonoBehaviour
{
    [Header("===== SKYBOX =====")]
    [SerializeField] private Material skyboxPhase1;
    [SerializeField] private Material skyboxPhase2;

    [Header("===== DIRECTIONAL LIGHT =====")]
    [SerializeField] private Light directionalLight;

    [SerializeField] private Color phase1LightColor = Color.white;
    [SerializeField] private float phase1Intensity = 1f;
    [SerializeField] private Vector3 phase1LightRotation;

    [SerializeField] private Color phase2LightColor = Color.white;
    [SerializeField] private float phase2Intensity = 1f;
    [SerializeField] private Vector3 phase2LightRotation;

    [Header("===== GLOBAL VOLUME =====")]
    [SerializeField] private GameObject globalVolumePhase1;
    [SerializeField] private GameObject globalVolumePhase2;
    public GameObject Boss;

    private bool phase2Started = false;

    private void Start()
    {
        ApplyPhase1();
    }

    // =========================
    // PHASE 1
    // =========================
    public void ApplyPhase1()
    {
        phase2Started = false;

        // Skybox
        if (skyboxPhase1 != null)
        {
            RenderSettings.skybox = skyboxPhase1;
        }

        // Directional Light
        if (directionalLight != null)
        {
            directionalLight.color = phase1LightColor;
            directionalLight.intensity = phase1Intensity;
            directionalLight.transform.rotation =
                Quaternion.Euler(phase1LightRotation);
        }

        // Global Volume
        if (globalVolumePhase1 != null)
            globalVolumePhase1.SetActive(true);

        if (globalVolumePhase2 != null)
            globalVolumePhase2.SetActive(false);

        DynamicGI.UpdateEnvironment();
    }

    // =========================
    // PHASE 2
    // =========================
    public void ChangeToPhase2()
    {
        if (phase2Started)
            return;

        phase2Started = true;

        // Skybox
        if (skyboxPhase2 != null)
        {
            RenderSettings.skybox = skyboxPhase2;
        }

        // Directional Light
        if (directionalLight != null)
        {
            directionalLight.color = phase2LightColor;
            directionalLight.intensity = phase2Intensity;
            directionalLight.transform.rotation =
                Quaternion.Euler(phase2LightRotation);
        }

        // Global Volume
        if (globalVolumePhase1 != null)
            globalVolumePhase1.SetActive(false);

        if (globalVolumePhase2 != null)
            globalVolumePhase2.SetActive(true);

        DynamicGI.UpdateEnvironment();

        Debug.Log("===== BOSS PHASE 2 =====");
    }

    // TEST: nhấn P để chuyển Phase 2
    private void Update()
    {
        if (Boss != null && Boss.activeInHierarchy)
        {
            ChangeToPhase2();
        }
    }
}
