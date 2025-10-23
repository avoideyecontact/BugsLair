using UnityEngine;
using VContainer;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Renderer _beamRenderer;
    [SerializeField] private float _distance = 100f;

    private PlayerContext _playerContext;
    private bool _isActive;

    [Inject]
    public void Construct(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    private void Start()
    {
        SetActive(false);
    }

    public void SetActive(bool active)
    {
        _isActive = active;
        _lineRenderer.enabled = active;
        _beamRenderer.enabled = active;
    }

    private void Update()
    {
        if (!_isActive) return;

        UpdateLaserPosition();
    }

    private void UpdateLaserPosition()
    {
        Vector3 start = transform.position;
        Vector3 end = start + transform.forward * _distance;

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = _playerContext.MainCamera.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, _distance))
        {
            end = hit.point;

            OnLaserHit(hit);
        }

        _lineRenderer.SetPositions(new Vector3[] { start, end });
    }

    private void OnLaserHit(RaycastHit hit)
    {
        // later
    }
}
