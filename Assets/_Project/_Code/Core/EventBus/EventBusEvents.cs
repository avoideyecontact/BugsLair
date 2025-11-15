
using System.Collections.Generic;

public struct SceneLoadStartedEvent
{
    public string SceneName;

    public SceneLoadStartedEvent(string sceneName)
    {
        SceneName = sceneName;
    }
}

public struct SceneLoadedEvent
{
    public string SceneName;

    public SceneLoadedEvent(string sceneName)
    {
        SceneName = sceneName;
    }
}

public struct GamePaused { }
public struct GameResumed { }

public struct WeaponChanged
{
    public WeaponType WeaponType;
    public int Ammo;

    public WeaponChanged(WeaponType weaponType, int ammo)
    {
        WeaponType = weaponType;
        Ammo = ammo;
    }
}

public struct AmmoChanged
{
    public int Ammo;
    public WeaponType WeaponType;
}

public struct PlayerHealthChanged
{
    public float HealthValue;
}

public struct PlayerDamaged
{
    public float DamageValue;
}

public struct PlayerDeath
{ }

public struct SettingsUIOpened
{ }

public struct SettingsUIClosed
{ }

public struct CameraSensitivityChanged
{
    public float Sensitivity;
}

public struct CockroachKilled
{ }

public struct PlayerIsUsingWeapon
{
    public WeaponType weaponType;
}

public struct ItemInventoryChanged
{
    public List<ItemDropType> items;
}
