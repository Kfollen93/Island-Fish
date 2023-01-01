using UnityEngine;
using UnityEngine.UI;

/* Created this script for the PROTOTYPE SCENE save button in order to use DDOL on the DataManager that is from the Main Menu scene.
 * This way, when you start the game from the main menu, you can choose New Game, or Load Game, and that will bring the
 * DDOL DataManager object over to the prototype scene, and then this listener will find the load button in protoype scene,
 * and then when clicking the "Load" button it will call the LoadGame() method and not have a null reference in the slot.
 */
public class SceneLoadButtonListener : MonoBehaviour
{
    private Button loadButton;
    [SerializeField] private GameObject levelTransitionEffect;
    void Start()
    {
        loadButton = GameObject.FindWithTag("Scene Load Button").GetComponent<Button>();
        loadButton.onClick.AddListener(FindObjectOfType<DataPersistenceManager>().LoadGame);
    }

    public void PlayLevelFadeAnim()
    {
        levelTransitionEffect.SetActive(false);
        levelTransitionEffect.SetActive(true);
    }
}
