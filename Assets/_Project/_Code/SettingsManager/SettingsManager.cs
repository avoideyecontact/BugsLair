using System;
using System.IO;
using UnityEngine;
using VContainer.Unity;

public class GameSettings
{
    public float masterVolume = 0.7f;
    public float sfxVolume = 1.0f;
    public float musicVolume = 1.0f;
    public bool vsync = true;
}

public interface ISettingsManager
{
    GameSettings Settings { get; }
    void LoadSettings();
    void SaveSettings();
    void ApplySettings(GameSettings newSettings);
}

public class SettingsManager : IInitializable, ISettingsManager
{
    private GameSettings _currentSettings;
    private readonly string _settingsPath;

    public GameSettings Settings => _currentSettings;

    public SettingsManager()
    {
        _settingsPath = Path.Combine(Application.persistentDataPath, "BugsLairSettings.json");
    }

    public void Initialize()
    {
        LoadSettings();
    }

    public void LoadSettings()
    {
        try
        {
            if (File.Exists(_settingsPath))
            {
                string json = File.ReadAllText(_settingsPath);
                _currentSettings = JsonUtility.FromJson<GameSettings>(json);
            }
            else
            {
                _currentSettings = new GameSettings();
                SaveSettings();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load settings: {e.Message}");
            _currentSettings = new GameSettings();
        }
    }

    public void SaveSettings()
    {
        try
        {
            string json = JsonUtility.ToJson(_currentSettings, true);
            File.WriteAllText(_settingsPath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save settings: {e.Message}");
        }
    }

    public void ApplySettings(GameSettings newSettings)
    {
        _currentSettings = newSettings;
        SaveSettings();
    }
}
