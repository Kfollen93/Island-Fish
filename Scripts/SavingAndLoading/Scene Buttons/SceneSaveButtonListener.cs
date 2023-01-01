using UnityEngine;
using UnityEngine.UI;

/* Created this script for the PROTOTYPE SCENE save button in order to use DDOL on the DataManager that is from the Main Menu scene.
 * This way, when you start the game from the main menu, you can choose New Game, or Load Game, and that will bring the
 * DDOL DataManager object over to the prototype scene, and then this listener will find the load button in protoype scene,
 * and then when clicking the "Save" button it will call the SaveGame() method and not have a null reference in the slot.
 */
public class SceneSaveButtonListener : MonoBehaviour
{
    private Button saveButton;
    void Start()
    {
        saveButton = GameObject.FindWithTag("Scene Save Button").GetComponent<Button>();
        saveButton.onClick.AddListener(FindObjectOfType<DataPersistenceManager>().SaveGame);
    }
}
