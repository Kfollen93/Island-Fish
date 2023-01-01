using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BobberObject : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject player;
    private Fishing fishingScript;
    private ParticleSystem bobberSplashVFX;
    private ParticleSystem fishOnLineVFX;
    private bool isFishOnLineParticlesPlaying;

    [Title("Text Fields")]
    private GameObject bobberTextObj;
    private TMP_Text editBobberText;

    [Title("Scroll Bar Fields")]
    private GameObject fishingCatchBar;
    private Scrollbar fishingCatchScrollBar;
  
    private float fishingCatchScrollBarValue;
    private const float DefaultCatchSpeed = 0.05f;
    private const float MaxCatchValue = 1.0f;
    [HideInInspector] public bool isFishOnTheLine;
    private float randomNumToCatchFish;
    private float catchTimeRemaining = 0f;
    [HideInInspector] public bool isBobberInWater;
    [HideInInspector] public bool hitGroundFirst;
    private bool SetDefaultFishingComponents =>
                 !InputManager.Instance.fishingControlsEnabled || isBobberInWater && fishingScript.IsCastInputCurrentlyHeld
                 || isBobberInWater && !isFishOnTheLine && fishingScript.pressedInputToCatchFish;

    [Title("Scriptable Objects Fish")]
    private bool hasFishBeenChosen;
    private FishSO randomFish;
    [HideInInspector] public FishableSurface fishingSurface;
    private bool caughtFish;
    [HideInInspector] public string nameOfFish;

    private Collider[] bobberColliders;

    [Title("Bobber Movement when Fish is On Line")]
    private Vector3 startPos;
    private Vector3 endPos;
    private float randomX, randomZ;
    private Vector3 targetPos;
    private readonly float speed = 8f;

    [Title("UI Bobber Text Fade")]
    private Tween fadeTween;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        fishingScript = player.GetComponent<Fishing>();
        bobberTextObj = GameObject.FindWithTag("BobberText");
        editBobberText = bobberTextObj.GetComponent<TMP_Text>();
        bobberSplashVFX = transform.GetChild(1).GetComponent<ParticleSystem>();
        fishOnLineVFX = transform.GetChild(2).GetComponent<ParticleSystem>();
        fishingCatchBar = FindInactiveObjects.FindInActiveObjectByTag("FishingCatchBar");
        fishingCatchScrollBar = fishingCatchBar.GetComponent<Scrollbar>();
        fishingCatchScrollBarValue = fishingCatchScrollBar.value;
        bobberColliders = GetComponents<Collider>();
        foreach (var collider in bobberColliders) Physics.IgnoreCollision(collider, player.GetComponent<Collider>());
    }

    // OnTriggerEnter used to check if Bobber went through a trigger that's placed on water surfaces for fishing.
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Water") && !isBobberInWater)
        {
            fishingSurface = col.GetComponentInParent<FishableSurface>();
            if (fishingSurface != null && !hitGroundFirst)
            {
                isBobberInWater = true;
                PlayBobberHitWaterSFX();
                SetBobberMovementFields();

                transform.rotation = Quaternion.identity;
                rb.isKinematic = true;
                fishingScript.castingOut = false;
                bobberSplashVFX.Play();
                randomNumToCatchFish = Random.Range(0.1f, 0.9f);
            }
        }
    }

    // OnCollisionEnter used to check if when casting, the bobber doesn't land in water.
    private void OnCollisionEnter(Collision collision)
    {
        if (!fishingScript.castingOut) return;

        hitGroundFirst = true;
        PlayBobberHitGroundSFX();
        HandleNoWaterText();
        StartCoroutine(fishingScript.ToggleObiSolverDamping());
        fishingScript.ResetFishingLineComponents();
        fishingScript.ResetBobberPosBeforeCasting();
    }

    private void SetBobberMovementFields()
    {
        startPos = transform.position;
        randomX = Random.Range(-3f, 4f);
        randomZ = Random.Range(-3f, 4f);
        endPos = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        targetPos = endPos;
    }

    private void Update()
    {
        if (isBobberInWater)
        {
            StartCatchFishTimer();
            PlayLineNoisesWaitingForFishSFX();
            CatchFish(randomNumToCatchFish);
        }

        if (SetDefaultFishingComponents) ResetFishingComponents();
    }

    void FixedUpdate()
    {
        MoveBobberWhenFishOnLine();
    }

    private void StartCatchFishTimer()
    {
        fishingCatchBar.SetActive(true);
        fishingCatchScrollBarValue -= Time.deltaTime * DefaultCatchSpeed;
        fishingCatchScrollBar.value = fishingCatchScrollBarValue;
    }

    private void CatchFish(float randomNum)
    {   
        // Allow 0.01f of imprecision.
        if (Mathf.Abs(randomNum - fishingCatchScrollBarValue) < 0.01f)
            isFishOnTheLine = true;

        if (isFishOnTheLine)
        {
            PlayFishOnLineVFX();
            
            if (!hasFishBeenChosen)
            {
                randomFish = fishingSurface.availableFish.GetRandomFishFromSet();
                hasFishBeenChosen = true;
                catchTimeRemaining = Random.Range(0.3f, 1.5f);
                PlayFishOnLineSplashingSFX();
            }

            catchTimeRemaining -= Time.deltaTime;
            if (catchTimeRemaining > 0f && fishingScript.pressedInputToCatchFish && !caughtFish)
            {
                int fishSize = GetRandomFishSize(randomFish);
                FishCaughtEvent.CaughtFish(randomFish, fishSize);
                caughtFish = true;
                StartCoroutine(fishingScript.ToggleObiSolverDamping());
                ResetFishingComponents();
                fishingScript.ResetFishingLineComponents();
                StopFishOnLineVFX();
                ProgessBar.IncrementFishingSkillBar(1);
            }
            else if (catchTimeRemaining <= 0f)
            {
                StartCoroutine(fishingScript.ToggleObiSolverDamping());
                ResetFishingComponents();
                HandleFishGotAwayText();
                fishingScript.ResetFishingLineComponents();
                fishingScript.ResetBobberPosBeforeCasting();
                StopFishOnLineVFX();
                PlayReelInNoFishSFX();
            }
        }
        else if (!isFishOnTheLine && fishingScript.pressedInputToCatchFish)
        {
            rb.isKinematic = false;
            StartCoroutine(fishingScript.ToggleObiSolverDamping());
            ResetFishingComponents();
            fishingScript.ResetFishingLineComponents();
            fishingScript.ResetBobberPosBeforeCasting();
            StopFishOnLineVFX();
            PlayReelInNoFishSFX();
        }
    }

    private int GetRandomFishSize(FishSO caughtFishSO)
    {
        int fishMinLength = caughtFishSO.minLength;
        int fishMaxLength = caughtFishSO.maxLength + 1; // Add +1 to maxLength since with Random.Range() the max is exclusive.
        int randomSize = Random.Range(fishMinLength, fishMaxLength);
        return randomSize;
    }

    public void DisableAllBobberColliders()
    {
        foreach (Collider col in bobberColliders)
            col.enabled = false;
    }

    public void EnableAllBobberColliders()
    {
        foreach (Collider col in bobberColliders)
            col.enabled = true;
    }

    private void MoveBobberWhenFishOnLine()
    {
        if (isFishOnTheLine)
        {
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            Vector3 currentPos = transform.position;
            float distFromCurrToEnd = Mathf.Abs(currentPos.x - endPos.x);
            float distFromCurrToStart = Mathf.Abs(currentPos.x - startPos.x);
            if (distFromCurrToEnd <= 0.5f)
            {
                targetPos = startPos;
            }
            else if (distFromCurrToStart <= 0.5f)
            {
                targetPos = endPos;
            }

            Vector3 targetDirection = (targetPos - currentPos).normalized;
            rb.MovePosition(currentPos + speed * Time.deltaTime * targetDirection);
        }
        else
        {
            rb.interpolation = RigidbodyInterpolation.None;
        }
    }

    private void PlayFishOnLineVFX()
    {
        if (!fishOnLineVFX.isPlaying && !isFishOnLineParticlesPlaying)
        {
            fishOnLineVFX.Play();
            isFishOnLineParticlesPlaying = true;
        }
    }

    private void StopFishOnLineVFX()
    {
        if (fishOnLineVFX.isPlaying && isFishOnLineParticlesPlaying)
        {
            fishOnLineVFX.Stop();
            isFishOnLineParticlesPlaying = false;
        }
    }

    private void HandleFishGotAwayText()
    {
        DisplayBobberText("Fish Got Away!");
        FadeOut(1.5f);
    }

    private void HandleNoWaterText()
    {
        DisplayBobberText("No Water!");
        FadeOut(1f);
        hitGroundFirst = false;
    }

    private void DisplayBobberText(string bobberTxt)
    {
        editBobberText.alpha = 1.0f;
        editBobberText.text = bobberTxt;
    }

    public void ResetFishingComponents()
    {
        isFishOnTheLine = false;
        fishingCatchScrollBar.value = MaxCatchValue;
        fishingCatchScrollBarValue = fishingCatchScrollBar.value;
        fishingCatchBar.SetActive(false);
        isBobberInWater = false;
        StopFishOnLineVFX();
        fishingScript.pressedInputToCatchFish = false;
        hasFishBeenChosen = false;
        rb.isKinematic = false;
        fishingSurface = null;
        caughtFish = false;
    }

    #region SFX
    private void PlayBobberHitWaterSFX()
    {
        if (!GameSounds.Instance._audioSource.isPlaying) GameSounds.Instance.PlayOneShotAudio(SoundType.bobberHitWater, 1f);
    }

    private void PlayBobberHitGroundSFX()
    {
        if (!GameSounds.Instance._audioSource.isPlaying) GameSounds.Instance.PlayOneShotAudio(SoundType.bobberHitGround, 1f);
    }

    private void PlayLineNoisesWaitingForFishSFX()
    {
        if (!GameSounds.Instance._audioSource.isPlaying) GameSounds.Instance.PlayOneShotAudio(SoundType.lineNoisesWaitingForFish, 1f);
    }

    private void PlayReelInNoFishSFX()
    {
        GameSounds.Instance.PlayOneShotAudio(SoundType.reelingInNoCaught, 1f);
    }

    private void PlayFishOnLineSplashingSFX()
    {
        GameSounds.Instance.PlayOneShotAudio(SoundType.fishOnLineSplashingInWater, 1f);
    }
    #endregion

    #region UI Bobber Text Fade
    private void Fade(float endValue, float duration, TweenCallback onEnd)
    {
        fadeTween?.Kill(false);
        fadeTween = editBobberText.DOFade(endValue, duration);
        fadeTween.onComplete += onEnd;
    }
    private void FadeOut(float duration) => Fade(0f, duration, () => { });

    #endregion
}