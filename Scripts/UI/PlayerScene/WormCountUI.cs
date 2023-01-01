using UnityEngine;
using TMPro;

public class WormCountUI : MonoBehaviour, IDataPersistence
{
    public int wormCount; // This value should always match what the value is in the ctor in GameData.cs
    private TMP_Text wormText;

    // Stats Page
    [SerializeField] private TMP_Text journalStatsPageCurrentWorms;

    public void LoadData(GameData data) => wormCount = data.wormCount;
    public void SaveData(GameData data) => data.wormCount = wormCount;

    private void Awake()
    {
        wormText = GetComponent<TMP_Text>();
    }
    public void Start()
    {
        wormText.text = $"Worms: {wormCount}";
        journalStatsPageCurrentWorms.text = wormText.text;
    }

    private void Update()
    {
        ToggleOnScreenWormsText();
        UpdateJournalWormsText();
    }

    private void ToggleOnScreenWormsText()
    {
        if (InputManager.Instance.fishingControlsEnabled)
        {
            wormText.text = $"Worms: {wormCount}";
            return;
        }

        wormText.text = string.Empty;
    }

    private void UpdateJournalWormsText()
    {
        if (InputManager.Instance.journalOpened) journalStatsPageCurrentWorms.text = $"Worms: {wormCount}";
    }

    public void AddWorm(int wormToAdd)
    {
        wormCount += wormToAdd;
        wormText.text = $"Worms: {wormCount}";
    }

    public void SubtractWorm(int wormToSubtract)
    {
        if (wormCount >= wormToSubtract)
        {
            wormCount -= wormToSubtract;
            wormText.text = $"Worms: {wormCount}";
        }
    }
}
