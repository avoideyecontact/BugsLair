using Unity.Cinemachine;
using UnityEngine;

// Used in gameplay scene for CinemachineStateDrivenCamera
public class CameraManager : MonoBehaviour
{
    private CinemachineCamera _playerCamera;
    private CinemachineCamera _testCamera;
    private CinemachineCamera _thirdPersonCamera;
    private string _currentCamera = "Player";

    private PlayerInputReader _input;
    private Transform _playerCameraTransform;
    private Animator _cameraAnimator;

    private void Awake()    
    {
        _cameraAnimator = GetComponent<Animator>();
        _playerCamera = transform.Find("PlayerCamera").GetComponent<CinemachineCamera>();
        _testCamera = transform.Find("TestCamera").GetComponent<CinemachineCamera>();
        _thirdPersonCamera = transform.Find("ThirdPersonCamera").GetComponent<CinemachineCamera>();
    }

    public void SetupPlayer(Transform player)
    {
        _input = player.GetComponent<PlayerInputReader>();
        _input.CameraSwitchStarted += OnCameraSwitch;

        _playerCameraTransform = player.GetComponent<PlayerCamera>().CameraTransform;
        _playerCamera.Follow = _playerCameraTransform;
        _thirdPersonCamera.Follow = _playerCameraTransform;
    }

    private void OnCameraSwitch()
    {
        if (_currentCamera == "Player")
        {
            _currentCamera = "ThirdPerson";
            _cameraAnimator.Play("ThirdPerson");
            return;
        }
        
        if (_currentCamera == "ThirdPerson")
        {
            _currentCamera = "Player";
            _cameraAnimator.Play("Player");
            return;
        }
    }
}
