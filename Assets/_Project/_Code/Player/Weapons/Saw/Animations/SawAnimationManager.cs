using UnityEngine;

public class SawAnimationManager : MonoBehaviour
{
    private PlayerInputReader _input;
    private Transform _cameraTransform;
    private Animator _animator1;
    private Animator _animator2;
    private Animator _bladeAnimator1;
    private Animator _bladeAnimator2;

    private void Start()
    {
        _input = transform.root.GetComponent<PlayerInputReader>();
        _cameraTransform = transform.root.Find("CameraPos");
        _animator1 = GameObject.Find("Saw1").GetComponent<Animator>();
        _animator2 = GameObject.Find("Saw2").GetComponent<Animator>();
        _bladeAnimator1 = GameObject.Find("Blade1").GetComponent<Animator>();
        _bladeAnimator2 = GameObject.Find("Blade2").GetComponent<Animator>();

        if (_input == null)
            Debug.LogError("Saw cant find PlayerInputReader");

        if (_animator1 == null || _animator2 == null)
        {
            Debug.LogError("Saw animator is missing");
        }

        if (_cameraTransform == null)
            Debug.LogError("CameraPos is missing");

        if (_bladeAnimator1 == null || _bladeAnimator2 == null)
        {
            Debug.LogError("Saw Blade animator is missing");
        }
    }

    void FixedUpdate()
    {
        _animator1.SetFloat("Attack", _input.attack);
        _animator2.SetFloat("Attack", _input.attack);
        _bladeAnimator1.SetFloat("Attack", _input.attack);
        _bladeAnimator2.SetFloat("Attack", _input.attack);

        var speedMultiplier = 1;

        if (_input.sprint > 0)
        {
            speedMultiplier = 4;
        }

        _animator1.SetFloat("SpeedMultiplier", speedMultiplier);
        _animator2.SetFloat("SpeedMultiplier", speedMultiplier);
    }

    private void Update()
    {
        transform.rotation = _cameraTransform.rotation;
    }
}
