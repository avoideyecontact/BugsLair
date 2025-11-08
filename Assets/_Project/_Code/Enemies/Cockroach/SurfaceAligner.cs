using UnityEngine;
using UnityEngine.AI;

public class SurfaceAligner : MonoBehaviour
{
    [Header("References")]
    public Transform childObject;
    public NavMeshAgent agent;

    [Header("Surface Alignment")]
    public float raycastDistance = 2f;
    public float alignmentSpeed = 5f;
    public LayerMask surfaceLayer = ~0;
    public Vector3 raycastOffset = Vector3.zero;

    [Header("Rotation Settings")]
    public bool preserveForwardDirection = true;

    private Quaternion targetSurfaceRotation;
    private Quaternion lastValidRotation;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        targetSurfaceRotation = childObject.rotation;
        lastValidRotation = childObject.rotation;
    }

    void FixedUpdate()
    {
        if (childObject == null || agent == null) return;

        UpdateSurfaceRotation();
        ApplyRotationToModel();
    }

    void UpdateSurfaceRotation()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + raycastOffset;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, raycastDistance, surfaceLayer))
        {
            Vector3 surfaceNormal = hit.normal;

            if (preserveForwardDirection)
            {
                Vector3 forward = agent.velocity.normalized;
                if (forward.magnitude < 0.1f)
                    forward = childObject.forward;

                targetSurfaceRotation = Quaternion.LookRotation(forward, surfaceNormal);
            }
            else
            {
                targetSurfaceRotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
            }

            lastValidRotation = targetSurfaceRotation;
        }
        else
        {
            targetSurfaceRotation = lastValidRotation;
        }
    }

    void ApplyRotationToModel()
    {
        childObject.rotation = Quaternion.Slerp(
            childObject.rotation,
            targetSurfaceRotation,
            alignmentSpeed * Time.deltaTime
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (childObject == null) return;

        Gizmos.color = Color.blue;
        Vector3 rayOrigin = transform.position + raycastOffset;
        Gizmos.DrawRay(rayOrigin, Vector3.down * raycastDistance);

        if (agent != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, agent.velocity.normalized * 2f);
        }
    }
}
