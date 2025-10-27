using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MainMenuController : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    [Header("Settings")]

    private GameSettings _tempSettings;

    [SerializeField] private Canvas _settingsCanvas;
    [SerializeField] private Button _backToMenuButton;

    [Header("Audio Settings")]
    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;

    private ISceneLoader _sceneLoader;
    private ISettingsManager _settingsManager;

    private void Start()
    {
        _mainCanvas.enabled = true;
        _settingsCanvas.enabled = false;

        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        _quitButton.onClick.AddListener(OnQuitButtonClicked);
        _backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);

        _masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        _sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }

    [Inject]
    public void Construct(ISceneLoader sceneLoader, ISettingsManager settingsManager)
    {
        _sceneLoader = sceneLoader;
        _settingsManager = settingsManager;
    }

    private void OnPlayButtonClicked()
    {
        //_sceneLoader.LoadSceneAsync("2_Cutscene", useFade: true);
        _sceneLoader.LoadSceneAsync("3_Gameplay", useFade: true);
    }

    private void OnSettingsButtonClicked()
    {
        LoadSettingsIntoUI();
        _mainCanvas.enabled = false;
        _settingsCanvas.enabled = true;
    }

    private void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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

        _mainCanvas.enabled = true;
        _settingsCanvas.enabled = false;
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
