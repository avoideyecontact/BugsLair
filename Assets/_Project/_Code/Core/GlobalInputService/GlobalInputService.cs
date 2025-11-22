using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class GlobalInputService : MonoBehaviour, IStartable
{
    [SerializeField] private InputActionAsset _actionsAsset;

    private InputAction _pauseAction;
    private GameStateManager _gameStateManager;
    private ISceneLoader _sceneLoader;

    [Inject]
    public void Construct(GameStateManager gameStateManager, ISceneLoader sceneLoader)
    {
        _gameStateManager = gameStateManager;
        _sceneLoader = sceneLoader;
    }

    void IStartable.Start()
    {
        InitializeInput();
    }

    private void InitializeInput()
    {
        if (_actionsAsset == null)
        {
            Debug.LogError("InputActionAsset is missing", this);
            return;
        }

        _pauseAction = _actionsAsset.FindAction("Global/Pause", true);

        _pauseAction.Enable();
        _pauseAction.started += OnPause;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (_sceneLoader.CurrentScene != "1_Menu")
        {
            if (_gameStateManager.GameIsPaused)
                _gameStateManager.ResumeGame();
            else
                _gameStateManager.PauseGame();
        }
    }

    private void OnDestroy()
    {
        _pauseAction.started -= OnPause;
        _pauseAction?.Dispose();
    }
}
