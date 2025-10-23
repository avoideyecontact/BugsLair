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

    private PlayerContext _playerContext;
    private bool _isCooldown;
    private CancellationTokenSource _cts1;
    private CancellationTokenSource _cts2;

    [Inject]
    public void Construct(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    private void Start()
    {
        SetLasersActive(false);
    }

    public void Use()
    {
        _cts2?.Cancel();
        _cts2 = new CancellationTokenSource();
        LasersFire(_cts2.Token).Forget();

        if (_isCooldown)
            return;

        DealDamage();

        _cts1 = new CancellationTokenSource();
        WeaponCooldownTimer(_cts1.Token).Forget();
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

    private async UniTask LasersFire(CancellationToken cts)
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

    private async UniTask WeaponCooldownTimer(CancellationToken cts)
    {
        _isCooldown = true;
        await UniTask.WaitForSeconds(_config.damageRate, cancellationToken: cts);
        _isCooldown = false;
    }

    private void OnDestroy()
    {
        _cts1?.Cancel();
        _cts1?.Dispose();
        _cts2?.Cancel();
        _cts2?.Dispose();
    }
}
