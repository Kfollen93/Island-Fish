using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PreviouslyCaughtFishUI : MonoBehaviour
{
    [Title("First Time Fish Caught")]
    [SerializeField] private Image firstTimeCatchingFishHolder;
    [SerializeField] private Image firstTimeCatchingImage;
    [SerializeField] private TMP_Text firstTryMaxSizeCaughtText;
    private bool isFirstTimeFishCaughtUiBeingDisplayed;

    [Title("Fish Caught More Than Once")]
    private Image defaultImage;
    [SerializeField] private TMP_Text previouslyCaughtFishText;

    private FishSlot[] allFishSlots;
    private readonly List<string> childrenNames = new List<string>();

    // Rect for DoTween
    private RectTransform firstTimeFishCaughtRectTrans;
    private RectTransform prevCaughtFishRectTrans;

    // This subscription MUST be initialized in Start(), AFTER FishSlot.cs is finished being initalized in Awake().
    void Start()
    {
        defaultImage = GetComponent<Image>();
        FishCaughtEvent.OnFishCaught += DisplayOnPlayerScreenCaughtFishUi;
        GetAllFishSlots();

        firstTimeFishCaughtRectTrans = (RectTransform)firstTimeCatchingFishHolder.transform;
        prevCaughtFishRectTrans = (RectTransform)this.transform;
    }

    private void GetAllFishSlots()
    {
        allFishSlots = FindObjectsOfType<FishSlot>(true);
        Array.Sort(allFishSlots, (a, b) => a.name.CompareTo(b.name));

        // Caching the list of slot children in a list to loop through later.
        // allFishSlots consists of all the FishSlot.cs scripts that are on the 'Slot's in the journal.
        // I am getting the Slot obj that has the FishSlot.cs script attached, and then going to its child
        // where there I get the name of the child.  I.E: Slots Holder -> Slot -> Salmon.
        foreach (var slot in allFishSlots)
        {
            Transform slotGameobjectTransform = slot.gameObject.transform;
            childrenNames.Add(slotGameobjectTransform.GetChild(0).name);
        }  
    }

    private void Update()
    {
        ClearFirstTimeCaughtFishUi();
    }

    private void DisplayOnPlayerScreenCaughtFishUi(FishSO caughtFishSO, int fishSize)
    {
        FishSlot fishScript = null;
        for (int i = 0; i < childrenNames.Count; i++)
        {
            if (childrenNames[i] == caughtFishSO.fishName)
            {
                fishScript = allFishSlots[i];
                break;
            }
        }

        // Find the fishStats from indexing the key on the found script.
        FishStats statsOfCaughtFish = fishScript.fishCollected[caughtFishSO.fishName];

        bool firstTimeCatchingFishAndFishIsNotMaxSize = statsOfCaughtFish.numOfFishCaught == 1 && fishSize != caughtFishSO.maxLength;
        bool firstTimeCatchingFishAndFishIsMaxSize = statsOfCaughtFish.numOfFishCaught == 1 && fishSize == caughtFishSO.maxLength;
        bool previouslyCaughtFishAndFishNotMaxSize = statsOfCaughtFish.numOfFishCaught >= 1 && fishSize != caughtFishSO.maxLength;

        if (firstTimeCatchingFishAndFishIsNotMaxSize)
            DisplayFishFirstTimeFishCaught(caughtFishSO);
        else if (firstTimeCatchingFishAndFishIsMaxSize)
            DisplayOnFirstCatchCaughtMaxSize(caughtFishSO);
        else if (previouslyCaughtFishAndFishNotMaxSize) 
            DisplayPreviouslyCaughtFish(caughtFishSO);
        else 
            DisplayPrevCaughtFishMaxSize(caughtFishSO);
    }

    private void DisplayPrevCaughtFishMaxSize(FishSO caughtFishSO)
    {
        defaultImage.sprite = caughtFishSO.fishImage;
        previouslyCaughtFishText.text = "Max Size Caught!";
        SlidePrevCaughtFishTweenUi();
        GameSounds.Instance.PlayMaxSizeFishCaughtSFX();
    }

    private void DisplayPreviouslyCaughtFish(FishSO caughtFishSO)
    {
        defaultImage.sprite = caughtFishSO.fishImage;
        previouslyCaughtFishText.text = string.Empty;
        SlidePrevCaughtFishTweenUi();
        GameSounds.Instance.PlayGeneralBoopSFX();
    }

    private void DisplayOnFirstCatchCaughtMaxSize(FishSO caughtFishSO)
    {
        firstTimeCatchingImage.sprite = caughtFishSO.fishImage;
        firstTryMaxSizeCaughtText.text = "First Try Max Size Caught! Well Done!";
        TweenFirstTimeCaughtUi();
        GameSounds.Instance.PlayMaxSizeFishCaughtSFX();
        isFirstTimeFishCaughtUiBeingDisplayed = true;
    }

    private void DisplayFishFirstTimeFishCaught(FishSO caughtFishSO)
    {
        firstTimeCatchingImage.sprite = caughtFishSO.fishImage;
        firstTryMaxSizeCaughtText.text = string.Empty;
        TweenFirstTimeCaughtUi();
        GameSounds.Instance.PlayFirstTimeFishCaughtSFX();
        isFirstTimeFishCaughtUiBeingDisplayed = true;
    }

    private void ClearFirstTimeCaughtFishUi()
    {
        if (isFirstTimeFishCaughtUiBeingDisplayed && Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame || isFirstTimeFishCaughtUiBeingDisplayed && Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame)
        {
            isFirstTimeFishCaughtUiBeingDisplayed = false;
            ResetFirstTimeCaughtUiTween();
        }
    }

    // Not displaying Size here anymore, will need to move this to the journal page, or implement visual slider rather than text.
    private string ConvertingInchesToStringFormat(int randomSize)
    {
        // Convert inches to feet.
        if (randomSize % 12 == 0)
        {
            int feet = randomSize / 12;
            return $"{feet} foot.";
        }
        else
        {
            int feet = randomSize / 12;
            int inches = randomSize % 12;

            if (feet == 0)
            {
                if (inches > 1)
                    return $"{inches} inches";
                else
                    return $"{inches} inch";
            }

            return inches > 1 ? $"{feet} feet and {inches} inches." : $"{feet} feet and {inches} inch";
        }
    }

    private void ResetFirstTimeCaughtUiTween() => firstTimeCatchingFishHolder.transform.DOMoveY(3000f, 0.25f);

    private void TweenFirstTimeCaughtUi()
    {
        var sequence = DOTween.Sequence();
        firstTimeFishCaughtRectTrans.DOAnchorPosY(800f, .75f);
        sequence.Append(firstTimeCatchingFishHolder.transform.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 0.9f))
                .Append(firstTimeCatchingFishHolder.transform.DOScale(new Vector3(1f, 1f, 1f), 1f)).SetLoops(-1, LoopType.Restart);
    }

    private void SlidePrevCaughtFishTweenUi()
    {
        prevCaughtFishRectTrans.DOAnchorPosX(30f, .30f);
        prevCaughtFishRectTrans.DOAnchorPosX(-200f, .25f).SetDelay(2f);
    }
}
