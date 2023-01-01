using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class ShopSpeech : MonoBehaviour
{
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private TMP_Text setSpeechText;
    private int prevIndex = 0;

    List<string> shopWelcomeMessageList = new List<string>()
    {
        "What do ya need?", "Can I help you?", "Take a look.",
        "Nothing is free.", "Go ahead, buy something.", "Check what's in stock.",
        "I've got some good stuff.", "Check it out.", "You got money?",
        "Check out my wares.", "Need equipment?"
    };

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int randomIndex = Random.Range(0, shopWelcomeMessageList.Count);
            if (randomIndex == prevIndex)
            {
                // Prevent same message from appearing twice in a row.
                while (randomIndex == prevIndex)
                {
                    randomIndex = Random.Range(0, shopWelcomeMessageList.Count);
                }
            }
            prevIndex = randomIndex;
            setSpeechText.text = shopWelcomeMessageList[randomIndex];
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
        if (speechBubble.activeInHierarchy)
            speechBubble.transform.rotation = Camera.main.transform.rotation;
    }
}
