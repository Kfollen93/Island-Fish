using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IDataPersistence
{
    [Title("Input --> Using Unity Events")]
    private Vector2 moveInput;
    private bool holdingRunButton;

    [Title("Character Variables")]
    [SerializeField] private float walkSpeed = 1.1f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxWalkSpeed = 1.1f;
    [SerializeField] private float runSpeed = 2.25f;
    [SerializeField] private float maxRunSpeed = 2.25f;

    private Rigidbody rb;
    private CharacterAnimations characterAnimationsScript;

    private ParticleSystem footstepDust;
    private bool psIsPlaying;

    public void LoadData(GameData data) => transform.position = data.playerPosition;
    public void SaveData(GameData data) => data.playerPosition = transform.position;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        characterAnimationsScript = GetComponent<CharacterAnimations>();
        footstepDust = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable() => characterAnimationsScript.StartingIdleAnim();

    private void Update()
    {
        RaycastGroundCheck();
        ToggleBetweenLocomotionAnimations();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        PlayerMovement();
        LookAt();
    }

    private void ToggleBetweenLocomotionAnimations()
    {
        if (holdingRunButton && (Mathf.Abs(moveInput.x) > 0 || Mathf.Abs(moveInput.y) > 0))
            characterAnimationsScript.PlayRunAnim();
        else if (!holdingRunButton && (Mathf.Abs(moveInput.x) > 0 || Mathf.Abs(moveInput.y) > 0))
            characterAnimationsScript.PlayWalkAnim();
    }

    private void PlayerMovement()
    {
        Vector3 forceDirection = Vector3.zero;
        if (!holdingRunButton)
        {
            forceDirection += moveInput.x * walkSpeed * GetCameraRight(Camera.main);
            forceDirection += moveInput.y * walkSpeed * GetCameraForward(Camera.main);
        }
        else if (holdingRunButton)
        {
            forceDirection += moveInput.x * runSpeed * GetCameraRight(Camera.main);
            forceDirection += moveInput.y * runSpeed * GetCameraForward(Camera.main);
        }

        // Player is given input
        if (Mathf.Abs(moveInput.x) > 0 || Mathf.Abs(moveInput.y) > 0)
        {
            // Move Player
            rb.AddForce(forceDirection, ForceMode.Impulse);

            // Play footstep sounds.
            if (!GameSounds.Instance._audioSource.isPlaying && !holdingRunButton)
                GameSounds.Instance.PlayOneShotAudio(SoundType.walkingFootsteps, 1f);
            else if (!GameSounds.Instance._audioSource.isPlaying && holdingRunButton)
                GameSounds.Instance.PlayOneShotAudio(SoundType.sprintingFootsteps, 1f);

            // Play footsteps particles
            if (!psIsPlaying)
            {
                psIsPlaying = true;
                footstepDust.Play();
            }    
        }  
        else if (RaycastGroundCheck())
        {
            psIsPlaying = false;
            footstepDust.Stop();
        }
        else
        {
            psIsPlaying = false;
            footstepDust.Stop();
        }
            
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;

        // Cap the players speed
        if (!holdingRunButton && horizontalVelocity.sqrMagnitude > maxWalkSpeed * maxWalkSpeed)
            rb.velocity = horizontalVelocity.normalized * maxWalkSpeed + Vector3.up * rb.velocity.y;
        else if (holdingRunButton && horizontalVelocity.sqrMagnitude > maxRunSpeed * maxRunSpeed)
            rb.velocity = horizontalVelocity.normalized * maxRunSpeed + Vector3.up * rb.velocity.y;

    }

    private void ApplyGravity()
    {
        if (rb.velocity.y < 0f)
        {
            rb.velocity -= Physics.gravity.y * Time.fixedDeltaTime * 2f * Vector3.down;
        }
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0;

        if (moveInput.sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private bool RaycastGroundCheck()
    {
        return Physics.SphereCast(transform.position + new Vector3(0, 1, 0), 0.1f, Vector3.down, out RaycastHit hit, 1f);
    }

    #region Unity Events
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && RaycastGroundCheck())
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // Drag and Dropped the Player that has this script, onto the Event "Movement" (which is what I named it in the Action Map) in Player Input
    // which is attached to the Player as well.
    public void MoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Shift Button.
    public void Run(InputAction.CallbackContext context)
    {
        holdingRunButton = context.performed;
    } 

    #endregion
}