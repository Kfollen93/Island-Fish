using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject background;
    private bool isMenuOpen;

    private void Start()
    {
        MouseStatus.HideCursor();
    }

    private void Update()
    {
        MenuControls();
    }

    private void MenuControls()
    {
        if (!isMenuOpen && !InputManager.Instance.gameplayControlsEnabled && !InputManager.Instance.fishingControlsEnabled)
        {
            OpenMenu();  
        }
        else if (isMenuOpen && InputManager.Instance.gameplayControlsEnabled)
        {
            CloseMenu();
        }
    }

    private void OpenMenu()
    {
        isMenuOpen = true;
        MouseStatus.DisplayCursor();
        background.SetActive(true);
    }

    // Only corresponds to "ESC" key being pressed, as that switches the Player/Menu controls from InputManager.cs
    public void CloseMenu()
    {
        isMenuOpen = false;
        MouseStatus.HideCursor();
        background.SetActive(false);
    }
}