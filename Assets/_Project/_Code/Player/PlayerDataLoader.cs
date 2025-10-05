using System;
using System.IO;
using UnityEngine;
using VContainer.Unity;

public class PlayerDataLoader : IInitializable, IPlayerDataLoader
{
    private PlayerData _currentPlayerData;
    private readonly string _playerDataPath;

    public PlayerData PlayerData => _currentPlayerData;

    public PlayerDataLoader()
    {
        _playerDataPath = Path.Combine(Application.persistentDataPath, "BugsLairPlayerData.json");
    }

    public void Initialize()
    {
        LoadPlayerData();
    }

    public void LoadPlayerData()
    {
        try
        {
            if (File.Exists(_playerDataPath))
            {
                string json = File.ReadAllText(_playerDataPath);
                _currentPlayerData = JsonUtility.FromJson<PlayerData>(json);
            }
            else
            {
                _currentPlayerData = new PlayerData();
                SavePlayerData();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load player data: {e.Message}");
            _currentPlayerData = new PlayerData();
        }
    }

    public void SavePlayerData()
    {
        try
        {
            string json = JsonUtility.ToJson(_currentPlayerData, true);
            File.WriteAllText(_playerDataPath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save player data: {e.Message}");
        }
    }

    public void ApplyPlayerData(PlayerData playerData)
    {
        _currentPlayerData = playerData;
        SavePlayerData();
    }
}
