using Animancer;
using UnityEngine;

public class BirdFlyAnims : MonoBehaviour
{
    [SerializeField] private AnimancerComponent _Animancer;
    [SerializeField] private AnimationClip[] animations;

    public void PlayBirdFlyingAnims()
    {
        int anim = Random.Range(0, animations.Length);
        _Animancer.Play(animations[anim]);
    }
}
