
public struct SceneLoadStartedEvent
{
    public string SceneName;

    public SceneLoadStartedEvent(string sceneName)
    {
        SceneName = sceneName;
    }
}

public struct SceneLoadedEvent
{
    public string SceneName;

    public SceneLoadedEvent(string sceneName)
    {
        SceneName = sceneName;
    }
}
