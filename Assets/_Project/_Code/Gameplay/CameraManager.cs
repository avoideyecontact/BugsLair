using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using VContainer;

// Used in gameplay scene for CinemachineStateDrivenCamera
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Camera _clippingCamera;
    [SerializeField] private LayerMask _layersForFirstPerson;
    [SerializeField] private LayerMask _layersForThirdPerson;
    [SerializeField] private CinemachineCamera _playerCamera;
    [SerializeField] private CinemachineCamera _testCamera;
    [SerializeField] private CinemachineCamera _thirdPersonCamera;
    [SerializeField] private CinemachineStateDrivenCamera _stateDrivenCamera;

    private PlayerContext _playerContext;
    private Animator _cameraAnimator;
    private string _currentCamera = "Player";

    [Inject]
    public void Construct(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    private void Start()
    {
        _cameraAnimator = GetComponent<Animator>();

        _playerContext.Input.CameraSwitchStarted += OnCameraSwitch;

        _playerCamera.Follow = _playerContext.PlayerCameraTransform;
        _thirdPersonCamera.Follow = _playerContext.PlayerCameraTransform;

        // change to something better
        TaskForCutscene().Forget();
    }

    private void OnCameraSwitch()
    {
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

    // change to something better
    private async UniTask TaskForCutscene()
    {
        var ct = this.GetCancellationTokenOnDestroy();
        EnableWeaponClipping();
        await UniTask.WaitForSeconds(4f, cancellationToken: ct);
        DisableWeaponClipping();
    }
}
