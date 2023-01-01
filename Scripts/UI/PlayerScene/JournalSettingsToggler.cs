using UnityEngine;

/* The Fullscreen Background and page two (Fish) are enabled before starting the game so that in 
   FishSlot.cs I can set the subscriber to run in Awake() (and get the slotImage component),
   before the other subscriber is set in PreviouslyCaughtFishUI.cs*/

public class JournalSettingsToggler : MonoBehaviour
{
    [SerializeField] private GameObject pageTwoFish;
    private void Start()
    {
        gameObject.SetActive(false);
        pageTwoFish.SetActive(false);
    }
}
