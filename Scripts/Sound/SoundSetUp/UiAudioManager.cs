using UnityEngine;
using UnityEngine.UI;

public class UiAudioManager : MonoBehaviour
{
    [SerializeField] private SoundDataSO _so;
    [SerializeField] private Toggle _musicEnabled;
    [SerializeField] private Toggle _soundEnabled;
    [SerializeField] private Slider _musicVolume;
    [SerializeField] private Slider _soundVolume;

    private void OnEnable()
    {
        _so.onMutated += OnMutated;

        _musicEnabled.onValueChanged.AddListener(v =>
        {
            _so.playMusic = v;
            _so.Changed();
        });

        _soundEnabled.onValueChanged.AddListener(v =>
        {
            _so.playSound = v;
            _so.Changed();
        });

        _musicVolume.onValueChanged.AddListener(v =>
        {
            _so.musicVolume = v;
            _so.Changed();
        });

        _soundVolume.onValueChanged.AddListener(v =>
        {
            _so.soundVolume = v;
            _so.Changed();
        });

        OnMutated();
    }

    private void OnDisable()
    {
        _so.onMutated -= OnMutated;

        _musicEnabled.onValueChanged.RemoveAllListeners();
        _soundEnabled.onValueChanged.RemoveAllListeners();
        _musicVolume.onValueChanged.RemoveAllListeners();
        _soundVolume.onValueChanged.RemoveAllListeners();
    }

    private void OnMutated()
    {
        _musicEnabled.SetIsOnWithoutNotify(_so.playMusic);
        _soundEnabled.SetIsOnWithoutNotify(_so.playSound);
        _musicVolume.SetValueWithoutNotify(_so.musicVolume);
        _soundVolume.SetValueWithoutNotify(_so.soundVolume);
    }
}