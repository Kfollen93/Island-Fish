using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class JournalTabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public JournalTabGroup tabGroupScript;
    [HideInInspector] public Image background;
    public bool setStartingPage; // This should be set via the inspector for which page/tab you want to be shown upon first opening journal.

    private void Start()
    {
        background = GetComponent<Image>();
        tabGroupScript.Subscribe(this);

        if (setStartingPage)
        {
            tabGroupScript.OnTabSelected(this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroupScript.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroupScript.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroupScript.OnTabExit(this);
    }
}
