using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer;

public class DashAbility : AbilityBase
{
    [SerializeField] private float _dashPower = 5f;
    [SerializeField] private float _dashDuration = 0.25f;
    [SerializeField] private float _cooldownTime = 2f;

    private PlayerContext _playerContext;
    private CancellationTokenSource _cooldownCts;
    private bool _isCooldown;

    [Inject]
    public void Construct(PlayerContext playerContext)
    {
        _playerContext = playerContext;
        SubscribeToInput();
    }

    private void SubscribeToInput() => _playerContext.Input.JumpStarted += Dash;
    private void UnsubscribeFromInput() => _playerContext.Input.JumpStarted -= Dash;

    private void Dash()
    {
        if (!IsActive)
            return;

        if (_isCooldown)
            return;

        if (_playerContext.Input.move == Vector2.zero)
            return;

        _playerContext.Movement.Dash(_dashPower, _dashDuration);

        _cooldownCts = new CancellationTokenSource();
        StartAbilityCooldown(_cooldownCts.Token).Forget();
    }

    private async UniTask StartAbilityCooldown(CancellationToken cts)
    {
        _isCooldown = true;
        await UniTask.WaitForSeconds(_cooldownTime, cancellationToken: cts);
        _isCooldown = false;
    }

    private void OnDestroy()
    {
        UnsubscribeFromInput();
        _cooldownCts?.Cancel();
        _cooldownCts?.Dispose();
    }
}
