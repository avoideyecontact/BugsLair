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
    private InputAction _jumpAction;
    private InputAction _interactAction;
    private InputAction _nextAction;
    private InputAction _previousAction;
    private InputAction _attackAction;
    private InputAction _cameraSwitchAction;
    private InputAction _dashAction;
    private InputAction _inventoryAction;

    public event System.Action JumpStarted;
    public event System.Action DashStarted;
    public event System.Action InteractStarted;
    public event System.Action NextStarted;
    public event System.Action PreviousStarted;
    public event System.Action CameraSwitchStarted;
    public event System.Action InventoryStarted;

    [Inject]
    public void Construct(IEventBus eventBus)
    {
        _eventBus = eventBus;
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
        _jumpAction = _actionsAsset.FindAction("Player/Jump", true);
        _dashAction = _actionsAsset.FindAction("Player/Dash", true);
        _interactAction = _actionsAsset.FindAction("Player/Interact", true);
        _nextAction = _actionsAsset.FindAction("Player/Next", true);
        _previousAction = _actionsAsset.FindAction("Player/Previous", true);
        _attackAction = _actionsAsset.FindAction("Player/Attack", true);
        _cameraSwitchAction = _actionsAsset.FindAction("Player/CameraSwitch", true);
        _inventoryAction = _actionsAsset.FindAction("Player/Inventory", true);
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

        _jumpAction.Enable();
        _jumpAction.started += OnJump;

        _dashAction.Enable();
        _dashAction.started += OnDash;

        _interactAction.Enable();
        _interactAction.started += OnInteract;

        _nextAction.Enable();
        _nextAction.started += OnNext;

        _previousAction.Enable();
        _previousAction.started += OnPrevious;

        _attackAction.Enable();
        _attackAction.performed += OnAttack;
        _attackAction.canceled += OnAttack;

        _cameraSwitchAction.Enable();
        _cameraSwitchAction.started += OnCameraSwitch;

        _inventoryAction.Enable();
        _inventoryAction.started += OnInventory;

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

        _jumpAction.started -= OnJump;
        _jumpAction.Disable();

        _dashAction.started -= OnDash;
        _dashAction.Disable();

        _interactAction.started -= OnInteract;
        _interactAction.Disable();

        _nextAction.started -= OnNext;
        _nextAction.Disable();

        _previousAction.started -= OnPrevious;
        _previousAction.Disable();

        _attackAction.performed -= OnAttack;
        _attackAction.canceled -= OnAttack;
        _attackAction.Disable();

        _cameraSwitchAction.started -= OnCameraSwitch;
        _cameraSwitchAction.Disable();

        _inventoryAction.started -= OnInventory;
        _inventoryAction.Disable();
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

    private void OnJump(InputAction.CallbackContext context)
    {
        JumpStarted?.Invoke();
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        DashStarted?.Invoke();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        InteractStarted?.Invoke();
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

    private void OnInventory(InputAction.CallbackContext context)
    {
        InventoryStarted?.Invoke();
    }
}
