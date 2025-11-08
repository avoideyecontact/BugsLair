using UnityEngine;
using UnityEngine.Audio;
using VContainer;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    private ISettingsManager _settingsManager;

    [Inject]
    public void Construct(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
    }

    private void Start()
    {
        SetMasterVolume(_settingsManager.Settings.masterVolume);
        SetSFXVolume(_settingsManager.Settings.sfxVolume);
        SetMusicVolume(_settingsManager.Settings.musicVolume);
    }

    public void SetMasterVolume(float level)
    {
        _audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }
    public void SetSFXVolume(float level)
    {
        _audioMixer.SetFloat("sfxVolume", Mathf.Log10(level) * 20f);
    }
    public void SetMusicVolume(float level)
    {
        _audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }
}
