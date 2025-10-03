using UnityEngine;

public class PauseService : IPauseService
{
    private readonly IEventBus _eventBus;
    private bool _paused;

    public bool isPaused => _paused;

    public PauseService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void PauseGame()
    {
        SetPause(true);
    }

    public void ResumeGame()
    {
        SetPause(false);
    }

    public void SetPause(bool value)
    {
        if (_paused == value)
            return;

        _paused = value;

        if (_paused)
        {
            Time.timeScale = 0;
            _eventBus.Publish(new GamePaused());
        }
        else
        {
            Time.timeScale = 1;
            _eventBus.Publish(new GameResumed());
        }
    }
}
