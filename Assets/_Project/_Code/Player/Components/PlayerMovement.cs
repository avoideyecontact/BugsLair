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

    private PlayerContext _playerContext;
    private Vector3 _verticalVelocity;
    private bool _jumpRequested;
    private CancellationTokenSource _dashCts;

    [Inject]
    public void Construct(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    void Update()
    {
        Move();
        ApplyVerticalVelocity();
    }

    private void Move()
    {
        Vector3 moveDirection = 
            transform.forward * _playerContext.Input.move.y
            + transform.right * _playerContext.Input.move.x;

        if (_playerContext.Input.sprint > 0)
            _controller.Move(moveDirection * _sprintSpeed * Time.deltaTime);
        else
            _controller.Move(moveDirection * _speed * Time.deltaTime);
    }

    private void ApplyVerticalVelocity()
    {
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

    private void OnDestroy()
    {
        CancelDash();
    }
}
