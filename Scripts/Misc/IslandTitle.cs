using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class IslandTitle : MonoBehaviour
{
    private TMP_Text islandTitle;
    private Tween fadeTween;
    private readonly WaitForSeconds timeToWaitForFadeIn = new WaitForSeconds(1f);
    private readonly WaitForSeconds timeToWaitForFadeOut = new WaitForSeconds(3f);

    private void Awake() => islandTitle = GetComponent<TMP_Text>();

    private void Start() => StartCoroutine(RunFade());

    public void FadeIn(float duration) => Fade(1f, duration, () => { });
    public void FadeOut(float duration) => Fade(0f, duration, () => { });


    private void Fade(float endValue, float duration, TweenCallback onEnd)
    {
        fadeTween?.Kill(false);
        fadeTween = islandTitle.DOFade(endValue, duration);
        fadeTween.onComplete += onEnd;
    }

    private IEnumerator RunFade()
    {
        yield return timeToWaitForFadeIn;
        FadeIn(1f);
        yield return timeToWaitForFadeOut;
        FadeOut(1f);
    }
}
