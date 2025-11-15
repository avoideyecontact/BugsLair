using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Spider : MonoBehaviour
{
    [Header("AI")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _target;
    [SerializeField] private float _stopDistance = 4f;
    private bool _isChasing = true;

    [Header("Damage")]
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _damageRate = 1f;
    private bool _isDamageCooldown;

    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _walkSound;
    [SerializeField] private AudioSource _biteSound;

    //private void Awake()
    //{
    //    if (_isSleeping)
    //        Deactivate();
    //}

    private void Start()
    {
        if (_target == null)
            FindPlayer();

        //SetTarget(_target);

        Activate();
    }

    void FixedUpdate()
    {
        if (_target == null) return;

        if (Vector3.Distance(transform.position, _target.position) < 100)
            Activate();
        else Deactivate();

        float currentDistance = Vector3.Distance(transform.position, _target.position);

        if (!_isDamageCooldown && currentDistance <= _stopDistance)
        {
            DealDamage();
            _biteSound.volume = 0.15f;
            _biteSound.pitch = Random.Range(0.9f, 1.1f);
            _biteSound.Play();
        }

        if (_isChasing)
        {
            _agent.SetDestination(_target.position);

            if (currentDistance <= _stopDistance)
            {
                _isChasing = false;
                _agent.isStopped = true;
                _animator.SetTrigger("Bite");
                _walkSound.Stop();
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
                if (!_walkSound.isPlaying)
                    _walkSound.Play();
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
        if (!_walkSound.isPlaying)
            _walkSound.Play();
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
        //_agent.enabled = true;
        FindPlayer();
        SetTarget(_target);
        _agent.isStopped = false;
        _animator.SetTrigger("Run");
        if (!_walkSound.isPlaying)
            _walkSound.Play();
    }

    public void Deactivate()
    {
        //_isSleeping = true;
        _agent.isStopped = true;
        _animator.SetTrigger("Idle");
        _walkSound.Stop();
        //_agent.enabled = false;
    }

    private void FindPlayer()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
