using Cysharp.Threading.Tasks;
using UnityEngine;

public class Bobblehead : MonoBehaviour
{
    [SerializeField] private float force = 0.15f;
    [SerializeField] private float torque = 0.15f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        ApplyForce();
        ApplyTorque();
    }

    public void ApplyForce()
    {
        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        rb.AddForce(randomDirection * force, ForceMode.Impulse);
    }

    public void ApplyTorque()
    {
        Vector3 randomRotationAxis = Random.insideUnitSphere.normalized;
        rb.AddTorque(randomRotationAxis * torque, ForceMode.Impulse);
    }
}
