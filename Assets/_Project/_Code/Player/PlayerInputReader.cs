using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] private InputActionAsset _actionsAsset;

    public Vector2 move;
    public Vector2 look;
    public float sprint;

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _sprintAction;
    private InputAction _nextAction;
    private InputAction _previousAction;

    public event System.Action<float> NextStarted;
    public event System.Action<float> PreviousStarted;

    private void Awake()
    {
        if (_actionsAsset == null)
            Debug.LogError("InputActionAsset is missing", this);

        _moveAction = _actionsAsset.FindAction("Player/Move", true);
        _lookAction = _actionsAsset.FindAction("Player/Look", true);
        _sprintAction = _actionsAsset.FindAction("Player/Sprint", true);
        _nextAction = _actionsAsset.FindAction("Player/Next", true);
        _previousAction = _actionsAsset.FindAction("Player/Previous", true);

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
        NextStarted?.Invoke(context.ReadValue<float>());
    }

    private void OnPrevious(InputAction.CallbackContext context)
    {
        PreviousStarted?.Invoke(context.ReadValue<float>());
    }
}
