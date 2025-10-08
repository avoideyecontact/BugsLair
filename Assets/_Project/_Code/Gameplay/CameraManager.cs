using Unity.Cinemachine;
using UnityEngine;

// Used in gameplay scene for CinemachineStateDrivenCamera
public class CameraManager : MonoBehaviour
{
    private CinemachineCamera _playerCamera;
    private CinemachineCamera _testCamera;

    private Animator _cameraAnimator;

    private void Awake()    
    {
        _cameraAnimator = GetComponent<Animator>();
        _playerCamera = transform.Find("PlayerCamera").GetComponent<CinemachineCamera>();
        _testCamera = transform.Find("TestCamera").GetComponent<CinemachineCamera>();
    }

    public void SetupPlayerCamera(Transform playerCameraPosition)
    {
        _playerCamera.Follow = playerCameraPosition;
    }
}
