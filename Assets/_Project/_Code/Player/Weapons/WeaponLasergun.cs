using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class WeaponLasergun : MonoBehaviour, IWeapon
{
    [SerializeField] protected float _damage = 1f;
    [SerializeField] protected float _damageRate = 0.1f;

    public float Damage => _damage;
    public float DamageRate => _damageRate;

    private bool _isCooldown;
    private CancellationTokenSource _cts;

    public void Use()
    {
        if (_isCooldown)
            return;

        Debug.Log("Using laser");

        _cts = new CancellationTokenSource();
        WeaponCooldownTimer(_cts.Token).Forget();
    }

    private async UniTask WeaponCooldownTimer(CancellationToken cts)
    {
        _isCooldown = true;
        await UniTask.WaitForSeconds(_damageRate, cancellationToken: cts);
        _isCooldown = false;
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
