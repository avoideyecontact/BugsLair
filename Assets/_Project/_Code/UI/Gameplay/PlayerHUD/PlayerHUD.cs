using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text _ammoText;
    [SerializeField] private TMP_Text _weaponText;
    [SerializeField] private RawImage _healthBar;

    [Header("Inventory")]
    [SerializeField] private TMP_Text _keyCardsCount;
    [SerializeField] private TMP_Text _gearsCount;

    private PlayerContext _playerContext;
    private IEventBus _eventBus;

    [Inject]
    public void Construct(PlayerContext playerContext, IEventBus eventBus)
    {
        _playerContext = playerContext;
        _eventBus = eventBus;
        SubscribeToEventBus();
        UpdateHealthBar(_playerContext.Health.GetHealth);
    }

    private void OnDestroy()
    {
        UnsubscribeFromEventBus();
    }

    private void SubscribeToEventBus()
    {
        _eventBus.Subscribe<AmmoChanged>(OnAmmoChanged);
        _eventBus.Subscribe<WeaponChanged>(OnWeaponChanged);
        _eventBus.Subscribe<PlayerHealthChanged>(OnPlayerHealthChanged);
        _eventBus.Subscribe<ItemInventoryChanged>(OnItemInventoryChanged);
    }

    private void UnsubscribeFromEventBus()
    {
        _eventBus.Unsubscribe<AmmoChanged>(OnAmmoChanged);
        _eventBus.Unsubscribe<WeaponChanged>(OnWeaponChanged);
        _eventBus.Unsubscribe<PlayerHealthChanged>(OnPlayerHealthChanged);
        _eventBus.Unsubscribe<ItemInventoryChanged>(OnItemInventoryChanged);
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

    private void OnPlayerHealthChanged(PlayerHealthChanged evt)
    {
        UpdateHealthBar(evt.HealthValue);
    }

    private void OnItemInventoryChanged(ItemInventoryChanged evt)
    {
        int keyCards = evt.items.Where(i => i == ItemDropType.KeyCard).Count();
        int gears = evt.items.Where(i => i == ItemDropType.Gear).Count();

        _keyCardsCount.text = "\u00D7" + $"{keyCards}";
        _gearsCount.text = "\u00D7" + $"{gears}";
    }

    private void UpdateHealthBar(float healthValue)
    {
        Vector3 newScale = Vector3.one;
        newScale.x = healthValue / 100;
        _healthBar.transform.localScale = newScale;
    }
}
