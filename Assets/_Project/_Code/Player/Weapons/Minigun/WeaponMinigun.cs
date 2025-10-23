using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer;

public class WeaponMinigun : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponConfig _config;

    public WeaponType WeaponType => _config.weaponType;
    public float Damage => _config.damage;
    public float DamageRate => _config.damageRate;
    public bool Available { get; set; }
    public bool Selected { get; set; }

    private PlayerContext _playerContext;
    private AmmoSystem _ammoSystem;
    private bool _isCooldown;
    private CancellationTokenSource _cooldownCts;

    [Inject]
    public void Construct(PlayerContext playerContext)
    {
        _playerContext = playerContext;
        _ammoSystem = new AmmoSystem(_config.maxAmmo);
    }

    public void Use()
    {
        Debug.Log($"{WeaponType}: {_ammoSystem.CurrentAmmo}");

        if (!_ammoSystem.HasAmmo)
            return;

        if (_isCooldown)
            return;

        _ammoSystem.SpendAmmo();
        DealDamage();

        _cooldownCts = new CancellationTokenSource();
        WeaponCooldownTimer(_cooldownCts.Token).Forget();
    }

    private void DealDamage()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = _playerContext.MainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _config.hitDistance, _config.enemyLayer))
        {
            hit.transform.GetComponent<HealthComponent>()?.DealDamage(_config.damage);
        }
    }

    private async UniTask WeaponCooldownTimer(CancellationToken cts)
    {
        _isCooldown = true;
        await UniTask.WaitForSeconds(_config.damageRate, cancellationToken: cts);
        _isCooldown = false;
    }

    private void OnDestroy()
    {
        _cooldownCts?.Cancel();
        _cooldownCts?.Dispose();
    }
}
