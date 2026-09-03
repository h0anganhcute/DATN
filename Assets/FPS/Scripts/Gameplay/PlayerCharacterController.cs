using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Unity.FPS.Gameplay
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerInputHandler), typeof(AudioSource))]
    public class PlayerCharacterController : MonoBehaviour
    {
        [Header("References")]
        public Camera PlayerCamera;
        public AudioSource AudioSource;

        [Header("General")]
        public float GravityDownForce = 20f;
        public LayerMask GroundCheckLayers = -1;
        public float GroundCheckDistance = 0.05f;

        [Header("Movement")]
        public float MaxSpeedOnGround = 10f;
        public float MovementSharpnessOnGround = 15f;

        [Range(0, 1)]
        public float MaxSpeedCrouchedRatio = 0.5f;

        public float MaxSpeedInAir = 10f;
        public float AccelerationSpeedInAir = 25f;
        public float SprintSpeedModifier = 2f;
        public float KillHeight = -50f;

        [Header("Rotation")]
        public float RotationSpeed = 200f;

        [Range(0.1f, 1f)]
        public float AimingRotationMultiplier = 0.4f;

        [Header("Jump")]
        public float JumpForce = 9f;

        [Header("Stance")]
        public float CameraHeightRatio = 0.9f;
        public float CapsuleHeightStanding = 1.8f;
        public float CapsuleHeightCrouching = 0.9f;
        public float CrouchingSharpness = 10f;

        // ============================================================
        // DASH
        // ============================================================

        [Header("Dash")]

        [Tooltip("Tốc độ Dash")]
        public float DashSpeed = 20f;

        [Tooltip("Thời gian Dash")]
        public float DashDuration = 0.2f;

        [Tooltip("Thời gian hồi Dash")]
        public float DashCooldown = 1f;

        [Tooltip("Camera hạ xuống bao nhiêu khi Dash")]
        public float DashCameraDrop = 0.25f;

        [Tooltip("Độ mượt camera")]
        public float DashCameraSharpness = 15f;

        [Tooltip("Có thể Dash khi đang crouch")]
        public bool CanDashWhileCrouching = true;

        [Tooltip("Có thể Dash khi đang sprint")]
        public bool CanDashWhileSprinting = true;

        // ============================================================
        // DASH VFX
        // ============================================================

        [Header("Dash VFX")]

        [Tooltip("Particle VFX khi Dash")]
        public ParticleSystem DashVFX;

        [Tooltip("Vị trí xuất hiện VFX")]
        public Transform DashVFXPoint;

        // ============================================================
        // DASH UI
        // ============================================================

        [Header("Dash UI")]

        [Tooltip("Image hiển thị cooldown Dash")]
        public Image DashIcon;

        [Range(0f, 1f)]
        public float DashCooldownAlpha = 0.35f;

        // ============================================================
        // AUDIO
        // ============================================================

        [Header("Audio")]

        public float FootstepSfxFrequency = 1f;
        public float FootstepSfxFrequencyWhileSprinting = 1f;

        public AudioClip FootstepSfx;
        public AudioClip JumpSfx;
        public AudioClip LandSfx;
        public AudioClip FallDamageSfx;

        [Header("Fall Damage")]

        public bool RecievesFallDamage;
        public float MinSpeedForFallDamage = 10f;
        public float MaxSpeedForFallDamage = 30f;
        public float FallDamageAtMinSpeed = 10f;
        public float FallDamageAtMaxSpeed = 50f;

        // ============================================================
        // PUBLIC VARIABLES
        // ============================================================

        public UnityAction<bool> OnStanceChanged;

        public Vector3 CharacterVelocity { get; set; }

        public bool IsGrounded { get; private set; }
        public bool HasJumpedThisFrame { get; private set; }
        public bool IsDead { get; private set; }
        public bool IsCrouching { get; private set; }
        public bool IsDashing { get; private set; }

        public float DashCooldownRemaining
        {
            get
            {
                if (Time.time >= m_LastDashTime + DashCooldown)
                    return 0f;

                return (m_LastDashTime + DashCooldown) - Time.time;
            }
        }

        public float RotationMultiplier
        {
            get
            {
                if (m_WeaponsManager != null && m_WeaponsManager.IsAiming)
                    return AimingRotationMultiplier;

                return 1f;
            }
        }

        // ============================================================
        // COMPONENTS
        // ============================================================

        Health m_Health;
        PlayerInputHandler m_InputHandler;
        CharacterController m_Controller;
        PlayerWeaponsManager m_WeaponsManager;
        Actor m_Actor;

        // ============================================================
        // MOVEMENT VARIABLES
        // ============================================================

        Vector3 m_GroundNormal;
        Vector3 m_LatestImpactSpeed;
        Vector3 m_LastGroundedPosition;

        float m_LastTimeJumped = 0f;
        float m_CameraVerticalAngle = 0f;
        float m_FootstepDistanceCounter;
        float m_TargetCharacterHeight;

        // ============================================================
        // DASH VARIABLES
        // ============================================================

        Vector3 m_DashDirection;
        float m_DashTimer;
        float m_LastDashTime = -999f;

        // ============================================================
        // CAMERA SHAKE VARIABLES
        // ============================================================

        Vector3 m_CameraShakeOffset;
        Vector3 m_CameraShakeVelocity;

        public void AddCameraShake(float intensity)
        {
            // Trong Unity, xoay quanh trục X theo giá trị âm sẽ khiến camera ngẩng lên trên.
            // Có thể thêm một chút xíu rung ngang (Y) để chân thực hơn, nhưng chủ yếu là giật lên trên.
            m_CameraShakeVelocity += new Vector3(
                -intensity,
                Random.Range(-intensity, intensity) * 0.05f, 
                0f);
        }

        // ============================================================
        // CONSTANTS
        // ============================================================

        const float k_JumpGroundingPreventionTime = 0.2f;
        const float k_GroundCheckDistanceInAir = 0.07f;

        // ============================================================
        // AWAKE
        // ============================================================

        void Awake()
        {
            ActorsManager actorsManager =
                FindFirstObjectByType<ActorsManager>();

            if (actorsManager != null)
                actorsManager.SetPlayer(gameObject);
        }

        // ============================================================
        // START
        // ============================================================

        void Start()
        {
            m_Controller =
                GetComponent<CharacterController>();

            DebugUtility.HandleErrorIfNullGetComponent
                <CharacterController, PlayerCharacterController>(
                    m_Controller,
                    this,
                    gameObject);

            m_InputHandler =
                GetComponent<PlayerInputHandler>();

            DebugUtility.HandleErrorIfNullGetComponent
                <PlayerInputHandler, PlayerCharacterController>(
                    m_InputHandler,
                    this,
                    gameObject);

            m_WeaponsManager =
                GetComponent<PlayerWeaponsManager>();

            DebugUtility.HandleErrorIfNullGetComponent
                <PlayerWeaponsManager, PlayerCharacterController>(
                    m_WeaponsManager,
                    this,
                    gameObject);

            m_Health =
                GetComponent<Health>();

            DebugUtility.HandleErrorIfNullGetComponent
                <Health, PlayerCharacterController>(
                    m_Health,
                    this,
                    gameObject);

            m_Actor =
                GetComponent<Actor>();

            DebugUtility.HandleErrorIfNullGetComponent
                <Actor, PlayerCharacterController>(
                    m_Actor,
                    this,
                    gameObject);

            m_Controller.enableOverlapRecovery = true;

            m_Health.OnDie += OnDie;
            EventManager.AddListener<PlayerReviveEvent>(OnPlayerRevive);

            m_LastGroundedPosition = transform.position;

            if (PlayerRespawnManager.HasRespawnData)
            {
                if (m_Controller != null)
                {
                    m_Controller.enabled = false;
                }

                transform.position = PlayerRespawnManager.DeathPosition;
                transform.rotation = PlayerRespawnManager.DeathRotation;
                Physics.SyncTransforms();

                if (m_Controller != null)
                {
                    m_Controller.enabled = true;
                }

                CharacterVelocity = Vector3.zero;
                m_CameraVerticalAngle = PlayerRespawnManager.DeathCameraVerticalAngle;
                m_LastGroundedPosition = transform.position;

                if (m_Health != null)
                {
                    m_Health.ResetHealth();
                }

                PlayerRespawnManager.ConsumeRespawnData();
            }
            else
            {
                if (m_Health != null)
                {
                    m_Health.ResetHealth();
                }
            }

            SetCrouchingState(false, true);

            UpdateCharacterHeight(true);

            UpdateDashUI();
        }

        // ============================================================
        // UPDATE
        // ============================================================

        void Update()
        {
            if (IsDead)
            {
                CharacterVelocity = Vector3.zero;
                return;
            }

            if (transform.position.y < KillHeight)
            {
                m_Health.Kill();
            }

            HasJumpedThisFrame = false;

            bool wasGrounded = IsGrounded;

            GroundCheck();

            if (IsGrounded && transform.position.y >= KillHeight)
            {
                m_LastGroundedPosition = transform.position;
            }

            // ========================================================
            // LANDING
            // ========================================================

            if (IsGrounded && !wasGrounded)
            {
                float fallSpeed =
                    -Mathf.Min(
                        CharacterVelocity.y,
                        m_LatestImpactSpeed.y);

                float fallSpeedRatio =
                    (fallSpeed - MinSpeedForFallDamage) /
                    (MaxSpeedForFallDamage - MinSpeedForFallDamage);

                if (RecievesFallDamage && fallSpeedRatio > 0f)
                {
                    float damage =
                        Mathf.Lerp(
                            FallDamageAtMinSpeed,
                            FallDamageAtMaxSpeed,
                            fallSpeedRatio);

                    m_Health.TakeDamage(damage, null);

                    if (FallDamageSfx != null)
                        AudioSource.PlayOneShot(FallDamageSfx);
                }
                else
                {
                    if (LandSfx != null)
                        AudioSource.PlayOneShot(LandSfx);
                }
            }

            // ========================================================
            // CROUCH
            // ========================================================

            if (m_InputHandler.GetCrouchInputDown() &&
                !IsDashing)
            {
                SetCrouchingState(
                    !IsCrouching,
                    false);
            }

            UpdateCharacterHeight(false);

            UpdateDashUI();

            HandleCharacterMovement();
        }

        // ============================================================
        // DIE
        // ============================================================

        void OnDie()
        {
            IsDead = true;
            IsDashing = false;

            Vector3 deathPos = transform.position;
            if (deathPos.y < KillHeight)
            {
                deathPos = m_LastGroundedPosition;
            }

            PlayerRespawnManager.SaveDeathState(
                SceneManager.GetActiveScene().name,
                deathPos,
                transform.rotation,
                m_CameraVerticalAngle);

            m_WeaponsManager.SwitchToWeaponIndex(-1, true);

            EventManager.Broadcast(
                Events.PlayerDeathEvent);
        }

        // ============================================================
        // REVIVE
        // ============================================================

        public void Revive()
        {
            IsDead = false;
            IsDashing = false;

            // Nếu rơi vực thì kéo lại vị trí mặt đất an toàn gần nhất
            if (transform.position.y < KillHeight)
            {
                if (m_Controller != null)
                {
                    m_Controller.enabled = false;
                }
                transform.position = m_LastGroundedPosition;
                Physics.SyncTransforms();
                if (m_Controller != null)
                {
                    m_Controller.enabled = true;
                }
            }

            CharacterVelocity = Vector3.zero;

            if (m_Health != null)
            {
                m_Health.ResetHealth();
            }

            // Trang bị lại vũ khí
            if (m_WeaponsManager != null)
            {
                m_WeaponsManager.SwitchToWeaponIndex(0, true);
            }
        }

        void OnPlayerRevive(PlayerReviveEvent evt) => Revive();

        void OnDestroy()
        {
            EventManager.RemoveListener<PlayerReviveEvent>(OnPlayerRevive);
        }

        // ============================================================
        // MOVEMENT
        // ============================================================

        void HandleCharacterMovement()
        {
            // ========================================================
            // CAMERA / PLAYER ROTATION
            // ========================================================

            transform.Rotate(
                new Vector3(
                    0f,
                    m_InputHandler.GetLookInputsHorizontal()
                    * RotationSpeed
                    * RotationMultiplier,
                    0f),
                Space.Self);

            m_CameraVerticalAngle +=
                m_InputHandler.GetLookInputsVertical()
                * RotationSpeed
                * RotationMultiplier;

            m_CameraVerticalAngle =
                Mathf.Clamp(
                    m_CameraVerticalAngle,
                    -89f,
                    89f);

            // ========================================================
            // CAMERA SHAKE UPDATE
            // ========================================================
            m_CameraShakeOffset = Vector3.Lerp(m_CameraShakeOffset, Vector3.zero, Time.deltaTime * 10f);
            m_CameraShakeVelocity = Vector3.Lerp(m_CameraShakeVelocity, Vector3.zero, Time.deltaTime * 20f);
            m_CameraShakeOffset += m_CameraShakeVelocity * Time.deltaTime;

            PlayerCamera.transform.localEulerAngles =
                new Vector3(
                    m_CameraVerticalAngle,
                    0f,
                    0f) + m_CameraShakeOffset;

            // ========================================================
            // SHIFT DASH
            // ========================================================

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                TryDash();
            }

            // ========================================================
            // DASH
            // ========================================================

            if (IsDashing)
            {
                HandleDash();
                return;
            }

            // ========================================================
            // NORMAL MOVEMENT
            // ========================================================

            bool isSprinting =
                m_InputHandler.GetSprintInputHeld();

            if (isSprinting)
            {
                isSprinting =
                    SetCrouchingState(false, false);
            }

            float speedModifier =
                isSprinting
                    ? SprintSpeedModifier
                    : 1f;

            Vector3 worldspaceMoveInput =
                transform.TransformVector(
                    m_InputHandler.GetMoveInput());

            // ========================================================
            // GROUND
            // ========================================================

            if (IsGrounded)
            {
                Vector3 targetVelocity =
                    worldspaceMoveInput
                    * MaxSpeedOnGround
                    * speedModifier;

                if (IsCrouching)
                {
                    targetVelocity *=
                        MaxSpeedCrouchedRatio;
                }

                if (targetVelocity.sqrMagnitude > 0.001f)
                {
                    targetVelocity =
                        GetDirectionReorientedOnSlope(
                            targetVelocity.normalized,
                            m_GroundNormal)
                        * targetVelocity.magnitude;
                }

                CharacterVelocity =
                    Vector3.Lerp(
                        CharacterVelocity,
                        targetVelocity,
                        MovementSharpnessOnGround
                        * Time.deltaTime);

                // ====================================================
                // JUMP
                // ====================================================

                if (m_InputHandler.GetJumpInputDown())
                {
                    if (SetCrouchingState(false, false))
                    {
                        Vector3 jumpVelocity =
                            CharacterVelocity;

                        jumpVelocity.y = 0f;

                        jumpVelocity +=
                            Vector3.up * JumpForce;

                        CharacterVelocity =
                            jumpVelocity;

                        if (JumpSfx != null)
                            AudioSource.PlayOneShot(
                                JumpSfx);

                        m_LastTimeJumped =
                            Time.time;

                        HasJumpedThisFrame = true;

                        IsGrounded = false;

                        m_GroundNormal =
                            Vector3.up;
                    }
                }

                // ====================================================
                // FOOTSTEP
                // ====================================================

                float footstepFrequency =
                    isSprinting
                        ? FootstepSfxFrequencyWhileSprinting
                        : FootstepSfxFrequency;

                if (m_FootstepDistanceCounter >=
                    1f / footstepFrequency)
                {
                    m_FootstepDistanceCounter = 0f;

                    if (FootstepSfx != null)
                        AudioSource.PlayOneShot(
                            FootstepSfx);
                }

                m_FootstepDistanceCounter +=
                    CharacterVelocity.magnitude
                    * Time.deltaTime;
            }

            // ========================================================
            // AIR
            // ========================================================

            else
            {
                CharacterVelocity +=
                    worldspaceMoveInput
                    * AccelerationSpeedInAir
                    * Time.deltaTime;

                float verticalVelocity =
                    CharacterVelocity.y;

                Vector3 horizontalVelocity =
                    Vector3.ProjectOnPlane(
                        CharacterVelocity,
                        Vector3.up);

                horizontalVelocity =
                    Vector3.ClampMagnitude(
                        horizontalVelocity,
                        MaxSpeedInAir
                        * speedModifier);

                CharacterVelocity =
                    horizontalVelocity
                    + Vector3.up * verticalVelocity;

                CharacterVelocity +=
                    Vector3.down
                    * GravityDownForce
                    * Time.deltaTime;
            }

            // ========================================================
            // MOVE CHARACTER
            // ========================================================

            Vector3 capsuleBottomBeforeMove =
                GetCapsuleBottomHemisphere();

            Vector3 capsuleTopBeforeMove =
                GetCapsuleTopHemisphere(
                    m_Controller.height);

            m_Controller.Move(
                CharacterVelocity
                * Time.deltaTime);

            // ========================================================
            // COLLISION
            // ========================================================

            m_LatestImpactSpeed =
                Vector3.zero;

            if (CharacterVelocity.sqrMagnitude > 0.001f)
            {
                if (Physics.CapsuleCast(
                    capsuleBottomBeforeMove,
                    capsuleTopBeforeMove,
                    m_Controller.radius,
                    CharacterVelocity.normalized,
                    out RaycastHit hit,
                    CharacterVelocity.magnitude
                    * Time.deltaTime,
                    -1,
                    QueryTriggerInteraction.Ignore))
                {
                    m_LatestImpactSpeed =
                        CharacterVelocity;

                    CharacterVelocity =
                        Vector3.ProjectOnPlane(
                            CharacterVelocity,
                            hit.normal);
                }
            }
        }

        // ============================================================
        // DASH
        // ============================================================

        void TryDash()
        {
            if (IsDead)
                return;

            if (IsDashing)
                return;

            if (!IsGrounded)
                return;

            if (IsCrouching &&
                !CanDashWhileCrouching)
                return;

            if (!CanDashWhileSprinting &&
                m_InputHandler.GetSprintInputHeld())
                return;

            if (Time.time <
                m_LastDashTime + DashCooldown)
                return;

            // ========================================================
            // GET INPUT
            // ========================================================

            Vector3 moveInput =
                m_InputHandler.GetMoveInput();

            Vector3 dashDirection =
                transform.forward * moveInput.z
                + transform.right * moveInput.x;

            // Không bấm WASD
            // Dash theo hướng nhìn
            if (dashDirection.sqrMagnitude < 0.01f)
            {
                dashDirection =
                    transform.forward;
            }

            dashDirection.y = 0f;

            dashDirection.Normalize();

            // ========================================================
            // SLOPE
            // ========================================================

            if (IsGrounded)
            {
                dashDirection =
                    GetDirectionReorientedOnSlope(
                        dashDirection,
                        m_GroundNormal);

                dashDirection.y = 0f;

                if (dashDirection.sqrMagnitude > 0.01f)
                    dashDirection.Normalize();
            }

            m_DashDirection =
                dashDirection;

            IsDashing = true;

            m_DashTimer = 0f;

            m_LastDashTime =
                Time.time;

            // Reset velocity
            CharacterVelocity =
                Vector3.zero;

            // VFX
            PlayDashVFX();
        }

        // ============================================================
        // HANDLE DASH
        // ============================================================

        void HandleDash()
        {
            m_DashTimer +=
                Time.deltaTime;

            // Tạo velocity Dash
            Vector3 dashVelocity =
                m_DashDirection
                * DashSpeed;

            // Không bay lên
            dashVelocity.y = -2f;

            // Gán lại toàn bộ Vector3
            CharacterVelocity =
                dashVelocity;

            // Di chuyển
            m_Controller.Move(
                CharacterVelocity
                * Time.deltaTime);

            // ========================================================
            // DASH END
            // ========================================================

            if (m_DashTimer >= DashDuration)
            {
                IsDashing = false;

                Vector3 exitVelocity =
                    m_DashDirection
                    * (DashSpeed * 0.15f);

                exitVelocity.y = -2f;

                CharacterVelocity =
                    exitVelocity;
            }
        }

        // ============================================================
        // DASH VFX
        // ============================================================

        void PlayDashVFX()
        {
            if (DashVFX == null)
                return;

            if (DashVFXPoint != null)
            {
                DashVFX.transform.position =
                    DashVFXPoint.position;

                if (m_DashDirection.sqrMagnitude > 0.01f)
                {
                    DashVFX.transform.rotation =
                        Quaternion.LookRotation(
                            -m_DashDirection);
                }
            }

            DashVFX.Stop(true);

            DashVFX.Play(true);
        }

        // ============================================================
        // DASH UI
        // ============================================================

        void UpdateDashUI()
        {
            if (DashIcon == null)
                return;

            float remaining =
                DashCooldownRemaining;

            // READY
            if (remaining <= 0f)
            {
                DashIcon.fillAmount = 1f;

                Color color =
                    DashIcon.color;

                color.a = 1f;

                DashIcon.color =
                    color;

                return;
            }

            // COOLDOWN
            float progress =
                1f -
                remaining / DashCooldown;

            DashIcon.fillAmount =
                progress;

            Color cooldownColor =
                DashIcon.color;

            cooldownColor.a =
                DashCooldownAlpha;

            DashIcon.color =
                cooldownColor;
        }

        // ============================================================
        // GROUND CHECK
        // ============================================================

        void GroundCheck()
        {
            float chosenGroundCheckDistance =
                IsGrounded
                    ? m_Controller.skinWidth
                    + GroundCheckDistance
                    : k_GroundCheckDistanceInAir;

            IsGrounded = false;

            m_GroundNormal =
                Vector3.up;

            if (Time.time >=
                m_LastTimeJumped
                + k_JumpGroundingPreventionTime)
            {
                if (Physics.CapsuleCast(
                    GetCapsuleBottomHemisphere(),
                    GetCapsuleTopHemisphere(
                        m_Controller.height),
                    m_Controller.radius,
                    Vector3.down,
                    out RaycastHit hit,
                    chosenGroundCheckDistance,
                    GroundCheckLayers,
                    QueryTriggerInteraction.Ignore))
                {
                    m_GroundNormal =
                        hit.normal;

                    if (
                        Vector3.Dot(
                            hit.normal,
                            transform.up) > 0f
                        &&
                        IsNormalUnderSlopeLimit(
                            m_GroundNormal))
                    {
                        IsGrounded = true;

                        if (hit.distance >
                            m_Controller.skinWidth)
                        {
                            m_Controller.Move(
                                Vector3.down
                                * hit.distance);
                        }
                    }
                }
            }
        }

        // ============================================================
        // SLOPE
        // ============================================================

        bool IsNormalUnderSlopeLimit(
            Vector3 normal)
        {
            return Vector3.Angle(
                transform.up,
                normal)
                <= m_Controller.slopeLimit;
        }

        public Vector3
            GetDirectionReorientedOnSlope(
                Vector3 direction,
                Vector3 slopeNormal)
        {
            Vector3 directionRight =
                Vector3.Cross(
                    direction,
                    transform.up);

            return Vector3.Cross(
                slopeNormal,
                directionRight).normalized;
        }

        // ============================================================
        // CAPSULE
        // ============================================================

        Vector3 GetCapsuleBottomHemisphere()
        {
            return transform.position
                + transform.up
                * m_Controller.radius;
        }

        Vector3 GetCapsuleTopHemisphere(
            float atHeight)
        {
            return transform.position
                + transform.up
                * (atHeight
                - m_Controller.radius);
        }

        // ============================================================
        // CHARACTER HEIGHT / CAMERA
        // ============================================================

        void UpdateCharacterHeight(
            bool force)
        {
            Vector3 targetCameraPosition =
                Vector3.up
                * m_TargetCharacterHeight
                * CameraHeightRatio;

            // Hạ camera khi Dash
            if (IsDashing)
            {
                targetCameraPosition +=
                    Vector3.down
                    * DashCameraDrop;
            }

            if (force)
            {
                m_Controller.height =
                    m_TargetCharacterHeight;

                m_Controller.center =
                    Vector3.up
                    * m_Controller.height
                    * 0.5f;

                PlayerCamera.transform.localPosition =
                    targetCameraPosition;

                m_Actor.AimPoint.transform.localPosition =
                    m_Controller.center;
            }
            else
            {
                if (
                    m_Controller.height !=
                    m_TargetCharacterHeight
                    || IsDashing)
                {
                    m_Controller.height =
                        Mathf.Lerp(
                            m_Controller.height,
                            m_TargetCharacterHeight,
                            CrouchingSharpness
                            * Time.deltaTime);

                    m_Controller.center =
                        Vector3.up
                        * m_Controller.height
                        * 0.5f;

                    PlayerCamera.transform.localPosition =
                        Vector3.Lerp(
                            PlayerCamera.transform.localPosition,
                            targetCameraPosition,
                            DashCameraSharpness
                            * Time.deltaTime);

                    m_Actor.AimPoint.transform.localPosition =
                        m_Controller.center;
                }
            }
        }

        // ============================================================
        // CROUCH
        // ============================================================

        bool SetCrouchingState(
            bool crouched,
            bool ignoreObstructions)
        {
            if (crouched)
            {
                m_TargetCharacterHeight =
                    CapsuleHeightCrouching;
            }
            else
            {
                if (!ignoreObstructions)
                {
                    Collider[] standingOverlaps =
                        Physics.OverlapCapsule(
                            GetCapsuleBottomHemisphere(),
                            GetCapsuleTopHemisphere(
                                CapsuleHeightStanding),
                            m_Controller.radius,
                            -1,
                            QueryTriggerInteraction.Ignore);

                    foreach (Collider c
                        in standingOverlaps)
                    {
                        if (c != m_Controller)
                        {
                            return false;
                        }
                    }
                }

                m_TargetCharacterHeight =
                    CapsuleHeightStanding;
            }

            if (OnStanceChanged != null)
            {
                OnStanceChanged.Invoke(
                    crouched);
            }

            IsCrouching =
                crouched;

            return true;
        }
    }
}