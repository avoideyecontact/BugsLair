using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Spider : MonoBehaviour
{
    [Header("AI")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private bool _isSleeping;
    [SerializeField] private Transform _target;
    [SerializeField] private float _stopDistance = 4f;
    private bool _isChasing = true;

    [Header("Damage")]
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _damageRate = 1f;
    private bool _isDamageCooldown;

    [SerializeField] private Animator _animator;

    private void Start()
    {
        if (_target == null)
            FindPlayer();

        SetTarget(_target);


        if (_isSleeping)
            Deactivate();
        else
            Activate();
    }

    void FixedUpdate()
    {
        if (_target == null) return;
        if (_isSleeping) return;

        float currentDistance = Vector3.Distance(transform.position, _target.position);

        if (!_isDamageCooldown && currentDistance <= _stopDistance)
        {
            DealDamage();
        }

        if (_isChasing)
        {
            _agent.SetDestination(_target.position);

            if (currentDistance <= _stopDistance)
            {
                _isChasing = false;
                _agent.isStopped = true;
                _animator.SetTrigger("Bite");
            }
        }
        else
        {
            if (currentDistance > _stopDistance)
            {
                _isChasing = true;
                _agent.isStopped = false;
                _agent.SetDestination(_target.position);
                _animator.SetTrigger("Run");
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
        _isChasing = true;
        _agent.isStopped = false;
        _agent.SetDestination(_target.position);
        _animator.SetTrigger("Run");
    }

    private void DealDamage()
    {
        _target.GetComponent<PlayerHealth>().DealDamage(_damage);
        DamageCooldownTimer().Forget();
    }

    private async UniTask DamageCooldownTimer()
    {
        var ct = this.GetCancellationTokenOnDestroy();
        _isDamageCooldown = true;
        await UniTask.WaitForSeconds(_damageRate, cancellationToken: ct);
        _isDamageCooldown = false;
    }

    public void Activate()
    {
        _isSleeping = false;
        _agent.isStopped = false;
    }

    public void Deactivate()
    {
        _isSleeping = true;
        _agent.isStopped = true;
        _animator.SetTrigger("Idle");
    }

    private void FindPlayer()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
