using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

public class GlobalInputService : MonoBehaviour, IStartable
{
    [SerializeField] private InputActionAsset _actionsAsset;

    private InputAction _pauseAction;
    private IPauseService _pauseService;
    private ISceneLoader _sceneLoader;

    [Inject]
    public void Construct(IPauseService pauseService, ISceneLoader sceneLoader)
    {
        _pauseService = pauseService;
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

        if (_pauseAction != null)
        {
            _pauseAction.Enable();
            _pauseAction.started += OnPause;
        }
        else
        {
            Debug.LogError("Pause action is missing", this);
        }
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (_sceneLoader.CurrentScene != "1_Menu")
            _pauseService?.TogglePause();
    }

    private void OnDestroy()
    {
        _pauseAction.started -= OnPause;
        _pauseAction?.Dispose();
    }
}
