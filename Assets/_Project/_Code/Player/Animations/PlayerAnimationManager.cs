using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField] private float _sprintMultiplier = 2;

    private PlayerInputReader _input;
    private Animator _animator;

    void Start()
    {
        _input = GetComponent<PlayerInputReader>();
        _animator = GameObject.Find("Bugslayer").GetComponent<Animator>();

        if ( _animator == null )
        {
            Debug.LogError("Bugslayer animator is missing");
        }
    }

    void Update()
    {
        _animator.SetFloat("Move", _input.move.magnitude);

        var speedMultiplier = _input.move.magnitude * Mathf.Sign(_input.move.y);

        if (_input.sprint > 0)
        {
            speedMultiplier *= _sprintMultiplier;
        }

        _animator.SetFloat("SpeedMultiplier", speedMultiplier);
    }
}
