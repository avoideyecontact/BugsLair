using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneLoader
{
    Task LoadSceneAsync(string sceneName);
    string CurrentScene { get; }
}

public class SceneLoader : ISceneLoader
{
    private readonly IEventBus _eventBus;

    private string _currentScene = "0_Bootstrap";
    public string CurrentScene => _currentScene;

    public SceneLoader(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task LoadSceneAsync(string sceneName)
    {
        _eventBus.Publish(new SceneLoadStartedEvent(sceneName));

        var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        _currentScene = sceneName;

        _eventBus.Publish(new SceneLoadedEvent(sceneName));
    }
}
