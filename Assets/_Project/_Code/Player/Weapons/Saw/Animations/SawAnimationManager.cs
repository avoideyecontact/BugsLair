using UnityEngine;

public class SawAnimationManager : MonoBehaviour
{
    private PlayerInputReader _input;
    private Animator _animator1;
    private Animator _animator2;
    private Animator _bladeAnimator1;
    private Animator _bladeAnimator2;

    private void Start()
    {
        _input = transform.root.GetComponent<PlayerInputReader>();
        _animator1 = GameObject.Find("Saw1").GetComponent<Animator>();
        _animator2 = GameObject.Find("Saw2").GetComponent<Animator>();
        _bladeAnimator1 = GameObject.Find("Blade1").GetComponent<Animator>();
        _bladeAnimator2 = GameObject.Find("Blade2").GetComponent<Animator>();

        if (_animator1 == null || _animator2 == null)
        {
            Debug.LogError("Saw animator is missing");
        }
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
}
