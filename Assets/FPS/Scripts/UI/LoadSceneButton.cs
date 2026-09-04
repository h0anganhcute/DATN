using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Unity.FPS.UI
{
    public class LoadSceneButton : MonoBehaviour
    {
        public string SceneName = "Level-01";

        private InputAction m_SubmitAction;
        
        void Start()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == "IntroMenu" || currentScene == "WinScene")
            {
                PlayerRespawnManager.ResetRespawnData();
            }

            m_SubmitAction = InputSystem.actions.FindAction("UI/Submit");
            m_SubmitAction.Enable();
        }
        
        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == gameObject
                && m_SubmitAction.WasPressedThisFrame())
            {
                LoadTargetScene();
            }
        }

        public void LoadTargetScene()
        {
            string target = SceneName;

            //if (PlayerRespawnManager.HasRespawnData && !string.IsNullOrEmpty(PlayerRespawnManager.DeathSceneName))
            //{
            //    target = PlayerRespawnManager.DeathSceneName;
            //}
            //else if (target == "MainScene")
            //{
            //    target = "Level-01";
            //}

            SceneManager.LoadScene(target);
        }
    }
}