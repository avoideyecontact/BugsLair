
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
