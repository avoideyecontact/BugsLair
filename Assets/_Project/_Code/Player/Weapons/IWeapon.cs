
public interface IWeapon
{
    public string Name { get; }
    public float Damage { get; }
    public float DamageRate { get; }
    public void Use();
}
