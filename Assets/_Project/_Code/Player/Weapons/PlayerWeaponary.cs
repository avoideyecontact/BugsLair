using System.Linq;
using UnityEngine;
using VContainer;

public class PlayerWeaponary : MonoBehaviour
{
    [SerializeField] private GameObject[] _weaponsGameObjects;
    [SerializeField] private LayerMask _dropLayer;

    private PlayerContext _playerContext;
    private IWeapon _currentWeapon;

    [Inject]
    public void Construct(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    private void Start()
    {
        _playerContext.Input.NextStarted += SelectNextWeapon;
        _playerContext.Input.PreviousStarted += SelectPreviousWeapon;

        DeselectAllWeapons();
        SelectFirstWeapon();
    }

    private void OnDestroy()
    {
        _playerContext.Input.NextStarted -= SelectNextWeapon;
        _playerContext.Input.PreviousStarted -= SelectPreviousWeapon;
    }

    private void DeselectAllWeapons()
    {
        foreach (var weaponGameObject in _weaponsGameObjects)
        {
            weaponGameObject.SetActive(false);
            var weapon = weaponGameObject.GetComponent<IWeapon>();
            weapon.Selected = false;
            _currentWeapon = null;
        }
    }

    private void SelectWeapon(WeaponType weaponType)
    {
        DeselectAllWeapons();
        foreach (var weaponGameObject in _weaponsGameObjects)
        {
            var weapon = weaponGameObject.GetComponent<IWeapon>();

            if (weaponType == weapon.WeaponType && weapon.Available)
            {
                weaponGameObject.SetActive(true);
                weapon.Selected = true;
                _currentWeapon = weapon;
                return;
            }
        }
    }

    private void SelectFirstWeapon()
    {
        foreach (var weaponGameObject in _weaponsGameObjects)
        {
            var weapon = weaponGameObject.GetComponent<IWeapon>();
            if (weapon.Available)
            {
                SelectWeapon(weapon.WeaponType);
                return;
            }
        }
    }

    private void SelectNextWeapon()
    {
        var availableWeapons = _weaponsGameObjects.Where(w => w.GetComponent<IWeapon>().Available).ToArray();

        if (availableWeapons.Length < 2)
            return;

        for (int i = 0; i < availableWeapons.Length; i++)
        {
            if (availableWeapons[i].GetComponent<IWeapon>().Selected)
            {
                var nextIndex = (i + 1) % availableWeapons.Length;
                var nextWeapon = availableWeapons[nextIndex].GetComponent<IWeapon>();
                var nextWeaponType = nextWeapon.WeaponType;
                SelectWeapon(nextWeaponType);
                return;
            }
        }
        SelectFirstWeapon();
    }

    private void SelectPreviousWeapon()
    {
        var availableWeapons = _weaponsGameObjects.Where(w => w.GetComponent<IWeapon>().Available).ToArray();

        if (availableWeapons.Length < 2)
            return;

        for (int i = 0; i < availableWeapons.Length; i++)
        {
            if (availableWeapons[i].GetComponent<IWeapon>().Selected)
            {
                var nextIndex = (i - 1) % availableWeapons.Length;
                if (nextIndex < 0)
                    nextIndex = availableWeapons.Length - 1;
                var nextWeapon = availableWeapons[nextIndex].GetComponent<IWeapon>();
                var nextWeaponType = nextWeapon.WeaponType;
                SelectWeapon(nextWeaponType);
                return;
            }
        }
        SelectFirstWeapon();
    }

    private void PickupWeapon()
    {
        WeaponType? weaponType = null;
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = _playerContext.MainCamera.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _playerContext.InteractionDistance, _dropLayer))
        {
            weaponType = hit.transform.GetComponent<WeaponDrop>()?.GetWeaponType;
        }

        if (weaponType == null)
            return;

        foreach (var weaponGameObject in _weaponsGameObjects)
        {
            var weapon = weaponGameObject.GetComponent<IWeapon>();
            if (weapon.WeaponType == weaponType)
            {
                weapon.Available = true;
                SelectWeapon(weapon.WeaponType);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickupWeapon();
        }

        if (_playerContext.Input.attack > 0.1 && _currentWeapon != null)
        {
            _currentWeapon.Use();
        }
    }
}
