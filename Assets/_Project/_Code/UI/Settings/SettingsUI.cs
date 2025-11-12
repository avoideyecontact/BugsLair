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

    [Header("Controls Settings")]    
    [SerializeField] private Slider _sensitivitySlider;

    [Header("Graphics Settings")]
    [SerializeField] private Toggle _vsyncToggle;

    private IEventBus _eventBus;
    private ISettingsManager _settingsManager;
    private SoundMixerManager _soundMixerManager;
    private GameSettings _tempSettings;

    [Inject]
    public void Construct(IEventBus eventBus, ISettingsManager settingsManager, SoundMixerManager soundMixerManager)
    {
        _eventBus = eventBus;
        _settingsManager = settingsManager;
        _soundMixerManager = soundMixerManager;
        SubscribeToEventBus();
        LoadSettingsIntoUI();

        QualitySettings.vSyncCount = _settingsManager.Settings.vsync ? 1 : 0;
    }

    private void SubscribeToEventBus()
    {
        _eventBus.Subscribe<GameResumed>(OnGameResumed);
    }

    private void UnsubscribeFromEventBus()
    {
        _eventBus.Unsubscribe<GameResumed>(OnGameResumed);
    }

    private void OnDestroy()
    {
        UnsubscribeFromEventBus();
    }

    private void Start()
    {
        _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
        _masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        _sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        _sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        _vsyncToggle.onValueChanged.AddListener(OnVSyncChanged);
    }

    public void Show()
    {
        LoadSettingsIntoUI();
        _settingsCanvas.enabled = true;
        _eventBus.Publish(new SettingsUIOpened());

        // fix for a bug when canvas sets to zero size
        if (_settingsCanvas.transform.localScale == Vector3.zero)
        {
            _settingsCanvas.transform.localScale = Vector3.one;
        }
    }

    public void Hide()
    {
        _settingsCanvas.enabled = false;

        _eventBus.Publish(new SettingsUIClosed());
    }

    private void LoadSettingsIntoUI()
    {
        _tempSettings = _settingsManager.Settings;

        if (_tempSettings == null)
            Debug.LogError("Настройки не были загружены");

        _masterVolumeSlider.value = _tempSettings.masterVolume;
        _sfxVolumeSlider.value = _tempSettings.sfxVolume;
        _musicVolumeSlider.value = _tempSettings.musicVolume;
        _sensitivitySlider.value = _tempSettings.sensitivity;
        _vsyncToggle.isOn = _tempSettings.vsync;
    }

    private void OnBackToMenuButtonClicked()
    {
        _settingsManager.ApplySettings(_tempSettings);
        Hide();
    }

    private void OnMasterVolumeChanged(float value)
    {
        _tempSettings.masterVolume = value;
        _soundMixerManager.SetMasterVolume(value);
    }
    private void OnSFXVolumeChanged(float value)
    {
        _tempSettings.sfxVolume = value;
        _soundMixerManager.SetSFXVolume(value);
    }
    private void OnMusicVolumeChanged(float value)
    {
        _tempSettings.musicVolume = value;
        _soundMixerManager.SetMusicVolume(value);
    }

    private void OnSensitivityChanged(float value)
    {
        _tempSettings.sensitivity = value;
        _eventBus.Publish(new CameraSensitivityChanged
        {
            Sensitivity = _tempSettings.sensitivity,
        });
    }

    private void OnVSyncChanged(bool value)
    {
        _tempSettings.vsync = value;

        QualitySettings.vSyncCount = value ? 1 : 0;
    }

    private void OnGameResumed(GameResumed evt)
    {
        Hide();
    }
}
