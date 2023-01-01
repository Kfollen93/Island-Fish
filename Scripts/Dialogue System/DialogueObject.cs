using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "DialogueObject", menuName = "NPC Dialogue Object", order = 0)]
public class DialogueObject : ScriptableObject
{
    [Header("Dialogue")]
    public List<DialogueSegment> dialogueSegments = new List<DialogueSegment>();

    [System.Serializable]
    public struct DialogueSegment
    {
        public string dialogueText;
        public float dialogueDisplayTime;
    }
}
