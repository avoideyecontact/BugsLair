using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform _tower;
    [SerializeField] private Transform _turret;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private ParticleSystem _effect;

    [SerializeField] private float _damage = 2f;
    [SerializeField] private float _damageRadius = 45f;
    [SerializeField] private float _damageCooldown = 1f;
    [SerializeField] private float _targetSearchCooldown = 5f;
    [SerializeField] private Transform _target;

    private void Start()
    {
        Behaviour().Forget();
    }

    private async UniTask Behaviour()
    {
        var ct = this.GetCancellationTokenOnDestroy();
        await UniTask.WaitForSeconds(1f);

        while (!ct.IsCancellationRequested)
        {
            TryToFindTarget();

            while (_target != null && !ct.IsCancellationRequested)
            {
                await Aim();
                DealDamage();

                _audio.pitch = Random.Range(0.9f, 1.1f);
                _audio.Play();

                _effect.Play();

                if (_target == null)
                {
                    await ResetOrientation();
                    break;
                }
                await UniTask.WaitForSeconds(_damageCooldown, cancellationToken: ct);
            }

            await UniTask.WaitForSeconds(_targetSearchCooldown, cancellationToken: ct);
        }
    }

    private void TryToFindTarget()
    {
        var enemies = GameObject.FindObjectsByType<CockroachAI>(FindObjectsSortMode.None);

        if (enemies.Length == 0)
        {
            _target = null;
            return;
        }

        var closestEnemy = enemies[0];
        float closestDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);

        foreach (var enemy in enemies)
        {
            var distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < closestDistance && !enemy.GetComponent<HealthComponent>().IsDead)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        if (closestDistance > _damageRadius)
        {
            _target = null;
            return;
        }

        _target = closestEnemy.transform;
    }

    private async UniTask Aim()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        var aim1 = _tower.DOLookAt(_target.position, 0.25f, AxisConstraint.Y, _tower.up).SetEase(Ease.InOutQuint).WithCancellation(ct);
        var aim2 = _turret.DOLookAt(_target.position, 0.25f, up: _turret.up).SetEase(Ease.InOutQuint).WithCancellation(ct);

        await UniTask.WhenAll(aim1, aim2);
    }

    private async UniTask ResetOrientation()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        var reset1 = _tower.DORotate(Vector3.zero, 1f, RotateMode.Fast).SetEase(Ease.InOutQuint).WithCancellation(ct);
        var reset2 = _turret.DORotate(Vector3.zero, 1f, RotateMode.Fast).SetEase(Ease.InOutQuint).WithCancellation(ct);

        await UniTask.WhenAll(reset1, reset2);
    }

    private void DealDamage()
    {
        if (_target == null)
            return;

        var health = _target.GetComponent<HealthComponent>();
        
        health.DealDamage(_damage);

        if (health.IsDead)
        {
            _target = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _damageRadius);

        if (_target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_target.position, Vector3.one * 3);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, _target.position);
        }
    }
}
