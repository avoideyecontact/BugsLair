using UnityEngine;
using VContainer;

public class SawAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator _animator1;
    [SerializeField] private Animator _animator2;
    [SerializeField] private Animator _bladeAnimator1;
    [SerializeField] private Animator _bladeAnimator2;

    private PlayerContext _playerContext;

    [Inject]
    public void Construct(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    void FixedUpdate()
    {
        _animator1.SetFloat("Attack", _playerContext.Input.attack);
        _animator2.SetFloat("Attack", _playerContext.Input.attack);
        _bladeAnimator1.SetFloat("Attack", _playerContext.Input.attack);
        _bladeAnimator2.SetFloat("Attack", _playerContext.Input.attack);

        var speedMultiplier = 1;

        if (_playerContext.Input.sprint > 0)
        {
            speedMultiplier = 4;
        }

        _animator1.SetFloat("SpeedMultiplier", speedMultiplier);
        _animator2.SetFloat("SpeedMultiplier", speedMultiplier);
    }

    private void Update()
    {
        transform.rotation = _playerContext.PlayerCameraTransform.rotation;
    }
}
