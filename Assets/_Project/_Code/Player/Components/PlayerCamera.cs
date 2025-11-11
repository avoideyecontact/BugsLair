using UnityEngine;
using VContainer;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private PlayerInputReader _input;

    private float _mouseSens = 0.1f;
    private float _xRotation;
    private float _yRotation;

    private IEventBus _eventBus;

    public Transform CameraTransform => _cameraTransform;

    [Inject]
    public void Construct(IEventBus eventBus, ISettingsManager settingsManager)
    {
        _eventBus = eventBus;
        SubscribeToEventBus();

        _mouseSens = settingsManager.Settings.sensitivity;
    }

    private void OnDestroy() => UnsubscribeFromEventBus();

    private void SubscribeToEventBus()
    {
        _eventBus.Subscribe<CameraSensitivityChanged>(OnSensetivityChanged);
    }

    private void UnsubscribeFromEventBus()
    {
        _eventBus.Unsubscribe<CameraSensitivityChanged>(OnSensetivityChanged);
    }

    void Update()
    {
        Look();
    }

    private void Look()
    {
        _yRotation += _input.look.x * _mouseSens;
        _xRotation -= _input.look.y * _mouseSens;
        _xRotation = Mathf.Clamp(_xRotation, -70f, 30f);
        transform.rotation = Quaternion.Euler(0, _yRotation, 0);
        _cameraTransform.transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
    }

    private void OnSensetivityChanged(CameraSensitivityChanged evt)
    {
        _mouseSens = evt.Sensitivity;
    }
}
