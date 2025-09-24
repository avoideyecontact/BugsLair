using UnityEngine;
using VContainer.Unity;

public class GameEntryPoint : IStartable
{
    private readonly ISceneLoader _sceneLoader;

    public GameEntryPoint(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    void IStartable.Start()
    {
        Debug.Log("Entry point");
        _sceneLoader.LoadSceneAsync("1_Menu");
    }
}
