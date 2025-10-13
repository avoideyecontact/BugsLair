using UnityEngine;

public class DragonflyCutsceneAnimationManager : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        PlayAnimationAtRandomTime("Turbulence", 0, Random.value);
        PlayAnimationAtRandomTime("Fly", 1, Random.value);
        
    }

    private void PlayAnimationAtRandomTime(string stateName, int layer, float normalizedTime)
    {
        _animator.Play(stateName, layer, normalizedTime);
    }
}
