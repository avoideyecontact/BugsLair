using VContainer.Unity;

public class GameplayEntryPoint : IStartable
{
    private readonly PlayerFactory _playerFactory;

    public GameplayEntryPoint(PlayerFactory playerFactory)
    {
        _playerFactory = playerFactory;
    }

    void IStartable.Start()
    {
        _playerFactory.SpawnPlayer();
    }
}
