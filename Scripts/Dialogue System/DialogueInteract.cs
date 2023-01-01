using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueInteract : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    private bool inDialogueTriggerArea;
    [SerializeField] private GameObject dialogueBackgroundWithText;
    private bool isDialogueActive;
    [SerializeField] private TMP_Text textComponent;
    private readonly WaitForSeconds typeWriterSpeed = new WaitForSeconds(0.05f);
    private Coroutine dialogueCoroutine;
    [SerializeField] private Transform player;

    private void Awake()
    {
        if (player == null) GameObject.FindWithTag("Player");
    }

    private IEnumerator DisplayDialogue()
    {
        foreach (var dialogue in dialogueObject.dialogueSegments)
        {
            textComponent.text = "";
            foreach (var ch in dialogue.dialogueText)
            {
                textComponent.text += ch;
                yield return typeWriterSpeed;
            }
            yield return new WaitForSeconds(dialogue.dialogueDisplayTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inDialogueTriggerArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inDialogueTriggerArea = false;
            isDialogueActive = false;
            dialogueBackgroundWithText.SetActive(false);
            if (dialogueCoroutine != null) StopCoroutine(dialogueCoroutine);
            transform.rotation = Quaternion.identity;
        }
    }
    private void Update()
    {
        if (inDialogueTriggerArea && !isDialogueActive && InputManager.Instance.InteractCurrentlyPressed)
        {
            Vector3 playerYPos = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(playerYPos);
            isDialogueActive = true;
            dialogueBackgroundWithText.SetActive(true);
            dialogueCoroutine = StartCoroutine(DisplayDialogue());
        }
    }
}
