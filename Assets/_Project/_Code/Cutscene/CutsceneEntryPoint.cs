using UnityEngine;
using VContainer.Unity;

public class CutsceneEntryPoint : IStartable
{
    private readonly ISceneLoader _sceneLoader;

    public CutsceneEntryPoint(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    void IStartable.Start()
    {
        Debug.Log("Sending to gameplay");
        _sceneLoader.LoadSceneAsync("3_Gameplay");
    }
}
