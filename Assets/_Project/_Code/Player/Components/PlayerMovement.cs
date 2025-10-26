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
}
