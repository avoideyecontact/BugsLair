using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _sprintSpeed = 10f;
    [SerializeField] private float _footstepsCooldownTime = 0.1f;
    [SerializeField] private SoundData[] _footstepsSounds;

    [Header("SphereMode")]
    [SerializeField] private float _force = 100;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider _sphereCollider;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private PlayerCameraManager _playerCameraManager;
    [SerializeField] private PlayerLook _playerLook;
    [SerializeField] private PlayerWeaponary _playerWeaponary;
    [SerializeField] private GameObject _legs;
    [SerializeField] private Transform _bugslayerTransform;
    private bool _sphereModeON = false;

    private PlayerContext _playerContext;
    private Vector3 _verticalVelocity;
    private bool _jumpRequested;
    private CancellationTokenSource _dashCts;
    private bool _footstepsCooldown;

    [Inject]
    public void Construct(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleSphereMode();
        }

        Move();
        ApplyVerticalVelocity();
    }

    public void ToggleSphereMode()
    {
        _sphereModeON = !_sphereModeON;

        if (_sphereModeON)
        {
            _playerCameraManager.EnableFreeLook();
        }
        else
        {
            _playerCameraManager.EnablePlayer();
        }

        _sphereCollider.enabled = _sphereModeON;
        _playerWeaponary.gameObject.SetActive(!_sphereModeON);
        _legs.SetActive(!_sphereModeON);
        _controller.enabled = !_sphereModeON;
        _rb.isKinematic = !_sphereModeON;
        _playerLook.enabled = !_sphereModeON;
    }

    private void Move()
    {
        //if (_playerContext.Input.move == Vector2.zero)
        //    return;

        if (_sphereModeON)
        {
            _bugslayerTransform.rotation = _cameraTransform.rotation;

            Vector3 force =
                _cameraTransform.right * _playerContext.Input.move.y
                + -_cameraTransform.forward * _playerContext.Input.move.x;

            force *= _force * _rb.mass;

            _rb.AddTorque(force);
            _rb.AddForce(Physics.gravity * 1 * _rb.mass);

            return;
        }

        Vector3 moveDirection = 
            transform.forward * _playerContext.Input.move.y
            + transform.right * _playerContext.Input.move.x;

        if (_playerContext.Input.sprint > 0)
        {
            _controller.Move(moveDirection * _sprintSpeed * Time.deltaTime);

            if (!_footstepsCooldown)
            {
                SoundManager.Instance.CreateSoundBuilder()
                    .WithRandomPitch()
                    .WithPosition(transform.position)
                    .Play(_footstepsSounds[Random.Range(0, _footstepsSounds.Length)]);

                FootstepsSoundCooldown(_footstepsCooldownTime / 2).Forget();
            }
        }
        else
        {
            _controller.Move(moveDirection * _speed * Time.deltaTime);

            if (!_footstepsCooldown)
            {
                SoundManager.Instance.CreateSoundBuilder()
                    .WithRandomPitch()
                    .WithPosition(transform.position)
                    .Play(_footstepsSounds[Random.Range(0, _footstepsSounds.Length)]);

                FootstepsSoundCooldown(_footstepsCooldownTime).Forget();
            }
        }
    }

    private void ApplyVerticalVelocity()
    {
        if (_sphereModeON) return;

        _verticalVelocity += Physics.gravity * Time.deltaTime;

        if (_controller.isGrounded && !_jumpRequested)
            _verticalVelocity.y = 0;

        _jumpRequested = false;

        _controller.Move(_verticalVelocity * Time.deltaTime);
    }

    public void Jump(float jumpPower)
    {
        _jumpRequested = true;
        _verticalVelocity.y = jumpPower;
    }

    public void Dash(float dashPower, float duration)
    {
        var move = _playerContext.Input.move;
        var player = _playerContext.PlayerTransform;
        var direction = player.forward * move.y + player.right * move.x;

        CancelDash();
        _dashCts = new CancellationTokenSource();
        DashTask(direction, dashPower, duration).Forget();
    }

    public void CancelDash()
    {
        _dashCts?.Cancel();
        _dashCts?.Dispose();
        _dashCts = null;
    }

    private async UniTask DashTask(Vector3 direction, float dashPower, float duration)
    {
        float elapsed = 0f;
        var ct = _dashCts.Token;

        while (elapsed < duration)
        {
            _controller.Move(direction * dashPower * Time.deltaTime);
            elapsed += Time.deltaTime;
            await UniTask.Yield(ct);
        }
    }

    private async UniTask FootstepsSoundCooldown(float cooldown)
    {
        var ct = this.GetCancellationTokenOnDestroy();
        _footstepsCooldown = true;
        await UniTask.WaitForSeconds(cooldown, cancellationToken: ct);
        _footstepsCooldown = false;
    }

    private void OnDestroy()
    {
        CancelDash();
    }
}
