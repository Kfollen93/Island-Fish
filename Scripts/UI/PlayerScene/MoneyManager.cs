using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour, IDataPersistence
{
    public int playerMoney = 35; // This value should always match what the value is in the ctor in GameData.cs
    private TMP_Text moneyText;
    [SerializeField] private GameObject notEnoughMoneyObj;
    private float timer = 0f;
    private readonly float waitTimeUntilDeactivate = 1.75f;
    private OpenStoreTrigger storeScript;

    // Stats Page
    [SerializeField] private TMP_Text journalStatsPageCurrentMoney;

    public void LoadData(GameData data) => playerMoney = data.playerMoney;
    public void SaveData(GameData data) => data.playerMoney = playerMoney;

    private void Awake()
    {
        storeScript = FindObjectOfType<OpenStoreTrigger>();
        moneyText = GetComponent<TMP_Text>();
    }
    public void Start()
    {
        moneyText.text = $"Money: ${playerMoney}";
        journalStatsPageCurrentMoney.text = moneyText.text;
    }

    public void AddMoney(int moneyAmtToAdd)
    {
        playerMoney += moneyAmtToAdd;
        moneyText.text = $"Money: ${playerMoney}";
        journalStatsPageCurrentMoney.text = moneyText.text;
    }

    public void SubtractMoney(int moneyAmtToSubtract)
    {
        if (playerMoney >= moneyAmtToSubtract)
        {
            playerMoney -= moneyAmtToSubtract;
            moneyText.text = $"Money: ${playerMoney}";
            journalStatsPageCurrentMoney.text = moneyText.text;
        }
        else
        {
            notEnoughMoneyObj.SetActive(true);
        }
    }

    public void Update()
    {
        ToggleOnScreenMoneyText();
        ToggleNotEnoughMoneyImage();
    }

    private void ToggleOnScreenMoneyText() => moneyText.text = storeScript.InStoreTrigger ? $"Money: ${playerMoney}" : string.Empty;
    private void ToggleNotEnoughMoneyImage()
    {
        if (!storeScript.InStoreTrigger) return;

        if (notEnoughMoneyObj.activeInHierarchy)
        {
            timer += Time.deltaTime;
            if (timer >= waitTimeUntilDeactivate)
            {
                notEnoughMoneyObj.SetActive(false);
                timer = 0f;
            }
        }
    }
}
