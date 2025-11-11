using UnityEngine;

public class Roper : MonoBehaviour
{
    [SerializeField] private GameObject _ropeObject;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    private void Start()
    {
        CreateRope();
    }

    public void CreateRope()
    {
        var rope = Instantiate(_ropeObject);

        if (pointA == null || pointB == null)
        {
            Debug.LogWarning("PointA / PointB is not assigned!");
            return;
        }

        rope.transform.position = (pointA.position + pointB.position) / 2f;

        Vector3 scale = rope.transform.localScale;
        scale.z = Vector3.Distance(pointA.position, pointB.position) / 30; // because length of the rope is 30
        rope.transform.localScale = scale;

        Vector3 direction = pointB.position - pointA.position;
        if (direction != Vector3.zero)
        {
            rope.transform.rotation = Quaternion.LookRotation(direction);
            rope.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}
