using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponary : MonoBehaviour
{
    private PlayerInputReader _input;

    private int _currentGunID = 0;
    private List<IWeapon> _weapons;

    [SerializeField] private GameObject _saw;
    [SerializeField] private GameObject _lasergun;
    [SerializeField] private GameObject _minigun;

    private void Start()
    {
        _input = transform.root.GetComponent<PlayerInputReader>();

        if (_input == null)
            Debug.LogError("PlayerWeaponary: PlayerInput is missing");

        _input.NextStarted += NextGun;
        _input.PreviousStarted += PreviousGun;

        ShowGun();
    }

    private void OnDestroy()
    {
        _input.NextStarted -= NextGun;
        _input.PreviousStarted -= PreviousGun;
    }

    public void Initialize(PlayerData playerData)
    {
        _weapons = new List<IWeapon>();

        if (playerData.hasSaw)
        {
            var saw = _saw.GetComponent<WeaponSaw>();
            _weapons.Add(saw);
        }

        if (playerData.hasLaserGun)
        {
            var lasergun = _lasergun.GetComponent<WeaponLasergun>();
            _weapons.Add(lasergun);
        }

        if (playerData.hasShotgun)
        {
        }

        if (playerData.hasMinigun)
        {
            var minigun = _minigun.GetComponent<WeaponMinigun>();
            _weapons.Add(minigun);
        }
    }

    public void NextGun()
    {
        _currentGunID += 1;

        _currentGunID = (_currentGunID >= _weapons.Count) ? 0 : _currentGunID;
        _currentGunID = (_currentGunID < 0) ? _weapons.Count - 1 : _currentGunID;

        ShowGun();
    }

    public void PreviousGun()
    {
        _currentGunID -= 1;

        _currentGunID = (_currentGunID >= _weapons.Count) ? 0 : _currentGunID;
        _currentGunID = (_currentGunID < 0) ? _weapons.Count - 1 : _currentGunID;

        ShowGun();
    }

    private void ShowGun()
    {
        string name = _weapons[_currentGunID].Name;

        if (name == "Saw")
        {
            _saw.SetActive(true);
            _lasergun.SetActive(false);
            _minigun.SetActive(false);
        }
        if (name == "Lasergun")
        {
            _saw.SetActive(false);
            _lasergun.SetActive(true);
            _minigun.SetActive(false);
        }
        if (name == "Minigun")
        {
            _saw.SetActive(false);
            _lasergun.SetActive(false);
            _minigun.SetActive(true);
        }
    }

    private void Update()
    {
        if (_input.attack == 1)
            _weapons[_currentGunID].Use();
    }
}
