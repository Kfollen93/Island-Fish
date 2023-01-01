using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* This script and its methods pertains to the Journal UI */

public class FishSlot : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string nameOfFishHeldWithinSlot;
    [SerializeField] private Material pixelReplacementMat;
    private Material instanceOfSilhouette;
    [SerializeField] private Image imageOfFish;
    private int thisFishCount;
    [SerializeField] private TMP_Text fishTextCount;
    [SerializeField] private TMP_Text fishTextName;
    [SerializeField] private TMP_Text fishDescription;
    [SerializeField] private TMP_Text maxSizeCaughtJournalText;
    [SerializeField] private Slider sizeCaughtJournalSlider;
    [SerializeField] private TMP_Text sizeCaughtJournalText;
    private int largestLengthCaught = 0;
    [SerializeField] private FishSO fishSO = null;
    public Dictionary<string, FishStats> fishCollected = new Dictionary<string, FishStats>();

    public void LoadData(GameData data)
    {
        if (data.serializeableFishCollected.TryGetValue(nameOfFishHeldWithinSlot, out FishStats fish))
        {
            if (fish.numOfFishCaught > 0)
            {
                fishTextCount.text = $"Amount Caught: {fish.numOfFishCaught}";
                fishTextName.text = nameOfFishHeldWithinSlot;
                fishDescription.text = fish.descriptionOfFish;
                instanceOfSilhouette.SetFloat("_Hide", 0);

                SetFishLengthSliderOnLoad(fish);
                PopulateFishCollectionDictionaryUponLoad(fish);
            }
        }
    }

    public void SaveData(GameData data)
    {
        foreach (var kvp in fishCollected)
        {
            if (!data.serializeableFishCollected.ContainsKey(kvp.Key))
                data.serializeableFishCollected.Add(kvp.Key, kvp.Value);
            else
                data.serializeableFishCollected[kvp.Key] = kvp.Value;
        }
    }

    // This slotImage & subscription MUST be initialized in Awake (first), before the sub in PreviouslyCaughtFishUI.cs (done in Start()).
    private void Awake()
    {
        FishCaughtEvent.OnFishCaught += FishCaught;

        instanceOfSilhouette = Instantiate(pixelReplacementMat);
        imageOfFish.material = instanceOfSilhouette;
    }

    private void FishCaught(FishSO caughtFishSO, int fishSize)
    {
        if (fishSO == caughtFishSO)
        {
            ++thisFishCount;

            // Update Journal Page UI values.
            fishTextCount.text = $"Amount Caught: {thisFishCount}";
            fishTextName.text = fishSO.fishName;
            fishDescription.text = fishSO.description;
            if (fishSize == caughtFishSO.maxLength) maxSizeCaughtJournalText.text = "Max Size!";

            // Toggle Shader to Display Fish. 1 is silhouette, and 0 removes it.
            instanceOfSilhouette.SetFloat("_Hide", 0);

            // Update Size Caught Journal Slider.
            SetFishLengthSliderSettings(caughtFishSO, fishSize);

            // Display Biggest Size Caught So Far.
            largestLengthCaught = Mathf.Max(fishSize, largestLengthCaught);
            sizeCaughtJournalText.text = $"Largest Caught: {largestLengthCaught} inches";

            PopulateFishCollectionDictionary(fishSize, caughtFishSO);
        }
    }

    private void PopulateFishCollectionDictionary(int fishSize, FishSO theFishThatWasCaught)
    {
        bool isMaxLength = fishSize == theFishThatWasCaught.maxLength;
        if (fishCollected.TryGetValue(theFishThatWasCaught.fishName, out FishStats statsOfFishCaught))
        {
            statsOfFishCaught.numOfFishCaught++;
            statsOfFishCaught.largestLengthCaught = largestLengthCaught;
            statsOfFishCaught.hasMaxLengthBeenCaught = isMaxLength;
        }
        else
        {
            fishCollected.Add(theFishThatWasCaught.fishName, new FishStats
            {
                descriptionOfFish = theFishThatWasCaught.description,
                minLengthOfFish = theFishThatWasCaught.minLength,
                maxLengthOfFish = theFishThatWasCaught.maxLength,
                numOfFishCaught = 1,
                hasMaxLengthBeenCaught = isMaxLength,
                largestLengthCaught = fishSize
            });
        }
    }

    private void SetFishLengthSliderSettings(FishSO caughtFishSO, int fishSize)
    {
        sizeCaughtJournalSlider.gameObject.SetActive(true);
        sizeCaughtJournalSlider.minValue = caughtFishSO.minLength - 1; // Subtracting 1 so that if you caught minLength the bar will still fill a bit.
        sizeCaughtJournalSlider.maxValue = caughtFishSO.maxLength;
        if (fishSize > sizeCaughtJournalSlider.value) sizeCaughtJournalSlider.value = fishSize;
    }

    private void SetFishLengthSliderOnLoad(FishStats fish)
    {
        sizeCaughtJournalSlider.gameObject.SetActive(true);
        sizeCaughtJournalSlider.minValue = fish.minLengthOfFish - 1; // Subtracting 1 so that if you caught minLength the bar will still fill a bit.
        sizeCaughtJournalSlider.maxValue = fish.maxLengthOfFish;
        sizeCaughtJournalSlider.value = fish.largestLengthCaught;
        sizeCaughtJournalText.text = $"Largest Caught: {fish.largestLengthCaught} inches";
    }

    private void PopulateFishCollectionDictionaryUponLoad(FishStats fish)
    {
        if (fishCollected.ContainsKey(nameOfFishHeldWithinSlot)) return;

        fishCollected.Add(nameOfFishHeldWithinSlot, fish);
        thisFishCount = fish.numOfFishCaught;
        largestLengthCaught = fish.largestLengthCaught;
        if (fish.hasMaxLengthBeenCaught) maxSizeCaughtJournalText.text = "Max Size!";
    }
}

[System.Serializable]
public class FishStats
{
    public string descriptionOfFish;
    public int minLengthOfFish;
    public int maxLengthOfFish;
    public int numOfFishCaught;
    public bool hasMaxLengthBeenCaught;
    public int largestLengthCaught;
}
