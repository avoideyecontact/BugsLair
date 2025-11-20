using Unity.Cinemachine;
using UnityEngine;
using VContainer;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField] private Animator _cameraAnimator;

    [Header("Cameras")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _clippingCamera;

    [Header("Virtual Cameras")]
    [SerializeField] private CinemachineCamera _playerCamera;
    [SerializeField] private CinemachineCamera _thirdPersonCamera;

    [Header("Camera Shake")]
    [SerializeField] private float _shakeForce = 1f;
    [SerializeField] private CinemachineImpulseSource _impulseSource;

    private PlayerContext _playerContext;
    private IEventBus _eventBus;
    private string _currentCamera = "Player";
    private LayerMask _layersForFirstPerson;
    private LayerMask _layersForThirdPerson;

    [Inject]
    public void Construct(PlayerContext playerContext, IEventBus eventBus)
    {
        _playerContext = playerContext;
        _eventBus = eventBus;
        SubscribeToEventBus();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEventBus();
    }

    private void SubscribeToEventBus()
    {
        _eventBus.Subscribe<PlayerDamaged>(OnPlayerDamaged);
        _eventBus.Subscribe<PlayerIsUsingWeapon>(OnPlayerIsUsingWeapon);
    }

    private void UnsubscribeFromEventBus()
    {
        _eventBus.Unsubscribe<PlayerDamaged>(OnPlayerDamaged);
        _eventBus.Unsubscribe<PlayerIsUsingWeapon>(OnPlayerIsUsingWeapon);
    }

    private void Start()
    {
        _layersForFirstPerson = ~0 - LayerMask.GetMask("Player");
        _layersForThirdPerson = ~0;

        _playerContext.Input.CameraSwitchStarted += OnCameraSwitch;

        _playerCamera.Follow = _playerContext.PlayerCameraTransform;
        _thirdPersonCamera.Follow = _playerContext.PlayerCameraTransform;
    }

    private void OnCameraSwitch()
    {
        // add state machine
        if (_currentCamera == "Player")
        {
            _currentCamera = "ThirdPerson";
            _cameraAnimator.Play("ThirdPerson");
            EnableWeaponClipping();
            return;
        }
        
        if (_currentCamera == "ThirdPerson")
        {
            _currentCamera = "Player";
            _cameraAnimator.Play("Player");
            DisableWeaponClipping();
            return;
        }
    }

    // for third person
    private void EnableWeaponClipping()
    {
        _mainCamera.cullingMask = _layersForThirdPerson;
        _clippingCamera.enabled = false;
        EnablePostProcessingForCamera(_mainCamera, true);
        EnablePostProcessingForCamera(_clippingCamera, false);
    }

    // for first person
    private void DisableWeaponClipping()
    {
        _mainCamera.cullingMask = _layersForFirstPerson;
        _clippingCamera.enabled = true;
        EnablePostProcessingForCamera(_mainCamera, false);
        EnablePostProcessingForCamera(_clippingCamera, true);
    }

    private void EnablePostProcessingForCamera(Camera camera, bool state)
    {
        var uac = camera.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
        uac.renderPostProcessing = state;
    }

    private void OnPlayerDamaged(PlayerDamaged evt)
    {
        _impulseSource.GenerateImpulseWithForce(_shakeForce);
    }

    private void OnPlayerIsUsingWeapon(PlayerIsUsingWeapon evt)
    {
        if (evt.weaponType == WeaponType.Minigun)
            _impulseSource.GenerateImpulseWithForce(_shakeForce / 5);
    }
}
