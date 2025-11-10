using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class PauseUI : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] private Canvas _pauseCanvas;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Transform _pauseMenu;

    [Header("Settings")]
    [SerializeField] private SettingsUI _settingsUI;
    [SerializeField] private Button _backFromSettingsButton;

    private IEventBus _eventBus;
    private IPauseService _pause;
    private ISceneLoader _sceneLoader;

    [Inject]
    public void Construct(IEventBus eventBus, IPauseService pauseService, ISceneLoader sceneLoader)
    {
        _eventBus = eventBus;
        _pause = pauseService;
        _sceneLoader = sceneLoader;
        _eventBus.Subscribe<GamePaused>(OnGamePaused);
        _eventBus.Subscribe<GameResumed>(OnGameResumed);
    }

    private void Start()
    {
        if (_pause.isPaused) Show();
        else Hide();

        _settingsUI.Hide();
        _continueButton.onClick.AddListener(OnContinueButtonPressed);
        _settingsButton.onClick.AddListener(OnSettingsButtonPressed);
        _quitButton.onClick.AddListener(OnQuitButtonPressed);

        _backFromSettingsButton.onClick.AddListener(OnBackFromSettingsButtonClicked);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<GamePaused>(OnGamePaused);
        _eventBus.Unsubscribe<GameResumed>(OnGameResumed);
    }

    private void OnContinueButtonPressed() => _pause.ResumeGame();

    private void OnSettingsButtonPressed()
    {
        _settingsUI.Show();
        _pauseMenu.gameObject.SetActive(false);
    }

    private void OnQuitButtonPressed() => _sceneLoader.LoadSceneAsync("1_Menu", true);

    private void OnGamePaused(GamePaused evt)
    {
        _pauseMenu.gameObject.SetActive(true);
        Show();
    }

    private void OnGameResumed(GameResumed evt)
    {
        _pauseMenu.gameObject.SetActive(true);
        Hide();
    }

    private void Show()
    {
        _pauseCanvas.enabled = true;
        ShowCursor();
    }

    private void Hide()
    {
        _pauseCanvas.enabled = false;
        HideCursor();
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnBackFromSettingsButtonClicked()
    {
        _pauseMenu.gameObject.SetActive(true);
    }
}
