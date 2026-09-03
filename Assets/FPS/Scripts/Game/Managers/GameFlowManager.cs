using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.FPS.Game
{
    public class GameFlowManager : MonoBehaviour
    {
        [Header("Parameters")] [Tooltip("Duration of the fade-to-black at the end of the game")]
        public float EndSceneLoadDelay = 3f;

        [Tooltip("The canvas group of the fade-to-black screen")]
        public CanvasGroup EndGameFadeCanvasGroup;

        [Header("Win")] [Tooltip("This string has to be the name of the scene you want to load when winning")]
        public string WinSceneName = "WinScene";

        [Tooltip("Duration of delay before the fade-to-black, if winning")]
        public float DelayBeforeFadeToBlack = 4f;

        [Tooltip("Win game message")]
        public string WinGameMessage;
        [Tooltip("Duration of delay before the win message")]
        public float DelayBeforeWinMessage = 2f;

        [Tooltip("Sound played on win")] public AudioClip VictorySound;

        [Header("Respawn")]
        [Tooltip("Duration of the fade-to-black when dying")]
        public float RespawnFadeOutDuration = 1.2f;

        [Tooltip("Duration to stay black before reviving")]
        public float RespawnBlackDuration = 0.4f;

        [Tooltip("Duration of the fade back in from black")]
        public float RespawnFadeInDuration = 1.0f;

        public bool GameIsEnding { get; private set; }
        public bool IsRespawning { get; private set; }

        float m_TimeLoadEndGameScene;
        string LostScene;
        Coroutine m_RespawnCoroutine;

        void Awake()
        {
            EventManager.AddListener<AllObjectivesCompletedEvent>(OnAllObjectivesCompleted);
            EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
        }

        void Start()
        {
            AudioUtility.SetMasterVolume(1);
        }

        void Update()
        {
            if (GameIsEnding)
            {
                float timeRatio = 1 - (m_TimeLoadEndGameScene - Time.time) / EndSceneLoadDelay;
                EndGameFadeCanvasGroup.alpha = timeRatio;

                AudioUtility.SetMasterVolume(1 - timeRatio);

                // See if it's time to load the end scene (after the delay)
                if (Time.time >= m_TimeLoadEndGameScene)
                {
                    SceneManager.LoadScene(LostScene);
                    GameIsEnding = false;
                }
            }
        }

        void OnAllObjectivesCompleted(AllObjectivesCompletedEvent evt) => EndGame(true);

        void OnPlayerDeath(PlayerDeathEvent evt)
        {
            if (GameIsEnding || IsRespawning)
                return;

            if (m_RespawnCoroutine != null)
            {
                StopCoroutine(m_RespawnCoroutine);
            }

            m_RespawnCoroutine = StartCoroutine(RespawnRoutine());
        }

        IEnumerator RespawnRoutine()
        {
            IsRespawning = true;
            EndGameFadeCanvasGroup.gameObject.SetActive(true);

            // 1. Màn hình tối dần
            float elapsed = 0f;
            while (elapsed < RespawnFadeOutDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / RespawnFadeOutDuration);
                EndGameFadeCanvasGroup.alpha = t;
                AudioUtility.SetMasterVolume(Mathf.Lerp(1f, 0.2f, t));
                yield return null;
            }
            EndGameFadeCanvasGroup.alpha = 1f;

            // 2. Giữ màn đen và hồi phục máu 100% cùng trạng thái Player tại chỗ
            yield return new WaitForSeconds(RespawnBlackDuration);

            EventManager.Broadcast(Events.PlayerReviveEvent);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // 3. Màn hình sáng dần trở lại
            elapsed = 0f;
            while (elapsed < RespawnFadeInDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / RespawnFadeInDuration);
                EndGameFadeCanvasGroup.alpha = 1f - t;
                AudioUtility.SetMasterVolume(Mathf.Lerp(0.2f, 1f, t));
                yield return null;
            }

            EndGameFadeCanvasGroup.alpha = 0f;
            EndGameFadeCanvasGroup.gameObject.SetActive(false);
            AudioUtility.SetMasterVolume(1f);

            IsRespawning = false;
            m_RespawnCoroutine = null;
        }

        void EndGame(bool win)
        {
            if (!win)
                return;

            // unlocks the cursor before leaving the scene, to be able to click buttons
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Remember that we need to load the appropriate end scene after a delay
            GameIsEnding = true;
            EndGameFadeCanvasGroup.gameObject.SetActive(true);

            LostScene = WinSceneName;
            m_TimeLoadEndGameScene = Time.time + EndSceneLoadDelay + DelayBeforeFadeToBlack;

            // play a sound on win
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = VictorySound;
            audioSource.playOnAwake = false;
            audioSource.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.HUDVictory);
            audioSource.PlayScheduled(AudioSettings.dspTime + DelayBeforeWinMessage);

            DisplayMessageEvent displayMessage = Events.DisplayMessageEvent;
            displayMessage.Message = WinGameMessage;
            displayMessage.DelayBeforeDisplay = DelayBeforeWinMessage;
            EventManager.Broadcast(displayMessage);
        }

        void OnDestroy()
        {
            EventManager.RemoveListener<AllObjectivesCompletedEvent>(OnAllObjectivesCompleted);
            EventManager.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
        }
    }
}