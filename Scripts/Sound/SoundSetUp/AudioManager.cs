using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundDataSO _data;

    [SerializeField] private AudioSource _audioSourceSoundFx;
    [SerializeField] private AudioSource _audioSourceMusic;

    // In-Game 3d Sounds
    [SerializeField] private List<AudioSource> _3dAudioSources = new List<AudioSource>();


    private void Awake()
    {
        OnMutated();
    }

    private void OnEnable() => _data.onMutated += OnMutated;

    private void OnDisable() => _data.onMutated -= OnMutated;

    private void OnMutated()
    {
        _audioSourceMusic.volume = _data.musicVolume;
        _audioSourceSoundFx.volume = _data.soundVolume;
        _audioSourceMusic.enabled = _data.playMusic;
        _audioSourceSoundFx.enabled = _data.playSound;

        // In-Game 3d Sounds
        foreach (var _3dAudio in _3dAudioSources)
        {
            if (_3dAudio != null)
            {
                _3dAudio.volume = _data.soundVolume;
                _3dAudio.enabled = _data.playSound;
            }
        }
    }
}