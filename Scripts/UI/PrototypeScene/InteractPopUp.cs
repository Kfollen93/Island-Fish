using UnityEngine;

// This script should be attached to the object that you want to interact with.
// The object that you want to interact with, should also have the
// Interact PopUp (E) Canvas prefab as a child of that object.

// With the above in place, the player can walk up to any object and will be prompted to press 'E'.
public class InteractPopUp : MonoBehaviour
{
    [SerializeField] private GameObject speechBubble;

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
}
