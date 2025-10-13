using Cysharp.Threading.Tasks;
using UnityEngine;

public class Bobblehead : MonoBehaviour
{
    [SerializeField] private float force = 0.15f;
    [SerializeField] private float torque = 0.15f;
    [SerializeField] private float interval = 1.5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        ApplyForceContinuous().Forget();
        ApplyTorqueContinuous().Forget();
    }

    private void ApplyForce()
    {
        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        rb.AddForce(randomDirection * force, ForceMode.Impulse);
    }

    private void ApplyTorque()
    {
        //Vector3 randomRotationAxis = Random.insideUnitSphere.normalized;
        Vector3 rotationAxis = new Vector3(0, 0, 1);
        rb.AddTorque(rotationAxis * torque, ForceMode.Impulse);
    }

    public async UniTask ApplyForceContinuous()
    {
        while (true)
        {
            ApplyForce();
            await UniTask.WaitForSeconds(interval);
        }
    }

    public async UniTask ApplyTorqueContinuous()
    {
        while (true)
        {
            ApplyTorque();
            await UniTask.WaitForSeconds(interval);
        }
    }
}
