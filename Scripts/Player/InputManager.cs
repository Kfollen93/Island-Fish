using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using System.Collections;

/* All input should be put in this script that is attached to the player, and added as a Unity Event.
    Having this script directly attached to the player will allow all the events to keep their references
    upon destroying and instantiating while loading.
*/
public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance => instance;

    [Title("Input --> Using Unity Events Connected to the Player")]
    private PlayerInput playerInput;

    [Title("Menu Manager Variables")]
    public bool gameplayControlsEnabled;
    private bool menuControlsEnabled;
    public bool fishingControlsEnabled;
    public bool exitShopPressed;
    private InputAction interactAction;
    public bool InteractCurrentlyPressed => interactAction.ReadValue<float>() != 0;
    // This really is saying that "Interact Is Not Pressed", but I am using it in conjunction with the InteractCurrentlyPressed bool.
    public bool InteractHasBeenLetGo => interactAction.ReadValue<float>() == 0;

    [Title("Shop Variables Referenced in OpenStoreTrigger.cs")]
    public bool scrollLeft;
    public bool scrollRight;

    [Title("Journal")]
    public bool journalOpened;
    private GameObject journal;

    //[Title("Inventory")] - Removed due to time.
    //public bool inventoryOpened;
    //private GameObject inventory;

    [Title("Buy Item")]
    [SerializeField] private BuyItem buyItem;

    private void Awake() 
    {
        instance = this;
        playerInput = GetComponent<PlayerInput>();
    }

    private IEnumerator FindPlayerUIReferences()
    {
        // Wait a frame after loading PlayerUI scene additively, otherwise will be NRE.
        yield return null;
        journal = FindInactiveObjects.FindInActiveObjectByTag("Journal");
        //inventory = FindInactiveObjects.FindInActiveObjectByTag("Inventory");
    }

    private void Start()
    {
        interactAction = playerInput.actions.FindAction("PlayerControls/Interact");
        gameplayControlsEnabled = true;
        StartCoroutine(FindPlayerUIReferences());
    }

    // This function executes while MenuControls Action Map is active.
    public void EnableGameplayControls(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerInput.SwitchCurrentActionMap("PlayerControls");
            gameplayControlsEnabled = true;
            fishingControlsEnabled = false;
            menuControlsEnabled = false;
        }
    }

    // This function executes while PlayerControls Action Map is active.
    public void EnablePauseMenuControls(InputAction.CallbackContext context)
    {
        if (context.performed && !journalOpened)
        {
            playerInput.SwitchCurrentActionMap("MenuControls");
            menuControlsEnabled = true;
            gameplayControlsEnabled = false;
        }
    }

    // This function executes while PlayerControls Action Map is active.
    public void EnableFishingControls(InputAction.CallbackContext context)
    {
        if (context.performed && buyItem.wasBasicFishingPoleBought && !journalOpened)
        {
            playerInput.SwitchCurrentActionMap("FishingControls");
            gameplayControlsEnabled = false;
            fishingControlsEnabled = true;
        }
    }

    public void OpenJournal(InputAction.CallbackContext context)
    {
        if (fishingControlsEnabled || menuControlsEnabled) return; // Can't open journal if in fishing mode/menu.

        if (context.performed && !journalOpened)
        {
            journalOpened = true;
            journal.SetActive(true);
            MouseStatus.DisplayCursor();
        }
        else if (context.performed && journalOpened)
        {
            journalOpened = false;
            journal.SetActive(false);
            MouseStatus.HideCursor();
        }
    }

    /* The Inventory has been scrapped due to time. Just uncomment for functionality.
    public void OpenInventory(InputAction.CallbackContext context) // Corresponds to I key.
    {
        if (context.performed && !inventoryOpened)
        {
            inventoryOpened = true;
            inventory.SetActive(true);
            MouseStatus.DisplayCursor();
        }
        else if (context.performed && inventoryOpened)
        {
            inventoryOpened = false;
            inventory.SetActive(false);
            MouseStatus.HideCursor();
        }
    }
    */

    #region SHOP CONTROLS
    public void ScrollLeftItems(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            scrollLeft = context.control.IsPressed();
        }
    }

    public void ScrollRightItems(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            scrollRight = context.control.IsPressed();
        }
    }

    public void ExitShopControls(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            exitShopPressed = context.control.IsPressed();
        }
    }
    #endregion
}
