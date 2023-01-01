using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeButtonIn : MonoBehaviour
{
    private YieldInstruction fadeInstruction = new YieldInstruction();
    private float fadeTime = 5f;
    [SerializeField] private TMP_Text buttonText;

    void Start()
    {
        Image imageOnButton = GetComponent<Image>();
        StartCoroutine(FadeIn(imageOnButton, buttonText));
    }

    private IEnumerator FadeIn(Image image, TMP_Text text)
    {
        float elapsedTime = 0.0f;
        Color c = image.color;
        Color textColor = text.color;
        while (elapsedTime < fadeTime)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / fadeTime);
            textColor.a = Mathf.Clamp01(elapsedTime / fadeTime);
            image.color = c;
            text.color = textColor;
        }
    }
}
