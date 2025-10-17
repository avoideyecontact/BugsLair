using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class WeaponMinigun : MonoBehaviour, IWeapon
{
    [SerializeField] private string _name = "Minigun";
    [SerializeField] protected float _damage = 1f;
    [SerializeField] protected float _damageRate = 0.1f;
    [SerializeField] private float _hitDistance = 50f;
    [SerializeField] private LayerMask _enemyLayer;

    public string Name => _name;
    public float Damage => _damage;
    public float DamageRate => _damageRate;

    private bool _isCooldown;
    private CancellationTokenSource _cts;

    public void Use()
    {
        if (_isCooldown)
            return;

        DealDamage();

        _cts = new CancellationTokenSource();
        WeaponCooldownTimer(_cts.Token).Forget();
    }

    private void DealDamage()
    {
        Camera camera = Camera.main;
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = camera.ScreenPointToRay(screenCenter);
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

    private void OnDrawGizmos()
    {
        Camera camera = Camera.main;
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        var test = camera.ScreenToWorldPoint(screenCenter);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(test, transform.forward * _hitDistance);
    }
}
