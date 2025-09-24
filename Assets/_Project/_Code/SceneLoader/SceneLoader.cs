using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneLoader
{
    Task LoadSceneAsync(string sceneName);
    Task UnloadCurrentSceneAsync();
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
        Debug.Log($"Loading scene: {sceneName}");

        _eventBus.Publish(new SceneLoadStartedEvent(sceneName));

        if (_currentScene != "BootstrapScene")
        {
            await UnloadCurrentSceneAsync();
        }

        var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        _currentScene = sceneName;

        Debug.Log($"Loaded scene: {sceneName}");
        _eventBus.Publish(new SceneLoadedEvent(sceneName));
    }

    public async Task UnloadCurrentSceneAsync()
    {
        if (_currentScene != "0_Bootstrap" && !string.IsNullOrEmpty(_currentScene))
        {
            Debug.Log($"Unloading scene: {_currentScene}");

            var asyncOperation = SceneManager.UnloadSceneAsync(_currentScene);

            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }

            await Resources.UnloadUnusedAssets();
            await Task.Delay(100);
        }
    }

    //public async Task LoadSceneAsync(string sceneName)
    //{
    //    Debug.Log($"Loading scene: {sceneName}");
    //    _eventBus.Publish(new SceneLoadStartedEvent(sceneName));

    //    AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);
    //    loading.allowSceneActivation = false;
    //    while (loading.progress < 0.85f)
    //    {
    //        Debug.Log("Scene loading...");
    //        await UniTask.WaitForSeconds(1);
    //    }
    //    loading.allowSceneActivation = true;
    //    await loading;

    //    _eventBus.Publish(new SceneLoadedEvent(sceneName));
    //    Debug.Log("Scene loaded!");
    //}
}
