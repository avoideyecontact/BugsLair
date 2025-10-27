using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Canvas _pauseCanvas;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

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

        _continueButton.onClick.AddListener(OnContinueButtonPressed);
        _quitButton.onClick.AddListener(OnQuitButtonPressed);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<GamePaused>(OnGamePaused);
        _eventBus.Unsubscribe<GameResumed>(OnGameResumed);
    }

    private void OnContinueButtonPressed() => _pause.ResumeGame();

    private void OnQuitButtonPressed() => _sceneLoader.LoadSceneAsync("1_Menu", true);

    private void OnGamePaused(GamePaused evt) => Show();

    private void OnGameResumed(GameResumed evt) => Hide();

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
}
