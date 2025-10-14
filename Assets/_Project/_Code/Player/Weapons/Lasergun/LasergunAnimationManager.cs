using UnityEngine;

public class LasergunAnimationManager : MonoBehaviour
{
    private PlayerInputReader _input;
    private Transform _cameraTransform;
    private Animator _animator1;
    private Animator _animator2;

    private void Start()
    {
        _input = transform.root.GetComponent<PlayerInputReader>();
        _cameraTransform = transform.root.Find("CameraPos");
        _animator1 = GameObject.Find("Lasergun1").GetComponent<Animator>();
        _animator2 = GameObject.Find("Lasergun2").GetComponent<Animator>();

        if (_input == null)
            Debug.LogError("Laserguns cant find PlayerInputReader");

        if (_animator1 == null || _animator2 == null)
        {
            Debug.LogError("Lasergun animator is missing");
        }

        if (_cameraTransform == null)
            Debug.LogError("CameraPos is missing");
    }

    private void FixedUpdate()
    {
        _animator1.SetFloat("Attack", _input.attack);
        _animator2.SetFloat("Attack", _input.attack);
    }

    private void Update()
    {
        transform.rotation = _cameraTransform.rotation;
    }
}
