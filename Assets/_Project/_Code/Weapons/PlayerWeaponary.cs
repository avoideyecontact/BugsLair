using UnityEngine;

public class PlayerWeaponary : MonoBehaviour
{
    private PlayerInputReader _input;

    private bool hasLasergun = false;
    private bool hasShotgun = false;
    private bool hasMinigun = false;

    private int _currentGunID = 0;

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
        hasLasergun = playerData.hasLaserGun;
        hasShotgun = playerData.hasShotgun;
        hasMinigun = playerData.hasMinigun;
    }

    public void SwitchGun(float value)
    {
        _currentGunID += (int)value;

        _currentGunID = (_currentGunID > 3) ? 0 : _currentGunID;
        _currentGunID = (_currentGunID < 0) ? 3 : _currentGunID;

        Debug.Log(_currentGunID);
    }
}
