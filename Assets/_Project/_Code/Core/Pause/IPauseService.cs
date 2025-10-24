
public interface IPauseService
{
    void PauseGame();
    void ResumeGame();
    void TogglePause();
    bool isPaused { get; }
}
