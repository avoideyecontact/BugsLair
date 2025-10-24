using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] private InputActionAsset _actionsAsset;
    private IEventBus _eventBus;

    public Vector2 move;
    public Vector2 look;
    public float sprint;
    public float attack;

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _sprintAction;
    private InputAction _nextAction;
    private InputAction _previousAction;
    private InputAction _attackAction;
    private InputAction _cameraSwitchAction;

    public event System.Action NextStarted;
    public event System.Action PreviousStarted;
    public event System.Action CameraSwitchStarted;

    [Inject]
    public void Construct(IEventBus eventBus)
    {
        _eventBus = eventBus;
        _eventBus.Subscribe<GamePaused>(OnGamePaused);
        _eventBus.Subscribe<GameResumed>(OnGameResumed);
    }

    private void OnGamePaused(GamePaused evt)
    {
        enabled = false;
        ResetInputValues();
    }
    private void OnGameResumed(GameResumed evt)
    {
        enabled = true;
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<GamePaused>(OnGamePaused);
        _eventBus.Unsubscribe<GameResumed>(OnGameResumed);
    }

    private void ResetInputValues()
    {
        move = Vector2.zero;
        look = Vector2.zero;
        sprint = 0;
        attack = 0;
    }

    private void Awake()
    {
        if (_actionsAsset == null)
            Debug.LogError("InputActionAsset is missing", this);

        _moveAction = _actionsAsset.FindAction("Player/Move", true);
        _lookAction = _actionsAsset.FindAction("Player/Look", true);
        _sprintAction = _actionsAsset.FindAction("Player/Sprint", true);
        _nextAction = _actionsAsset.FindAction("Player/Next", true);
        _previousAction = _actionsAsset.FindAction("Player/Previous", true);
        _attackAction = _actionsAsset.FindAction("Player/Attack", true);
        _cameraSwitchAction = _actionsAsset.FindAction("Player/CameraSwitch", true);

        if (_moveAction == null)
            Debug.LogError("InputAction is missing", this);
        if (_lookAction == null)
            Debug.LogError("InputAction is missing", this);
        if (_lookAction == null)
            Debug.LogError("InputAction is missing", this);
        if (_nextAction == null)
            Debug.LogError("InputAction is missing", this);
        if (_previousAction == null)
            Debug.LogError("InputAction is missing", this);
        if (_attackAction == null)
            Debug.LogError("InputAction is missing", this);
        if (_cameraSwitchAction == null)
            Debug.LogError("InputAction is missing", this);
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;

        _lookAction.Enable();
        _lookAction.performed += OnLook;
        _lookAction.canceled += OnLook;

        _sprintAction.Enable();
        _sprintAction.performed += OnSprint;
        _sprintAction.canceled += OnSprint;

        _nextAction.Enable();
        _nextAction.started += OnNext;

        _previousAction.Enable();
        _previousAction.started += OnPrevious;

        _attackAction.Enable();
        _attackAction.performed += OnAttack;
        _attackAction.canceled += OnAttack;

        _cameraSwitchAction.Enable();
        _cameraSwitchAction.started += OnCameraSwitch;
    }

    private void OnDisable()
    {
        _moveAction.performed -= OnMove;
        _moveAction.canceled -= OnMove;
        _moveAction.Disable();

        _lookAction.performed -= OnLook;
        _lookAction.canceled -= OnLook;
        _lookAction.Disable();

        _sprintAction.performed -= OnSprint;
        _sprintAction.canceled -= OnSprint;
        _sprintAction.Disable();

        _nextAction.started -= OnNext;
        _nextAction.Disable();

        _previousAction.started -= OnPrevious;
        _previousAction.Disable();

        _attackAction.performed -= OnAttack;
        _attackAction.canceled -= OnAttack;
        _attackAction.Disable();

        _cameraSwitchAction.started -= OnCameraSwitch;
        _cameraSwitchAction.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        sprint = context.ReadValue<float>();
    }

    private void OnNext(InputAction.CallbackContext context)
    {
        NextStarted?.Invoke();
    }

    private void OnPrevious(InputAction.CallbackContext context)
    {
        PreviousStarted?.Invoke();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        attack = context.ReadValue<float>();
    }

    private void OnCameraSwitch(InputAction.CallbackContext context)
    {
        CameraSwitchStarted?.Invoke();
    }
}
