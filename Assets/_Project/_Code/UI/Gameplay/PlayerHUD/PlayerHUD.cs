using TMPro;
using UnityEngine;
using VContainer;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private TMP_Text _weaponText;

    private PlayerContext _playerContext;
    private IEventBus _eventBus;

    [Inject]
    public void Construct(PlayerContext playerContext, IEventBus eventBus)
    {
        _playerContext = playerContext;
        _eventBus = eventBus;
        _eventBus.Subscribe<AmmoChanged>(OnAmmoChanged);
        _eventBus.Subscribe<WeaponChanged>(OnWeaponChanged);
    }

    private void OnAmmoChanged(AmmoChanged evt)
    {
        if (_playerContext.Weaponary.CurrentWeaponType != evt.WeaponType)
            return;

        _ammoText.text = evt.Ammo.ToString();
    }

    private void OnWeaponChanged(WeaponChanged evt)
    {
        _weaponText.text = evt.WeaponType.ToString();

        if (evt.Ammo == -1)
            _ammoText.text = "\u221E"; // infinity symbol
        else
            _ammoText.text = evt.Ammo.ToString();
    }
}
