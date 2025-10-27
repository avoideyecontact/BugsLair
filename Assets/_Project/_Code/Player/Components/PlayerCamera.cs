using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSens = 2.5f;

    private PlayerInputReader _input;
    [SerializeField] private Transform _cameraTransform;
    private float _xRotation;
    private float _yRotation;

    public Transform CameraTransform => _cameraTransform;

    void Start()
    {
        _input = GetComponent<PlayerInputReader>();
    }

    void Update()
    {
        Look();
    }

    private void Look()
    {
        _yRotation += _input.look.x * mouseSens;
        _xRotation -= _input.look.y * mouseSens;
        _xRotation = Mathf.Clamp(_xRotation, -70f, 30f);
        transform.rotation = Quaternion.Euler(0, _yRotation, 0);
        _cameraTransform.transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
    }
}
