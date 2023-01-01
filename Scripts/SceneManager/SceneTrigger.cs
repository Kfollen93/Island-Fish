using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoaderScript;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boat"))
        {
            levelLoaderScript.LoadNextLevel();
        }
    }
}
