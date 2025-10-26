
public interface IAbility
{
    AbilityType AbilityType { get; }
    bool IsActive { get; }
    void Activate();
    void Deactivate();
}
