using UnityEngine;

[System.Serializable]
public class GameData
{
    public int playerMoney;
    public Vector3 playerPosition;
    public bool wasBasicFishingPoleBought;
    public int wormCount;
    public int progressBarValue;
    public SerializableDictionary<string, FishStats> serializeableFishCollected;
    public bool hasBasicFishingPoleTipBeenDisplayed;
    public bool hasWormTipBeenDisplayed;

    // Default values when starting a New Game go in this constructor.
    public GameData()
    {
        wormCount = 0;
        progressBarValue = 0;
        playerMoney = 35;
        playerPosition = new Vector3(18.23f, 1.997f, 76.922f);
        wasBasicFishingPoleBought = false;
        serializeableFishCollected = new SerializableDictionary<string, FishStats>();
        hasBasicFishingPoleTipBeenDisplayed = false;
        hasWormTipBeenDisplayed = false;
    }
}
