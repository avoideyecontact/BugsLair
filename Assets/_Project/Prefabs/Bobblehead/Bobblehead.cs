using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class Bobblehead : MonoBehaviour
{
    [SerializeField] private float force = 0.15f;
    [SerializeField] private float torque = 0.15f;
    [SerializeField] private float interval = 1.5f;

    private Rigidbody _rb;
    private CancellationTokenSource _cts;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cts = new CancellationTokenSource();
        ApplyForceContinuous(_cts.Token).Forget();
        ApplyTorqueContinuous(_cts.Token).Forget();
    }

    private void ApplyForce()
    {
        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        _rb.AddForce(randomDirection * force, ForceMode.Impulse);
    }

    private void ApplyTorque()
    {
        //Vector3 randomRotationAxis = Random.insideUnitSphere.normalized;
        Vector3 rotationAxis = new Vector3(0, 0, 1);
        _rb.AddTorque(rotationAxis * torque, ForceMode.Impulse);
    }

    public async UniTask ApplyForceContinuous(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            ApplyForce();
            await UniTask.WaitForSeconds(interval);
        }
    }

    public async UniTask ApplyTorqueContinuous(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            ApplyTorque();
            await UniTask.WaitForSeconds(interval);
        }
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
