using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class WeaponSaw : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponConfig _config;

    public WeaponType WeaponType => _config.weaponType;
    public float Damage => _config.damage;
    public float DamageRate => _config.damageRate;
    public int Ammo => -1;
    public bool Available { get; set; }
    public bool Selected { get; set; }

    private BoxCollider _damageCollider;
    private bool _isCooldown;
    private CancellationTokenSource _cts;

    private void Start()
    {
        _damageCollider = GetComponent<BoxCollider>();
    }

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
        var hits = Physics.OverlapBox(
            transform.position + _damageCollider.center,
            _damageCollider.size,
            Quaternion.identity,
            _config.enemyLayer);

        foreach (var hit in hits)
        {
            hit.GetComponent<HealthComponent>()?.DealDamage(_config.damage);
        }
    }

    private async UniTask WeaponCooldownTimer(CancellationToken cts)
    {
        _isCooldown = true;
        await UniTask.WaitForSeconds(_config.damageRate, cancellationToken: cts);
        _isCooldown = false;
    }

    public void AddAmmo(int value)
    {
        Debug.LogWarning("Saw doesnt use ammo");
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }

    void OnDrawGizmos()
    {
        var damageCollider = GetComponent<BoxCollider>();
        Gizmos.color = Color.red;
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(damageCollider.center, damageCollider.size);
    }
}
