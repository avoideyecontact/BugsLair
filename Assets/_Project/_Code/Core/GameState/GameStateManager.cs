using UnityEngine;

public class GameStateManager
{
    private bool _inventoryIsOpened;
    private bool _gameIsPaused;

    private IEventBus _eventBus;
    private GlobalInputService _globalInputService;

    public bool GameIsPaused => _gameIsPaused;
    
    public GameStateManager(IEventBus eventBus, GlobalInputService globalInputService)
    {
        _eventBus = eventBus;
        _globalInputService = globalInputService;

        SubscribeToEventBus();
        SubscribeToInput();

        if (SceneLoader.CurrentScene != "1_Menu")
            HideCursor();
    }

    ~GameStateManager()
    {
        UnsubscribeFromEventBus();
        UnsubscribeFromInput();
    }

    private void SubscribeToEventBus()
    {
        _eventBus.Subscribe<SceneLoadedEvent>(OnSceneLoaded);
    }

    private void UnsubscribeFromEventBus()
    {
        _eventBus.Unsubscribe<SceneLoadedEvent>(OnSceneLoaded);
    }

    private void SubscribeToInput()
    {
        _globalInputService.PauseStarted += TogglePause;
    }

    private void UnsubscribeFromInput()
    {
        _globalInputService.PauseStarted -= TogglePause;
    }

    private void UpdateGameState()
    {
        if (_gameIsPaused || _inventoryIsOpened)
        {
            Time.timeScale = 0;
            AudioListener.volume = 0.25f;
            ShowCursor();
        }
        else
        {
            Time.timeScale = 1;
            AudioListener.volume = 1f;
            HideCursor();
        }
    }

    public void PauseGame()
    {
        _gameIsPaused = true;
        _eventBus.Publish(new GamePaused());
        UpdateGameState();
    }

    public void ResumeGame()
    {
        _gameIsPaused = false;
        _eventBus.Publish(new GameResumed());
        UpdateGameState();
    }

    private void TogglePause()
    {
        if (SceneLoader.CurrentScene == "1_Menu")
            return;

        if (_gameIsPaused) ResumeGame();
        else PauseGame();
    }

    public void OpenInventory()
    {
        _inventoryIsOpened = true;
        _eventBus.Publish(new InventoryOpened());
        UpdateGameState();
    }

    public void CloseInventory()
    {
        _inventoryIsOpened = false;
        _eventBus.Publish(new InventoryClosed());
        UpdateGameState();
    }

    private void OnSceneLoaded(SceneLoadedEvent evt)
    {
        ResumeGame();
        CloseInventory();

        if (evt.SceneName == "1_Menu")
            ShowCursor();
    }

    public void ShowCursor()
    {
        // order of this two lines is important
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideCursor()
    {
        // order of this two lines is important
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
