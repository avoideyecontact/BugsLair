using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UFO : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector3 _movementLimit = new Vector3(800, 100, 800);

    [Header("Effects")]
    [SerializeField] private GameObject _explosion;

    [Header("Components")]
    [SerializeField] private HealthComponent _health;
    [SerializeField] private Transform _door1;
    [SerializeField] private Transform _door2;
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Damage")]
    [SerializeField] private float _damage = 25;
    [SerializeField] private float _damageRadius = 15;

    [Header("Gravity")]
    [SerializeField] private float attractionForce = 1000f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float smoothTime = 0.3f;
    private Vector3 currentVelocity;

    private Transform _playerTransform;
    private CharacterController _playerController;
    private Vector3 _initialPosition;
    private bool _isAttacking;
    private bool _isPulling;

    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _playerController = _playerTransform.GetComponent<CharacterController>();
        _initialPosition = transform.position;

        _health.OnHealthChange += BecomeAngry;
        _health.OnDeath += OnDeath;

        MovementBehaviour().Forget();
    }

    private void Update()
    {
        if (_isPulling)
            PullPlayer();
    }

    private async UniTask MovementBehaviour()
    {
        var ct = this.GetCancellationTokenOnDestroy();
        var spinTask = Spinning();

        while (!ct.IsCancellationRequested)
        {
            if (!_isAttacking)
            {
                await FlyTowardsRandomLocation();
                await UniTask.WaitForSeconds(5f, cancellationToken: ct);
            }
            else
            {
                await AttackPlayer();
                await UniTask.WaitForSeconds(3);
            }
        }
    }

    private async UniTask FlyTowardsRandomLocation()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        float x = _initialPosition.x + Random.Range(-_movementLimit.x, _movementLimit.x);
        float y = _initialPosition.y + Random.Range(-_movementLimit.y, _movementLimit.y);
        float z = _initialPosition.z + Random.Range(-_movementLimit.z, _movementLimit.z);
        var newPosition = new Vector3(x, y, z);

        await transform.DOMove(newPosition, 2f).SetEase(Ease.InOutSine).WithCancellation(ct);
    }

    private async UniTask FlyTowardsPlayer()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        var newPosition = _playerTransform.position;
        newPosition.y += 25;

        await transform.DOMove(newPosition, 1f).SetEase(Ease.InOutSine).WithCancellation(ct);
    }

    private void BecomeAngry()
    {
        _health.OnHealthChange -= BecomeAngry;
        _isAttacking = true;
    }

    private async UniTask AttackPlayer()
    {
        var ct = this.GetCancellationTokenOnDestroy();
        await FlyTowardsPlayer();
        await OpenDoors();
        _isPulling = true;
        await UniTask.WaitForSeconds(3f, cancellationToken: ct);
        if (HorizontalDistanceToPlayer() <= _damageRadius)
            DealDamage();
        _isPulling = false;
        await CloseDoors();
    }

    private void DealDamage()
    {
        var health = _playerTransform.gameObject.GetComponent<PlayerHealth>();
        health.DealDamage(_damage);
    }

    private void PullPlayer()
    {
        Vector3 direction = _playerTransform.position - transform.position;
        float distance = direction.magnitude;

        if (distance > minDistance)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(
                transform.position,
                _playerTransform.position,
                ref currentVelocity,
                smoothTime,
                attractionForce
            );

            _playerController.Move(transform.position - targetPosition);
        }
    }

    private float HorizontalDistanceToPlayer()
    {
        Vector3 a = transform.position;
        Vector3 b = _playerTransform.position;
        a.y = 0;
        b.y = 0;

        return Vector3.Distance( a, b );
    }

    private async UniTask Spinning()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        var spinTask = transform.DOLocalRotate(new Vector3(0, 360, 0), 4f, RotateMode.FastBeyond360)
                .SetRelative()
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart)
                .WithCancellation(ct);

        await UniTask.WaitUntilCanceled(ct);
    }

    private async UniTask OpenDoors()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        var open1 = _door1.DOLocalMove(new Vector3(0, 0.5f, -2f), 1f).SetEase(Ease.OutQuint).WithCancellation(ct);
        var open2 = _door2.DOLocalMove(new Vector3(0, 0.5f, 2f), 1f).SetEase(Ease.OutQuint).WithCancellation(ct);

        await UniTask.WhenAll(open1, open2);
    }

    private async UniTask CloseDoors()
    {
        var ct = this.GetCancellationTokenOnDestroy();

        var open1 = _door1.DOLocalMove(Vector3.zero, 1f).SetEase(Ease.OutQuint).WithCancellation(ct);
        var open2 = _door2.DOLocalMove(Vector3.zero, 1f).SetEase(Ease.OutQuint).WithCancellation(ct);

        await UniTask.WhenAll(open1, open2);
    }

    private async UniTask DeathTask()
    {
        await CloseDoors();
        Boom();
        Fall();
        Destroy(this);
    }

    private void OnDeath()
    {
        DeathTask().Forget();
    }

    private void Fall()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.AddTorque(new Vector3(0, 5000 * _rigidbody.mass, 0));
    }

    private void Boom()
    {
        var boom = Instantiate(_explosion, transform);
        Renderer renderer = boom.GetComponent<Renderer>();
        var material = renderer.material;
        material.color = Color.cyan;
        boom.transform.localScale *= 10;
        Destroy(boom, 2);
    }
}
