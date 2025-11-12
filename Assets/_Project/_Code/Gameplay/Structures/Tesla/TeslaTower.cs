using Cysharp.Threading.Tasks;
using UnityEngine;

public class TeslaTower : MonoBehaviour
{
    [SerializeField] private Transform _teslaCenter;
    [SerializeField] private float _radius = 150;
    [SerializeField] private float _damage = 1;
    [SerializeField] private float _cooldown = 0.05f;

    private Transform _player;
    private bool _isCooldown;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (_player == null)
            return;

        if (_isCooldown)
            return;

        if (Vector3.Distance(_player.position, _teslaCenter.position) <= _radius)
        {
            var health = _player.GetComponent<PlayerHealth>();
            health.DealDamage(_damage);
            StartCooldown().Forget();
        }
    }

    private async UniTask StartCooldown()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        _isCooldown = true;
        await UniTask.WaitForSeconds(_cooldown, cancellationToken: ct);
        _isCooldown = false;
    }

    private void OnDrawGizmos()
    {
        if (_teslaCenter == null)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_teslaCenter.position, _radius);
    }
}
