using UnityEngine;

public class GameStateManager
{
    private bool _inventoryIsOpened;
    private bool _gameIsPaused;

    private IEventBus _eventBus;

    public bool GameIsPaused => _gameIsPaused;

    public GameStateManager(IEventBus eventBus)
    {
        _eventBus = eventBus;
        SubscribeToEventBus();
    }

    ~GameStateManager()
    {
        UnsubscribeFromEventBus();
    }

    private void SubscribeToEventBus()
    {
        _eventBus.Subscribe<SceneLoadedEvent>(OnSceneLoaded);
    }

    private void UnsubscribeFromEventBus()
    {
        _eventBus.Unsubscribe<SceneLoadedEvent>(OnSceneLoaded);
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

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void HideCursor()
    {
        Debug.Log("Hided");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
