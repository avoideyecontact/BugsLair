
public interface IPauseService
{
    void PauseGame();
    void ResumeGame();
    bool isPaused { get; }
}
