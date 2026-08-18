using UnityEngine;
using Unity.FPS.Gameplay;

namespace Unity.FPS.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DashOverlayUI : MonoBehaviour
    {
        [Tooltip("Tham chiếu tới PlayerCharacterController để biết lúc nào đang Dash")]
        public PlayerCharacterController PlayerCharacter;

        [Tooltip("Tốc độ hiện hiệu ứng gió")]
        public float FadeInSharpness = 15f;

        [Tooltip("Tốc độ mờ hiệu ứng gió")]
        public float FadeOutSharpness = 6f;

        [Tooltip("Độ rõ nét tối đa (Alpha) của hiệu ứng gió (0 đến 1)")]
        [Range(0f, 1f)]
        public float MaxAlpha = 0.8f;

        CanvasGroup m_CanvasGroup;

        void Start()
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
            m_CanvasGroup.alpha = 0f;
            m_CanvasGroup.interactable = false;
            m_CanvasGroup.blocksRaycasts = false;
        }

        void Update()
        {
            if (PlayerCharacter == null)
            {
                PlayerCharacter = FindFirstObjectByType<PlayerCharacterController>();
                if (PlayerCharacter == null) return;
            }

            if (PlayerCharacter.IsDashing)
            {
                // Hiện hiệu ứng gió 2 bên
                m_CanvasGroup.alpha = Mathf.Lerp(m_CanvasGroup.alpha, MaxAlpha, FadeInSharpness * Time.deltaTime);
            }
            else
            {
                // Mờ dần
                m_CanvasGroup.alpha = Mathf.Lerp(m_CanvasGroup.alpha, 0f, FadeOutSharpness * Time.deltaTime);
            }
        }
    }
}
