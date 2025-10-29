using UnityEngine;
using VContainer;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private float _sprintMultiplier = 2;
    [SerializeField] private Animator _animator;

    private PlayerContext _playerContext;

    [Inject]
    public void Construct(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    void FixedUpdate()
    {
        _animator.SetFloat("Move", _playerContext.Input.move.magnitude);

        var speedMultiplier = _playerContext.Input.move.magnitude * Mathf.Sign(_playerContext.Input.move.y);

        if (_playerContext.Input.sprint > 0)
            speedMultiplier *= _sprintMultiplier;

        _animator.SetFloat("SpeedMultiplier", speedMultiplier);
    }
}
