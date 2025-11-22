using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class GlobalInputService : MonoBehaviour
{
    [SerializeField] private InputActionAsset _actionsAsset;

    private ISceneLoader _sceneLoader;

    private InputAction _pauseAction;
    public event System.Action PauseStarted;

    [Inject]
    public void Construct(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    private void Awake()
    {
        if (_actionsAsset == null)
            Debug.LogError("InputActionAsset is missing", this);

        _pauseAction = _actionsAsset.FindAction("Global/Pause", true);
    }

    private void OnEnable()
    {
        _pauseAction.Enable();
        _pauseAction.started += OnPause;
    }

    private void OnDisable()
    {
        _pauseAction.started -= OnPause;
        _pauseAction.Disable();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        PauseStarted?.Invoke();
    }

    private void OnDestroy()
    {
        _pauseAction.started -= OnPause;
        _pauseAction?.Dispose();
    }
}
