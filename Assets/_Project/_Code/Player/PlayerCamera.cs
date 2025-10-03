using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;

    public float mouseSens = 1f;

    private float _xRotation;
    private float _yRotation;
    private PlayerInputReader _input;

    void Start()
    {
        _input = GetComponent<PlayerInputReader>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();
    }

    public void Look()
    {
        _yRotation += _input.look.x * mouseSens * Time.deltaTime;
        _xRotation -= _input.look.y * mouseSens * Time.deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _cameraTransform.transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);

        transform.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
}
