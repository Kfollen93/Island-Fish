using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterDialogue : MonoBehaviour
{
    [SerializeField] private GameObject textGO;
    [SerializeField] private TMP_Text textComponent;
    private readonly WaitForSeconds typingSpeed = new WaitForSeconds(0.08f);
    private readonly WaitForSeconds textPause = new WaitForSeconds(1f);

    /* Each sentence must end in '. . .' to properly clear and display the next, besides the last*/
    private readonly string introText = "It's been a while since I've taken the time to leave and do something for myself . . .The same routine everyday can become quite tiresome, it's possible something new will be good for me. . .I've had this old sailboat for so long, but I never had the courage to take it out. . .I plan to meet many new friends along the way, and track my journey to share with friends and family back home. . .The coordinates show I should be arriving soon . . .Ah, this must be it, Hargough Island...";

    private void Start()
    {
        textComponent = textGO.GetComponent<TMP_Text>();
        StartCoroutine(TypeWriterEffect());
    }

    private IEnumerator TypeWriterEffect()
    {
        for (int i = 0; i < introText.Length; i++)
        {
            // End of sentence, clear text, start new sentence.
            if (introText[i] == '.' && introText[i - 4] == '.')
            {
                textComponent.text += introText[i];
                yield return textPause;
                textComponent.text = " ";
            }
            else
            {
                textComponent.text += introText[i];
                yield return typingSpeed;
            }
        }
    }
}
