using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using Sirenix.OdinInspector;
using Cinemachine;
using TMPro;
using System.IO;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject loadingInterface;
    [SerializeField] private Slider loadingProgressBar;
    [SerializeField] private TMP_Text progressText;

    [Title("Switch between Cameras")]
    [SerializeField] private CinemachineVirtualCamera cmVirtualTentCam;
    [SerializeField] private GameObject tentSettingsHolder;
    private readonly WaitForSeconds timeToWait = new WaitForSeconds(1.75f);

    [Title("UI Pop Up on Load Button")]
    [SerializeField] private NoSaveDataUi noSaveDataUiScript;

    public void NewGame()
    {
        DataPersistenceManager.Instance.NewGame();
        DataPersistenceManager.Instance.SaveGame();

        HideMenu();
        ShowLoadingScreen();
        StartCoroutine(PlayLoadingScreen(SceneManager.LoadSceneAsync("IntroCutScene")));
    }

    public void LoadGame()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, DataPersistenceManager.Instance.fileName);
        if (!File.Exists(fullPath))
        {
            StartCoroutine(noSaveDataUiScript.DisplaySymbol());
            return;
        }
        HideMenu();
        ShowLoadingScreen();
        // Load Prototype Scene - which will in turn load the game data because of OnSceneLoaded() in DataPersistenceManager.cs.
        StartCoroutine(PlayLoadingScreen(SceneManager.LoadSceneAsync("Prototype")));
    }

    private IEnumerator PlayLoadingScreen(AsyncOperation sceneToLoad)
    {
        while (!sceneToLoad.isDone)
        {
            float totalProgress = Mathf.Clamp01(sceneToLoad.progress / .9f);
            loadingProgressBar.value = totalProgress;
            progressText.text = (totalProgress * 100f).ToString("F0") + "%";
            yield return null;
        }
    }

    public void HideMenu()
    {
        menu.SetActive(false);
    }
    
    public void ShowLoadingScreen()
    {
        loadingInterface.SetActive(true);
    }

    public void MoveCameraToSettingsTent()
    {
        cmVirtualTentCam.Priority = 3;
        StartCoroutine(WaitToDisplayTentSettingsMenu());
    }

    public void MoveCameraToSignPost()
    {
        cmVirtualTentCam.Priority = 1;
        tentSettingsHolder.SetActive(false);
    }

    private IEnumerator WaitToDisplayTentSettingsMenu()
    {
        yield return timeToWait;
        tentSettingsHolder.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
