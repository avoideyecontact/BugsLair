using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform _cameraTransform;
    private PlayerInputReader _input;

    public float mouseSens = 2.5f;

    private float _xRotation;
    private float _yRotation;

    void Start()
    {
        _input = GetComponent<PlayerInputReader>();
        _cameraTransform = transform.Find("CameraPos");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();
    }

    private void Look()
    {
        _yRotation += _input.look.x * mouseSens * Time.deltaTime;
        _xRotation -= _input.look.y * mouseSens * Time.deltaTime;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _cameraTransform.transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);

        transform.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
}
