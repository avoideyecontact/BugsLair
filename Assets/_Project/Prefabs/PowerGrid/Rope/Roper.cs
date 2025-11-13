using UnityEngine;

public class Roper : MonoBehaviour
{
    [SerializeField] private GameObject _ropeObject;
    [SerializeField] private RopeConnector _ropeConnectorA;
    [SerializeField] private RopeConnector _ropeConnectorB;

    private void Start()
    {
        if (_ropeConnectorB == null)
            FindRopeConnector();

        CreateRopeForRopeConnectors();
    }

    private void FindRopeConnector()
    {
        var ropeConnectors = GameObject.FindGameObjectsWithTag("RopeConnection");

        if (ropeConnectors.Length == 0)
        {
            Debug.LogError("ropeConnector is missing");
            return;
        }

        var desiredConnector = ropeConnectors[0];

        float min = Mathf.Infinity;
        foreach (var connector in ropeConnectors)
        {
            var distance = Vector3.Distance(transform.position, connector.transform.position);
            if (distance < min)
            {
                min = distance;
                desiredConnector = connector;
            }
        }

        _ropeConnectorB = desiredConnector.GetComponent<RopeConnector>();
    }

    private void CreateRopeForRopeConnectors()
    {
        if (_ropeConnectorA == null || _ropeConnectorB == null)
        {
            Debug.LogError("ropeConnector is missing");
            return;
        }

        if (_ropeConnectorA.Ropes.Length != _ropeConnectorB.Ropes.Length)
        {
            Debug.LogError("ropeConnectors length is not equal");
            return;
        }

        for (int i = 0; i < _ropeConnectorA.Ropes.Length; i++)
        {
            CreateRope(_ropeConnectorA.Ropes[i], _ropeConnectorB.Ropes[i]);
        }
    }

    public void CreateRope(Transform ropeStart, Transform ropeEnd)
    {
        var rope = Instantiate(_ropeObject);

        rope.transform.position = (ropeStart.position + ropeEnd.position) / 2f;

        Vector3 scale = rope.transform.localScale;
        scale.z = Vector3.Distance(ropeStart.position, ropeEnd.position) / 30; // because length of the rope is 30
        rope.transform.localScale = scale;

        Vector3 direction = ropeEnd.position - ropeStart.position;
        if (direction != Vector3.zero)
        {
            rope.transform.rotation = Quaternion.LookRotation(direction);
            rope.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}
