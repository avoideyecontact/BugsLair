using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class WeaponLasergun : MonoBehaviour, IWeapon
{
    [SerializeField] private string _name = "Lasergun";
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _damageRate = 0.1f;
    [SerializeField] private float _hitDistance = 100f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LaserBeam _laserBeam1;
    [SerializeField] private LaserBeam _laserBeam2;

    public string Name => _name;
    public float Damage => _damage;
    public float DamageRate => _damageRate;

    private bool _isCooldown;
    private CancellationTokenSource _cts1;
    private CancellationTokenSource _cts2;

    private void Start()
    {
        _laserBeam1.gameObject.SetActive(false);
        _laserBeam2.gameObject.SetActive(false);
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
        Camera camera = Camera.main;
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        var start = camera.ScreenToWorldPoint(screenCenter);
        var hits = Physics.RaycastAll(start, transform.forward, _hitDistance, _enemyLayer);
        foreach (var hit in hits)
        {
            var damage = _damage * (hit.distance / _hitDistance);
            hit.transform.GetComponent<HealthComponent>()?.DealDamage(_damage);
        }
    }

    private async UniTask LasersFire(CancellationToken cts)
    {
        _laserBeam1.gameObject.SetActive(true);
        _laserBeam2.gameObject.SetActive(true);
        await UniTask.WaitForSeconds(0.1f, cancellationToken: cts);
        _laserBeam1.gameObject.SetActive(false);
        _laserBeam2.gameObject.SetActive(false);
    }

    private async UniTask WeaponCooldownTimer(CancellationToken cts)
    {
        _isCooldown = true;
        await UniTask.WaitForSeconds(_damageRate, cancellationToken: cts);
        _isCooldown = false;
    }

    private void OnDestroy()
    {
        _cts1?.Cancel();
        _cts1?.Dispose();
        _cts2?.Cancel();
        _cts2?.Dispose();
    }

    private void OnDrawGizmos()
    {
        Camera camera = Camera.main;
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        var test = camera.ScreenToWorldPoint(screenCenter);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(test, transform.forward * _hitDistance);
    }
}
