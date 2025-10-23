
public class AmmoSystem
{
    private int _currentAmmo;
    private readonly int _maxAmmo;

    public int CurrentAmmo => _currentAmmo;
    public bool HasAmmo => _currentAmmo > 0;

    public AmmoSystem(int maxAmmo)
    {
        _maxAmmo = maxAmmo;
        _currentAmmo = maxAmmo;
    }

    public void SpendAmmo()
    {
        _currentAmmo--;
    }

    public void Reload()
    {
        _currentAmmo = _maxAmmo;
    }
}
