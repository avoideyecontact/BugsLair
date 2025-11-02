using UnityEngine;
using UnityEngine.AI;

public class CockroachAI : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _stopDistance = 4f;

    private NavMeshAgent _agent;
    private bool _isChasing = true;

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
}
