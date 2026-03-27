using UnityEngine;
using UnityEngine.InputSystem;

namespace TheDeveloperTrain.SciFiGuns
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 20f;
        [SerializeField] private float mouseSensitivity = 2f;

        [SerializeField] Vector2 xAxisClamp = new Vector2(-100, 100);
        [SerializeField] private Vector2 yAxisClamp = new Vector2(-100, 100);
        [SerializeField] private Vector2 zAxisClamp = new Vector2(-100, 100);

        private float rotationX = 0f;
        private float rotationY = 0f;

        private Vector2 moveInput;
        private Vector2 lookInput;

        private PlayerInputActions inputActions;

        private void Awake()
        {
            inputActions = new PlayerInputActions();

            inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

            inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
            inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            // Mouse look
            float mouseX = lookInput.x * mouseSensitivity;
            float mouseY = lookInput.y * mouseSensitivity;

            rotationX -= mouseY;
            rotationY += mouseX;

            rotationX = Mathf.Clamp(rotationX, -90f, 90f);

            transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);

            // Movement
            Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
            transform.position += moveSpeed * Time.deltaTime * move;

            // Clamp position
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, xAxisClamp.x, xAxisClamp.y),
                Mathf.Clamp(transform.position.y, yAxisClamp.x, yAxisClamp.y),
                Mathf.Clamp(transform.position.z, zAxisClamp.x, zAxisClamp.y)
            );
        }
    }
}