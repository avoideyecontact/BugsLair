using UnityEngine;

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

    public bool TrySpendAmmo()
    {
        if (_currentAmmo <= 0)
            return false;

        _currentAmmo--;
        return true;
    }

    public void Reload(int amount = -1)
    {
        _currentAmmo = amount == -1 ? _maxAmmo : Mathf.Min(_currentAmmo + amount, _maxAmmo);
    }
}
