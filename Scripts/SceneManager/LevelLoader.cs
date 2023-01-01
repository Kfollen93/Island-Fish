using System.Collections;
using Animancer;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private AnimancerComponent _Animancer;
    [SerializeField] private AnimationClip fadeStart;
    [SerializeField] private AnimationClip fadeEnd;

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingInterface;
    [SerializeField] private Slider loadingProgressBar;
    [SerializeField] private TMP_Text progressText;

    public void LoadNextLevel()
    {
        loadingInterface.SetActive(true);
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
}
