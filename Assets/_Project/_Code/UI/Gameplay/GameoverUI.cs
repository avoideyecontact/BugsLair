using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class GameoverUI : MonoBehaviour
{
    [SerializeField] private Canvas _gameoverCanvas;
    [SerializeField] private Button _retryButton;
    [SerializeField] private Button _menuButton;

    private IEventBus _eventBus;
    private ISceneLoader _sceneLoader;
    private IPauseService _pauseService;

    [Inject]
    public void Construct(IEventBus eventBus, ISceneLoader sceneLoader, IPauseService pauseService)
    {
        _eventBus = eventBus;
        _sceneLoader = sceneLoader;
        _pauseService = pauseService;
        _eventBus.Subscribe<PlayerDeath>(OnPlayerDeath);

        _retryButton.onClick.AddListener(OnRetryButtonPressed);
        _menuButton.onClick.AddListener(OnMenuButtonPressed);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<PlayerDeath>(OnPlayerDeath);
    }

    private void Start()
    {
        _gameoverCanvas.enabled = false;
    }

    private void OnPlayerDeath(PlayerDeath evt)
    {
        _gameoverCanvas.enabled = true;
        _pauseService.PauseGame();
        ShowCursor();
    }

    private void OnRetryButtonPressed()
    {
        _sceneLoader.LoadSceneAsync("3_Gameplay", useFade: true);
    }

    private void OnMenuButtonPressed()
    {
        _sceneLoader.LoadSceneAsync("1_Menu", useFade: true);
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
