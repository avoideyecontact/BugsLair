using UnityEngine;

public abstract class AbilityBase : MonoBehaviour, IAbility
{
    [SerializeField] protected AbilityType _abilityType;
    private bool _active = false;

    public AbilityType AbilityType => _abilityType;
    public bool IsActive => _active;

    public virtual void Activate()
    {
        _active = true;
    }
    public virtual void Deactivate()
    {
        _active = false;
    }
}
