using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class CockroachAI : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _stopDistance = 4f;

    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _damageRate = 1f;

    private NavMeshAgent _agent;
    private bool _isChasing = true;
    private bool _isDamageCooldown;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = _stopDistance;

        if (_target == null)
            _target = GameObject.FindGameObjectWithTag("Player").transform;            

        SetTarget(_target);
    }

    void FixedUpdate()
    {
        if (_target == null) return;

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
            }
        }
        else
        {
            if (currentDistance > _stopDistance)
            {
                _isChasing = true;
                _agent.isStopped = false;
                _agent.SetDestination(_target.position);
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
        _isChasing = true;
        _agent.isStopped = false;
        _agent.SetDestination(_target.position);
    }

    public void Disable()
    {
        _agent.enabled = false;
        enabled = false;
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
}
