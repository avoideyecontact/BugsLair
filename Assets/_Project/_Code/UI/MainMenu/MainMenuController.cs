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
    [SerializeField] private SettingsUI _settingsUI;
    [SerializeField] private Button _backFromSettings;

    private ISceneLoader _sceneLoader;

    private void Start()
    {
        _mainCanvas.enabled = true;
        _settingsUI.Hide();

        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        _quitButton.onClick.AddListener(OnQuitButtonClicked);

        _backFromSettings.onClick.AddListener(OnBackFromSettingsButtonClicked);
    }

    [Inject]
    public void Construct(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    private void OnPlayButtonClicked()
    {
        //_sceneLoader.LoadSceneAsync("2_Cutscene", useFade: true);
        _sceneLoader.LoadSceneAsync("3_Gameplay", useFade: true);
    }

    private void OnSettingsButtonClicked()
    {
        _mainCanvas.enabled = false;
        _settingsUI.Show();
    }

    private void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnBackFromSettingsButtonClicked()
    {
        _mainCanvas.enabled = true;
    }
}
