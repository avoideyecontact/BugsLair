using System.Linq;
using UnityEngine;
using VContainer;

public class PlayerWeaponary : MonoBehaviour
{
    [SerializeField] private GameObject[] _weaponsGameObjects;
    [SerializeField] private LayerMask _dropLayer;

    [SerializeField] private SoundData _minigunPickupSound;
    [SerializeField] private SoundData _lasergunPickupSound;
    [SerializeField] private SoundData[] _ammoPickupSounds;

    private PlayerContext _playerContext;
    private IEventBus _eventBus;
    private IWeapon _currentWeapon;

    public WeaponType CurrentWeaponType => _currentWeapon.WeaponType;

    [Inject]
    public void Construct(PlayerContext playerContext, IEventBus eventBus)
    {
        _playerContext = playerContext;
        _eventBus = eventBus;
    }

    private void Start()
    {
        SubscribeToInput();
        DeselectAllWeapons();
        MakeWeaponAvailable(WeaponType.Saw);
        MakeWeaponAvailable(WeaponType.Minigun);
        MakeWeaponAvailable(WeaponType.Laser);
        SelectFirstWeapon();
    }

    private void OnDestroy() => UnsubscribeFromInput();

    private void SubscribeToInput()
    {
        _playerContext.Input.InteractStarted += PickupWeapon;
        _playerContext.Input.NextStarted += SelectNextWeapon;
        _playerContext.Input.PreviousStarted += SelectPreviousWeapon;
    }

    private void UnsubscribeFromInput()
    {
        _playerContext.Input.InteractStarted -= PickupWeapon;
        _playerContext.Input.NextStarted -= SelectNextWeapon;
        _playerContext.Input.PreviousStarted -= SelectPreviousWeapon;
    }

    private void DeselectAllWeapons()
    {
        foreach (var weaponGameObject in _weaponsGameObjects)
        {
            weaponGameObject.SetActive(false);
            weaponGameObject.GetComponent<IWeapon>().Selected = false;
        }
        _currentWeapon = null;
    }

    private void SelectWeapon(WeaponType weaponType)
    {
        foreach (var weaponGameObject in _weaponsGameObjects)
        {
            var weapon = weaponGameObject.GetComponent<IWeapon>();

            if (weaponType == weapon.WeaponType && weapon.Available)
            {
                DeselectAllWeapons();
                weaponGameObject.SetActive(true);
                weapon.Selected = true;
                _currentWeapon = weapon;
                OnWeaponChanged();
                return;
            }
        }
    }

    private void OnWeaponChanged()
    {
        _eventBus.Publish(new WeaponChanged
        {
            WeaponType = _currentWeapon.WeaponType,
            Ammo = _currentWeapon.Ammo
        });
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

    private void MakeWeaponAvailable(WeaponType weaponType)
    {
        foreach (var weaponGameObject in _weaponsGameObjects)
        {
            var weapon = weaponGameObject.GetComponent<IWeapon>();
            if (weapon.WeaponType == weaponType)
            {
                weapon.Available = true;
            }
        }
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

        Destroy(hit.transform.gameObject);

        MakeWeaponAvailable((WeaponType)weaponType);
        SelectWeapon((WeaponType)weaponType);


        SoundData pickupSound = _ammoPickupSounds[0];

        switch ((WeaponType)weaponType)
        {
            case WeaponType.Saw:
                break;
            case WeaponType.Minigun:
                pickupSound = _minigunPickupSound;
                break;
            case WeaponType.Laser:
                pickupSound = _lasergunPickupSound;
                break;
            default:
                break;
        }

        SoundManager.Instance.CreateSoundBuilder()
                .WithRandomPitch()
                .WithPosition(transform.position)
                .Play(pickupSound);
    }

    public void AddAmmo(WeaponType weaponType, int value)
    {
        foreach (var weaponGameObject in _weaponsGameObjects)
        {
            var weapon = weaponGameObject.GetComponent<IWeapon>();
            if (weapon.WeaponType == weaponType)
            {
                weapon.AddAmmo(value);
            }
        }

        SoundManager.Instance.CreateSoundBuilder()
                .WithRandomPitch()
                .WithPosition(transform.position)
                .Play(_ammoPickupSounds[Random.Range(0, _ammoPickupSounds.Length)]);
    }

    private void Update()
    {
        if (_playerContext.Input.attack > 0.1 && _currentWeapon != null)
        {
            _currentWeapon.Use();
            _eventBus.Publish(new PlayerIsUsingWeapon
            {
                weaponType = _currentWeapon.WeaponType,
            });
        }
    }
}
