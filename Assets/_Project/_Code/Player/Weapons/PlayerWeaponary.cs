using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class PlayerWeaponary : MonoBehaviour
{
    [SerializeField] private GameObject _saw;
    [SerializeField] private GameObject _lasergun;
    [SerializeField] private GameObject _minigun;

    private PlayerContext _playerContext;
    private List<IWeapon> _weapons;
    private int _currentGunID = 0;

    [Inject]
    public void Construct(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    private void Start()
    {
        _playerContext.Input.NextStarted += NextGun;
        _playerContext.Input.PreviousStarted += PreviousGun;

        // rewrite
        _weapons = new List<IWeapon>();
        var saw = _saw.GetComponent<WeaponSaw>();
        var lasergun = _lasergun.GetComponent<WeaponLasergun>();
        var minigun = _minigun.GetComponent<WeaponMinigun>();
        _weapons.Add(saw);
        _weapons.Add(lasergun);
        _weapons.Add(minigun);

        ShowGun();
    }

    private void OnDestroy()
    {
        _playerContext.Input.NextStarted -= NextGun;
        _playerContext.Input.PreviousStarted -= PreviousGun;
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
        if (_playerContext.Input.attack == 1)
            _weapons[_currentGunID].Use();
    }
}
