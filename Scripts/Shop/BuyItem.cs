using System.Collections;
using UnityEngine;

public class BuyItem : MonoBehaviour, IDataPersistence
{
    public int itemCost;
    public int itemInventory; // Set to 1 via the inspector.
    public GameObject itemOnWallToTurnOff;
    public bool wasBasicFishingPoleBought;
    private MoneyManager moneyManager;

    public void LoadData(GameData data) => wasBasicFishingPoleBought = data.wasBasicFishingPoleBought;
    public void SaveData(GameData data) => data.wasBasicFishingPoleBought = wasBasicFishingPoleBought;

    private void Start()
    {
        if (wasBasicFishingPoleBought) itemOnWallToTurnOff.SetActive(false);
        StartCoroutine(FindMoneyManager());
    }

    private IEnumerator FindMoneyManager()
    {
        // Wait a frame after loading PlayerUI scene additively, otherwise will be NRE.
        yield return null;
        moneyManager = GameObject.FindWithTag("Money Manager").GetComponent<MoneyManager>();
    }

    public void BuyItemOnClick()
    {
        // Log player money before attempting to buy item.
        int playerInitialMoney = moneyManager.playerMoney;

        // Attempt to buy item.
        moneyManager.SubtractMoney(itemCost);

        // If player's money decreased, then item was bought
        int playerNewMoneyTotal = moneyManager.playerMoney;
        if (playerNewMoneyTotal < playerInitialMoney && itemInventory > 0)
        {
            itemInventory -= 1;
            itemOnWallToTurnOff.SetActive(false);
            wasBasicFishingPoleBought = true;
        }
    }
}
