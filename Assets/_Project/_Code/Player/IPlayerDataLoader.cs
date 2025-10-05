
public interface IPlayerDataLoader
{
    PlayerData PlayerData { get; }
    void LoadPlayerData();
    void SavePlayerData();
    void ApplyPlayerData(PlayerData player);
}
