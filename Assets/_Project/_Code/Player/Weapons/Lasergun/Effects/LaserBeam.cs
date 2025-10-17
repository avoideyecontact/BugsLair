using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _distance = 100f;

    private void Start()
    {
        if (_lineRenderer == null)
            _lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    private void Update()
    {
        Vector3 start = transform.position;
        Vector3 end = start + transform.forward * _distance;

        Camera camera = Camera.main;
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = camera.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _distance))
        {
            end = hit.point;
        }

        _lineRenderer.SetPositions(new Vector3[] { start, end });
    }
}
