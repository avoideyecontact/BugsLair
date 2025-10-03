using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInputReader _input;
    private CharacterController _controller;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _sprintSpeed = 10f;

    void Start()
    {
        _input = GetComponent<PlayerInputReader>();
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveDirection = transform.forward * _input.move.y + transform.right * _input.move.x;

        if (_input.sprint > 0)
        {
            _controller.SimpleMove(moveDirection * _sprintSpeed);
        }
        else
        {
            _controller.SimpleMove(moveDirection * _speed);
        }
    }
}
