using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Transform _playerCameraTransform;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerInputReader _input;
    [SerializeField] private PlayerWeaponary _weaponary;
    [SerializeField] private AbilityManager _abilities;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private float _interactionDistance = 4;

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
    public AbilityManager AbilityManager => _abilities;
    public PlayerMovement Movement => _movement;
    public CharacterController Controller => _controller;
    public PlayerHealth Health => _health;
    public float InteractionDistance => _interactionDistance;
}
