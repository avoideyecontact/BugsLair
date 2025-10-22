using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer;

public class WeaponMinigun : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponType _weaponType = WeaponType.Minigun;
    [SerializeField] protected float _damage = 1f;
    [SerializeField] protected float _damageRate = 0.1f;
    [SerializeField] private float _hitDistance = 50f;
    [SerializeField] private LayerMask _enemyLayer;

    public WeaponType WeaponType => _weaponType;
    public float Damage => _damage;
    public float DamageRate => _damageRate;
    public bool Available { get; set; }
    public bool Selected { get; set; }

    private PlayerContext _playerContext;
    private bool _isCooldown;
    private CancellationTokenSource _cts;

    [Inject]
    public void Construct(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    public void Use()
    {
        if (_isCooldown)
            return;

        DealDamage();

        _cts = new CancellationTokenSource();
        WeaponCooldownTimer(_cts.Token).Forget();
    }

    // change
    private void DealDamage()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = _playerContext.MainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _hitDistance, _enemyLayer))
        {
            hit.transform.GetComponent<HealthComponent>()?.DealDamage(_damage);
        }
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
