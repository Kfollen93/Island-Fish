using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalTabGroup : MonoBehaviour
{
    [HideInInspector] public List<JournalTabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    private JournalTabButton selectedTab;
    public List<GameObject> objectsToSwap;
    private Color32 tabIconColor = new Color32(106, 133, 171, 255); // Blue

    public void Subscribe(JournalTabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<JournalTabButton>();
        }
        tabButtons.Add(button);
    }

    public void OnTabEnter(JournalTabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(JournalTabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(JournalTabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.sprite = tabActive;

        SetIconTabColor(tabIconColor, button);

        // Change the Journal displayed page.
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach(JournalTabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) continue;

            button.background.sprite = tabIdle;
            SetIconTabColor(Color.white, button);
        }
    }

    private void SetIconTabColor(Color32 color, JournalTabButton btn)
    {
        var childFromTab = btn.transform.GetChild(0);
        var imageForUiIconButton = childFromTab.GetComponent<Image>();
        imageForUiIconButton.color = color;
    }
}
