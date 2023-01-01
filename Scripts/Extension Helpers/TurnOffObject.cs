using UnityEngine;

public class TurnOffObject : MonoBehaviour
{
    // Keep this method as Start(), as this is script will work with objects that are originally on,
    // then in Awake() for those objects, references will be set, and then during Start(), this script will turn them off.
    // This prevents needing to find Inactive GOs.
    private void Start()
    {
        this.gameObject.SetActive(false);
    }
}
