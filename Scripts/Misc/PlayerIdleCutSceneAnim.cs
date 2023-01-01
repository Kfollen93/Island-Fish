using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerIdleCutSceneAnim : MonoBehaviour
{
    [Title("Animancer Variables")]
    [SerializeField] private AnimancerComponent animancer;
    [SerializeField] private AnimationClip idle;

    private void OnEnable()
    {
        PlayIdleAnim();
    }

    public void PlayIdleAnim() => animancer.Play(idle, 0.25f);
}
