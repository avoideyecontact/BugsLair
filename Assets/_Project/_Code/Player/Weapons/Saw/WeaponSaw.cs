using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class WeaponSaw : MonoBehaviour, IWeapon
{
    [SerializeField] private string _name = "Saw";
    [SerializeField] protected float _damage = 1f;
    [SerializeField] protected float _damageRate = 0.5f;
    [SerializeField] protected LayerMask _enemyLayer;
    private BoxCollider _damageCollider;    

    public string Name => _name;
    public float Damage => _damage;
    public float DamageRate => _damageRate;

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
        var hits = Physics.OverlapBox(transform.position + _damageCollider.center, _damageCollider.size, Quaternion.identity, _enemyLayer);
        foreach (var hit in hits)
        {
            hit.GetComponent<HealthComponent>()?.DealDamage(_damage);
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

    void OnDrawGizmos()
    {
        var damageCollider = GetComponent<BoxCollider>();
        Gizmos.color = Color.red;
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(damageCollider.center, damageCollider.size);
    }
}
