using VContainer.Unity;

public class MenuEntryPoint : IStartable
{
    private readonly ISceneLoader _sceneLoader;

    public MenuEntryPoint(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    void IStartable.Start()
    {

    }
}
