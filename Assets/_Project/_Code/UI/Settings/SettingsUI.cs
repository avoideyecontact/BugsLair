using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Canvas _settingsCanvas;
    [SerializeField] private Button _backToMenuButton;

    [Header("Audio Settings")]
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;

    private ISettingsManager _settingsManager;
    private GameSettings _tempSettings;

    [Inject]
    public void Construct(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
        LoadSettingsIntoUI();
    }

    private void Start()
    {
        _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
        _masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        _sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }

    public void Show()
    {
        LoadSettingsIntoUI();
        _settingsCanvas.enabled = true;
    }

    public void Hide()
    {
        _settingsCanvas.enabled = false;
    }

    private void LoadSettingsIntoUI()
    {
        _tempSettings = _settingsManager.Settings;

        _masterVolumeSlider.value = _tempSettings.masterVolume;
        _sfxVolumeSlider.value = _tempSettings.sfxVolume;
        _musicVolumeSlider.value = _tempSettings.musicVolume;
    }

    private void OnBackToMenuButtonClicked()
    {
        _settingsManager.ApplySettings(_tempSettings);
        Hide();
    }

    private void OnMasterVolumeChanged(float value)
    {
        _tempSettings.masterVolume = value;
    }
    private void OnSFXVolumeChanged(float value)
    {
        _tempSettings.sfxVolume = value;
    }
    private void OnMusicVolumeChanged(float value)
    {
        _tempSettings.musicVolume = value;
    }
}
