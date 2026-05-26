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
    private GameStateManager _gameStateManager;

    [Inject]
    public void Construct(IEventBus eventBus, ISceneLoader sceneLoader, GameStateManager gameStateManager)
    {
        _eventBus = eventBus;
        _sceneLoader = sceneLoader;
        _gameStateManager = gameStateManager;
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
        Debug.Log(_gameStateManager != null);
    }

    private void OnPlayerDeath(PlayerDeath evt)
    {
        _gameoverCanvas.enabled = true;
        _gameStateManager.PauseGame();
        _gameStateManager.ShowCursor();
    }

    private void OnRetryButtonPressed()
    {
        _sceneLoader.LoadSceneAsync("3_Gameplay", useFade: true);
    }

    private void OnMenuButtonPressed()
    {
        _sceneLoader.LoadSceneAsync("1_Menu", useFade: true);
    }
}
