using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private GameObject dialogueBackgroundWithText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speechBubble.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            speechBubble.SetActive(false);
        }
    }

    private void Update()
    {
        if (dialogueBackgroundWithText.activeInHierarchy)
        {
            speechBubble.SetActive(false);
        }
        else if (speechBubble.activeInHierarchy)
        {
            speechBubble.transform.rotation = Camera.main.transform.rotation;
        }
    }
}
