using Obi;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    [SerializeField] private GameObject fishingCastBar;
    [SerializeField] private GameObject bobberGO;
    [SerializeField] private GameObject basicFishingPoleBody;
    [SerializeField] private GameObject basicFishingPoleHand;
    [SerializeField] private BuyItem buyItem;
    private Scrollbar fishingCastScrollBar;
    private Rigidbody rb;
    private float fishingCastScrollBarValue;
    private float rotationSmoothVelocity;
    private readonly float _playerRotationSpeed = 0f;
    private bool gamepadTriggerHeld, spaceKeyHeld;
    private Camera cam;
    private readonly float forceToAddToBobber = 75f;
    private float castStrength;
    [HideInInspector] public float playerYRotation;
    public bool hasBobberReset = false;
    [SerializeField] private Transform bobberResetPoint;
    // Referenced in Bobber.cs script
    [HideInInspector] public bool IsCastInputCurrentlyHeld => gamepadTriggerHeld || spaceKeyHeld;
    [HideInInspector] public bool pressedInputToCatchFish;
    private bool wasInputHeld;
    private bool hasForceBeenAdded = false;
    [SerializeField] private BobberObject bobberScript;

    private bool bobberPosReset;
    private readonly WaitForSeconds secondstoKeepBobberAsKinematic = new WaitForSeconds(0.45f);

    [Title("Text Fields")]
    [SerializeField] private GameObject fishingTextObj;
    private readonly WaitForSeconds turnOffOutOfBaitTextTime = new WaitForSeconds(0.75f);

    [Title("Animancer Fishing Animations")]
    private CharacterAnimations characterAnimationsScript;

    [Title("Obi Rope Fields")]
    public ObiRopeCursor fishingPoleInHandCursor;
    public ObiRope rope;
    public ObiSolver obiSolver;
    public bool castingOut;
    private float ropeRestLength;
    private readonly WaitForSeconds secondsToToggleObiSolverDamping = new WaitForSeconds(1f);
    private readonly float lengthToChangeRope = 12f;

    [Title("Access Worms")]
    private WormCountUI wormScript;

    [Title("SFX Related")]
    private bool hasWindUpPlayed;

    private void Awake()
    {
        if (fishingCastBar == null) fishingCastBar = FindInactiveObjects.FindInActiveObjectByTag("Fishing Cast Bar");
        fishingCastScrollBar = fishingCastBar.GetComponent<Scrollbar>();
        if (basicFishingPoleBody == null && buyItem.wasBasicFishingPoleBought) basicFishingPoleBody = GameObject.FindWithTag("BasicPole");
        if (basicFishingPoleHand == null && buyItem.wasBasicFishingPoleBought) basicFishingPoleHand = GameObject.FindWithTag("BasicPole");
        if (bobberGO == null) bobberGO = FindInactiveObjects.FindInActiveObjectByTag("Bobber");
        rb = bobberGO.GetComponent<Rigidbody>();
        if (fishingTextObj == null) fishingTextObj = FindInactiveObjects.FindInActiveObjectByTag("Out of Worms");
    }

    private void Start()
    {
        StartCoroutine(FindWormManager());
        fishingCastScrollBarValue = fishingCastScrollBar.value;
        cam = Camera.main;
        characterAnimationsScript = GetComponent<CharacterAnimations>();

        if (basicFishingPoleBody == null) basicFishingPoleBody = GameObject.FindWithTag("BasicPole");
        if (buyItem.wasBasicFishingPoleBought) basicFishingPoleBody.SetActive(true);

        ropeRestLength = rope.restLength;
    }

    private void Update()
    {
        SetFishingPolePosition();

        // Begin Fishing Set Up.
        if (IsCastInputCurrentlyHeld)
        {
            ResetFishingLineComponents();
            if (bobberScript.isBobberInWater) StartCoroutine(ToggleObiSolverDamping());
            PlayCastingAnimations();
            PlayWindUpSFX();
            DisplayAndIncrementCastingBar();
            wasInputHeld = true;
            ResetBobberPosBeforeCasting();
        }
        else if (wasInputHeld && !IsCastInputCurrentlyHeld) // Cast! 
        {
            StopWindUpSFX();
            CastBobber();
            HideCastingBar();
            wasInputHeld = false;
            bobberPosReset = false;
            hasWindUpPlayed = false;
        }

        IncreaseObiRopeLength();
    }

    private void FixedUpdate()
    {
        AddForceToBobber(castStrength);
    }

    private IEnumerator FindWormManager()
    {
        // Wait a frame after loading PlayerUI scene additively, otherwise will be NRE.
        yield return null;
        wormScript = GameObject.FindWithTag("Worm Manager").GetComponent<WormCountUI>();
    }

    public void ResetBobberPosBeforeCasting()
    {  
        if (!bobberPosReset)
        {
            rb.isKinematic = true;
            bobberGO.transform.position = bobberResetPoint.transform.position;
            StartCoroutine(HoldBobberAsKinematicLength());
        }
    }

    private IEnumerator HoldBobberAsKinematicLength()
    {
        // This is to suspend the bobber at the pole as the rope winds in so it's not flying everywhere.
        yield return secondstoKeepBobberAsKinematic;
        rb.isKinematic = false;
        bobberPosReset = true;
    }

    private void IncreaseObiRopeLength()
    {
        if (castingOut && bobberScript.fishingSurface == null || castingOut && !bobberScript.hitGroundFirst)
            fishingPoleInHandCursor.ChangeLength(rope.restLength + lengthToChangeRope * Time.deltaTime);
    }

    private void DisplayAndIncrementCastingBar()
    {
        fishingCastBar.SetActive(true);
        fishingCastScrollBarValue += Time.deltaTime * 0.55f;
        if (fishingCastScrollBar == null) fishingCastScrollBar = GameObject.FindWithTag("Fishing Cast Bar").GetComponent<Scrollbar>();
        fishingCastScrollBar.value = fishingCastScrollBarValue;
    }

    private void HideCastingBar()
    {
        fishingCastBar.SetActive(false);
        fishingCastScrollBarValue = 0f;
    }

    private void PlayCastingAnimations()
    {
        if (fishingCastScrollBarValue < 1f) characterAnimationsScript.PlayFishingCastAnim();
        else if (fishingCastScrollBarValue >= 1f) characterAnimationsScript.PauseFishingCastAnim();
    }

    private void SetFishingPolePosition()
    {
        if (!buyItem.wasBasicFishingPoleBought) return;

        if (basicFishingPoleHand == null && buyItem.wasBasicFishingPoleBought) basicFishingPoleHand = GameObject.FindWithTag("BasicPole");

        bool switchToFishingStance = InputManager.Instance.fishingControlsEnabled && buyItem.wasBasicFishingPoleBought;
        bool switchToWalkingStance = !InputManager.Instance.fishingControlsEnabled && buyItem.wasBasicFishingPoleBought;

        if (switchToFishingStance)
        {
            basicFishingPoleHand.SetActive(true);
            basicFishingPoleBody.SetActive(false);
            EnableStationaryFishingMode();
        }
        else if (switchToWalkingStance)
        {
            if (basicFishingPoleHand.activeInHierarchy)
            {
                ResetFishingLineComponents();
                bobberScript.ResetFishingComponents();
            }

            basicFishingPoleBody.SetActive(true);
            basicFishingPoleHand.SetActive(false);
        }

        // Local function.
        void EnableStationaryFishingMode()
        {
            if (!IsCastInputCurrentlyHeld) characterAnimationsScript.PlayFishingIdleAnim();

            // Prevent player from rotating while fishing.
            if (castingOut || bobberScript.isBobberInWater) return;

            Vector3 direction = new Vector3(0f, 0f, 0f).normalized;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            playerYRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSmoothVelocity, _playerRotationSpeed);
            transform.rotation = Quaternion.Euler(0f, playerYRotation, 0f);
        }
    }

    public void ResetFishingLineComponents()
    {
        hasForceBeenAdded = false;
        castingOut = false;
        bobberScript.DisableAllBobberColliders();

        if (!hasBobberReset)
        {
            RemoveBobberForces();
            StartCoroutine(SetBool());
        }

        if (fishingPoleInHandCursor != null) fishingPoleInHandCursor.ChangeLength(ropeRestLength);
    }

    private IEnumerator SetBool()
    {
        yield return new WaitForSeconds(0.75f);
        hasBobberReset = true;
    }

    private void RemoveBobberForces()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public IEnumerator ToggleObiSolverDamping()
    {
        obiSolver.parameters.damping = 0.98f;
        obiSolver.PushSolverParameters();

        yield return secondsToToggleObiSolverDamping;

        // Reset to default value.
        obiSolver.parameters.damping = 0f;
        obiSolver.PushSolverParameters();
    }

    private void AddForceToBobber(float fishingScrollBarCastStrength)
    {
        if (castingOut && !hasForceBeenAdded)
        {
            Vector3 direction = transform.forward;
            direction.y = 0.6f;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce((forceToAddToBobber * fishingScrollBarCastStrength) * direction, ForceMode.Impulse);
            hasForceBeenAdded = true;
        }
    }

    private void CastBobber()
    {
        if (wormScript.wormCount <= 0)
        {
            fishingTextObj.SetActive(true);
            StartCoroutine(TurnOffOutOfBaitText());
            return;
        }

        wormScript.SubtractWorm(1);
        castStrength = fishingCastScrollBarValue;
        castingOut = true;
        PlayCastSwishSFX();
        PlayLineGoingOutSFX();
        StartCoroutine(WaitAFrameToEnableBobberColliders());
    }

    private IEnumerator WaitAFrameToEnableBobberColliders()
    {
        yield return null;
        bobberScript.EnableAllBobberColliders();
    }

    private IEnumerator TurnOffOutOfBaitText()
    {
        yield return turnOffOutOfBaitTextTime;
        fishingTextObj.SetActive(false);
    }

    #region SFX
    private void PlayCastSwishSFX()
    {
        // FISH_SOUND
        if (!GameSounds.Instance._audioSource.isPlaying)
        {
            GameSounds.Instance.PlayOneShotAudio(SoundType.castingSwish, 1f);
        }
    }

    private void PlayWindUpSFX()
    {
        // FISH_SOUND
        if (!hasWindUpPlayed)
        {
            GameSounds.Instance.PlayWindUpSFX();
            hasWindUpPlayed = true;
        }
    }

    private void StopWindUpSFX()
    {
        // FISH_SOUND
        GameSounds.Instance.StopWindUpSFX();
    }

    private void PlayLineGoingOutSFX()
    {
        // FISH_SOUND
        if (castingOut)
        {
            GameSounds.Instance.PlayOneShotAudio(SoundType.lineGoingOut, 3.5f);
        }
    }
    #endregion

    #region Input
    /* forum.unity.com/threads/mouse-clicks-not-detected-in-new-input-system.1160315 */
    public void Cast(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            gamepadTriggerHeld = context.control.IsPressed();
            spaceKeyHeld = context.control.IsPressed();
        }
    }

    public void CatchFishInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pressedInputToCatchFish = context.control.IsPressed();
        }
    }
    #endregion
}
