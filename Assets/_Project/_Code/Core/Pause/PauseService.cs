using UnityEngine;

public class PauseService : IPauseService
{
    private readonly IEventBus _eventBus;
    private bool _paused;

    public bool isPaused => _paused;

    public PauseService(IEventBus eventBus)
    {
        _eventBus = eventBus;
        _eventBus.Subscribe<SceneLoadedEvent>(OnSceneLoaded);
    }

    public void PauseGame()
    {
        SetPause(true);
    }

    public void ResumeGame()
    {
        SetPause(false);
    }

    public void TogglePause()
    {
        if (_paused) SetPause(false);
        else SetPause(true);
    }

    public void SetPause(bool value)
    {
        if (_paused == value)
            return;

        _paused = value;

        if (_paused)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
            _eventBus.Publish(new GamePaused());
        }
        else
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            _eventBus.Publish(new GameResumed());
        }
    }

    private void OnSceneLoaded(SceneLoadedEvent evt)
    {
        ResumeGame();
    }
}
