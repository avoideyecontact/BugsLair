
public interface ISettingsManager
{
    GameSettings Settings { get; }
    void LoadSettings();
    void SaveSettings();
    void ApplySettings(GameSettings newSettings);
}
