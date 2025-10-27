using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VContainer;

public class WeaponLasergun : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponConfig _config;

    [SerializeField] private LaserBeam _laserBeam1;
    [SerializeField] private LaserBeam _laserBeam2;

    public WeaponType WeaponType => _config.weaponType;
    public float Damage => _config.damage;
    public float DamageRate => _config.damageRate;
    public bool Available { get; set; }
    public bool Selected { get; set; }
    public int Ammo => _ammoSystem.CurrentAmmo;

    private PlayerContext _playerContext;
    private IEventBus _eventBus;
    private AmmoSystem _ammoSystem;
    private bool _isCooldown;
    private CancellationTokenSource _cooldownCts;
    private CancellationTokenSource _lasersCts;

    [Inject]
    public void Construct(PlayerContext playerContext, IEventBus eventBus)
    {
        _playerContext = playerContext;
        _eventBus = eventBus;
        _ammoSystem = new AmmoSystem(_config.maxAmmo);
    }

    private void Start()
    {
        SetLasersActive(false);
    }

    public void Use()
    {
        if (!_ammoSystem.HasAmmo)
            return;

        _lasersCts?.Cancel();
        _lasersCts = new CancellationTokenSource();
        ActivateLaserEffects(_lasersCts.Token).Forget();

        if (_isCooldown)
            return;

        _ammoSystem.SpendAmmo();
        OnAmmoChanged();
        DealDamage();

        _cooldownCts?.Cancel();
        _cooldownCts = new CancellationTokenSource();
        StartWeaponCooldown(_cooldownCts.Token).Forget();
    }

    private void DealDamage()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        var start = _playerContext.MainCamera.ScreenToWorldPoint(screenCenter);
        var hits = Physics.RaycastAll(start, transform.forward, _config.hitDistance, _config.enemyLayer);
        foreach (var hit in hits)
        {
            var damage = _config.damage * (hit.distance / _config.hitDistance);
            hit.transform.GetComponent<HealthComponent>()?.DealDamage(_config.damage);
        }
    }

    private async UniTask ActivateLaserEffects(CancellationToken cts)
    {
        SetLasersActive(true);
        await UniTask.WaitForSeconds(0.1f, cancellationToken: cts);
        SetLasersActive(false);
    }

    private void SetLasersActive(bool active)
    {
        _laserBeam1?.SetActive(active);
        _laserBeam2?.SetActive(active);
    }

    private async UniTask StartWeaponCooldown(CancellationToken cts)
    {
        _isCooldown = true;
        await UniTask.WaitForSeconds(_config.damageRate, cancellationToken: cts);
        _isCooldown = false;
    }

    public void AddAmmo(int value)
    {
        _ammoSystem.AddAmmo(value);
        OnAmmoChanged();
    }

    private void OnAmmoChanged()
    {
        _eventBus.Publish(new AmmoChanged
        {
            Ammo = _ammoSystem.CurrentAmmo,
            WeaponType = WeaponType
        });
    }

    private void OnDestroy()
    {
        _cooldownCts?.Cancel();
        _cooldownCts?.Dispose();
        _lasersCts?.Cancel();
        _lasersCts?.Dispose();
    }
}
