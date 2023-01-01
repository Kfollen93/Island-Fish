using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    [Title("Animancer Variables")]
    [SerializeField] private AnimancerComponent animancer;
    [SerializeField] private AnimationClip walk;
    [SerializeField] private AnimationClip idle;
    [SerializeField] private AnimationClip run;
    [SerializeField] private AnimationClip pickUpItem;
    [SerializeField] private AnimationClip fishingIdle;
    [SerializeField] private AnimationClip fishingCast;

    public void StartingIdleAnim() => animancer.Play(idle);

    public void PlayRunAnim()
    {
        var runningState = animancer.Play(run);
        runningState.Speed = 1f;
        runningState.Events.OnEnd = StartingIdleAnim;
    }

    public void PlayWalkAnim()
    {
        var walkingState = animancer.Play(walk);
        walkingState.Speed = 1f;
        walkingState.Events.OnEnd = StartingIdleAnim;
    }

    public void PlayPickUpAnim()
    {
        var pickUp = animancer.Play(pickUpItem);
        pickUp.Speed = 1.75f;
        pickUp.Events.OnEnd = StartingIdleAnim;
    }

    public void PlayFishingIdleAnim()
    {
        var fishingIdleState = animancer.Play(fishingIdle);
        fishingIdleState.Speed = 1f;
        fishingIdleState.Events.OnEnd = StartingIdleAnim;
    }

    public void PlayFishingCastAnim()
    {
        var fishingCastState = animancer.Play(fishingCast);
        fishingCastState.Speed = 0.6f;
        fishingCastState.Events.OnEnd = PlayFishingIdleAnim;
    }

    public void PauseFishingCastAnim()
    {
        var state = animancer.States[fishingCast];
        state.IsPlaying = false;
    }
}
