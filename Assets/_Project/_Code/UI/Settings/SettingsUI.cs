using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SettingsUI : MonoBehaviour
{
    //[SerializeField] private Transform _pauseMenu;
    [SerializeField] private Canvas _settingsCanvas;
    [SerializeField] private Button _backToMenuButton;

    [Header("Audio Settings")]
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;

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
    }

    public void Show()
    {
        LoadSettingsIntoUI();
        _settingsCanvas.enabled = true;
        _eventBus.Publish(new SettingsUIOpened());
    }

    public void Hide()
    {
        _settingsCanvas.enabled = false;

        // Hides pause menu (everywhere except main menu)
        //if (_pauseMenu != null)
        //    _pauseMenu.gameObject.SetActive(true);

        _eventBus.Publish(new SettingsUIClosed());
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

    private void OnGameResumed(GameResumed evt)
    {
        Hide();
    }
}
