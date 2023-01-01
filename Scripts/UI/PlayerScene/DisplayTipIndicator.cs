using UnityEngine;

public class DisplayTipIndicator : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject redExclamationTip;
    [SerializeField] private GameObject journalExclamationTip;
    private BuyItem buyItem;
    private bool hasJournalBeenOpenedBefore;
    private Vector3 journalExclamationTipOffSet = new Vector3(15f, -25f, 0f);

    // Basic Fishing Pole Bought
    [SerializeField] private GameObject equipFishingPoleMessage;
    private bool hasBasicFishingPoleTipBeenDisplayed;

    // Picked Up Worm
    [SerializeField] private GameObject pickedUpWormMessage;
    private bool hasWormTipBeenDisplayed;

    public void LoadData(GameData data)
    {
        hasWormTipBeenDisplayed = data.hasWormTipBeenDisplayed;
        hasBasicFishingPoleTipBeenDisplayed = data.hasBasicFishingPoleTipBeenDisplayed;
        DisplayAnyLoadedMessages();
    }

    public void SaveData(GameData data)
    {
        data.hasWormTipBeenDisplayed = hasWormTipBeenDisplayed;
        data.hasBasicFishingPoleTipBeenDisplayed = hasBasicFishingPoleTipBeenDisplayed;
    }

    private void Start()
    {
        redExclamationTip.SetActive(false);
        journalExclamationTip.SetActive(false);
        buyItem = FindInactiveObjects.FindInActiveObjectByTag("Buy Item").GetComponent<BuyItem>();
    }

    private void Update()
    {
        DisplayEquipFishingPoleMessage();
        DisplayPickedUpWormMessage();

        // First time opening journal when there is a red ! showing.
        if (InputManager.Instance.journalOpened && !hasJournalBeenOpenedBefore)
        {
            hasJournalBeenOpenedBefore = true;
        }

        // Journal is closed, but was opened before when the red ! was showing.
        if (!InputManager.Instance.journalOpened && hasJournalBeenOpenedBefore)
        {
            redExclamationTip.SetActive(false);
            journalExclamationTip.SetActive(false);
            hasJournalBeenOpenedBefore = false;
        }
    }

    private void UpdateLatestJournalEntry(GameObject journalMessage)
    {
        redExclamationTip.SetActive(true);
        journalExclamationTip.SetActive(true);
        journalMessage.SetActive(true);
        journalExclamationTip.transform.position = journalMessage.transform.position;
        journalExclamationTip.transform.position -= journalExclamationTipOffSet;
    }

    /* Functions For Journal Entries Below*/

    private void DisplayEquipFishingPoleMessage()
    {
        if (buyItem.wasBasicFishingPoleBought && !hasBasicFishingPoleTipBeenDisplayed)
        {
            hasBasicFishingPoleTipBeenDisplayed = true;
            UpdateLatestJournalEntry(equipFishingPoleMessage);
        }
    }

    private void DisplayPickedUpWormMessage()
    {
        if (ItemPickup.firstTimePickingUpWorm && !hasWormTipBeenDisplayed)
        {
            hasWormTipBeenDisplayed = true;
            UpdateLatestJournalEntry(pickedUpWormMessage);
        }
    }

    private void DisplayAnyLoadedMessages()
    {
        if (hasBasicFishingPoleTipBeenDisplayed) equipFishingPoleMessage.SetActive(true);
        if (hasWormTipBeenDisplayed) pickedUpWormMessage.SetActive(true);
    }
}