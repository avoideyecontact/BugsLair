using UnityEngine;

public class MinigunAnimationManager : MonoBehaviour
{
    private PlayerInputReader _input;
    private Animator _animator1;
    private Animator _animator2;

    void Start()
    {
        _input = transform.root.GetComponent<PlayerInputReader>();
        _animator1 = GameObject.Find("Minigun1").GetComponent<Animator>();
        _animator2 = GameObject.Find("Minigun2").GetComponent<Animator>();

        if (_input == null)
            Debug.LogError("Miniguns cant find PlayerInputReader");

        if (_animator1 == null || _animator2 == null)
            Debug.LogError("Miniguns animator is missing");
    }

    void FixedUpdate()
    {
        _animator1.SetFloat("Attack", _input.attack);
        _animator2.SetFloat("Attack", _input.attack);
    }
}
