using UnityEngine;
using VContainer;

public class LasergunAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator _animator1;
    [SerializeField] private Animator _animator2;

    private PlayerContext _playerContext;

    [Inject]
    public void Construct(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    private void FixedUpdate()
    {
        _animator1.SetFloat("Attack", _playerContext.Input.attack);
        _animator2.SetFloat("Attack", _playerContext.Input.attack);
    }

    private void Update()
    {
        transform.rotation = _playerContext.PlayerCameraTransform.rotation;
    }
}
