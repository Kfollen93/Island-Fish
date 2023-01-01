using System;
using UnityEngine;

[CreateAssetMenu]
public class SoundDataSO : ScriptableObject
{
    public bool playMusic = true;
    public bool playSound = true;
    public float musicVolume = 0.5f;
    public float soundVolume = 0.5f;

    public event Action onMutated;

    public void Changed() => onMutated?.Invoke();
}
