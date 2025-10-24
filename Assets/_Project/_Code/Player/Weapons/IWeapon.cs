
public interface IWeapon
{
    public WeaponType WeaponType { get; }
    public float Damage { get; }
    public float DamageRate { get; }
    public int Ammo { get; }
    public bool Available { get; set; }
    public bool Selected { get; set; }
    public void Use();
}
