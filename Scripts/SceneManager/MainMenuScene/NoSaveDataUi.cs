using UnityEngine;
using TMPro;
using System.Collections;

/* This will pop up from the MainMenu.cs Load() function, to display if there is NO prior save game data.
    Meaning player must click start a new game instead. Just a UI indicator :) */
public class NoSaveDataUi : MonoBehaviour
{
    private TMP_Text xSymbol;
    private WaitForSeconds timeToWait = new WaitForSeconds(1f);
    private void Start()
    {
        xSymbol = GetComponent<TMP_Text>();
    }

    public IEnumerator DisplaySymbol()
    {
        xSymbol.text = "X";
        yield return timeToWait;
        xSymbol.text = string.Empty;
    }
}
