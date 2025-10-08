using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponary : MonoBehaviour
{
    private PlayerInputReader _input;

    private int _currentGunID = 0;
    private List<IWeapon> weapons;

    private void Start()
    {
        _input = GetComponent<PlayerInputReader>();

        _input.NextStarted += SwitchGun;
        _input.PreviousStarted += SwitchGun;
    }

    private void OnDestroy()
    {
        _input.NextStarted -= SwitchGun;
        _input.PreviousStarted -= SwitchGun;
    }

    public void Initialize(PlayerData playerData)
    {
        weapons = new List<IWeapon>();

        if (playerData.hasSaw)
        {
            var saw = GetComponent<WeaponSaw>();
            weapons.Add(saw);
        }

        if (playerData.hasLaserGun)
        {
            var lasergun = GetComponent<WeaponLasergun>();
            weapons.Add(lasergun);
        }

        if (playerData.hasShotgun)
        {
        }

        if (playerData.hasMinigun)
        {
        }
    }

    public void SwitchGun(float value)
    {
        _currentGunID += (int)value;

        _currentGunID = (_currentGunID >= weapons.Count) ? 0 : _currentGunID;
        _currentGunID = (_currentGunID < 0) ? weapons.Count - 1 : _currentGunID;
    }

    private void Update()
    {
        if (_input.attack == 1)
            weapons[_currentGunID].Use();
    }
}
