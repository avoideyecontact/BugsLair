using UnityEngine;

public class MinigunAnimationManager : MonoBehaviour
{
    private PlayerInputReader _input;
    private Transform _cameraTransform;
    private Animator _animator1;
    private Animator _animator2;

    void Start()
    {
        _input = transform.root.GetComponent<PlayerInputReader>();
        _cameraTransform = transform.root.Find("CameraPos");
        _animator1 = GameObject.Find("Minigun1").GetComponent<Animator>();
        _animator2 = GameObject.Find("Minigun2").GetComponent<Animator>();

        if (_input == null)
            Debug.LogError("Miniguns cant find PlayerInputReader");

        if (_cameraTransform == null)
            Debug.LogError("CameraPos is missing");

        if (_animator1 == null || _animator2 == null)
            Debug.LogError("Miniguns animator is missing");
    }

    private void FixedUpdate()
    {
        _animator1.SetFloat("Attack", _input.attack);
        _animator2.SetFloat("Attack", _input.attack);
    }

    void Update()
    {
        transform.rotation = _cameraTransform.rotation;
    }
}
