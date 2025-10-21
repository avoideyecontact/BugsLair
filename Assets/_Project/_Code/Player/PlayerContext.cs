using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Transform _playerCameraTransform;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerInputReader _input;
    [SerializeField] private PlayerWeaponary _weaponary;

    private void Awake()
    {
        _mainCamera = Camera.main;

        if (_mainCamera == null)
            Debug.LogError("PlayerContext: MainCamera is missing", this);
    }

    public Camera MainCamera => _mainCamera;
    public Transform PlayerCameraTransform => _playerCameraTransform;
    public Transform PlayerTransform => _playerTransform;
    public PlayerInputReader Input => _input;
    public PlayerWeaponary Weaponary => _weaponary;
}
