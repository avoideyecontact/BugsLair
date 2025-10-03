using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneLoader : ISceneLoader
{
    private readonly IEventBus _eventBus;
    private readonly ScreenFade _screenFade;

    private string _currentScene = "0_Bootstrap";
    public string CurrentScene => _currentScene;

    public SceneLoader(IEventBus eventBus, ScreenFade screenFade)
    {
        _eventBus = eventBus;
        _screenFade = screenFade;
    }

    public async UniTask LoadSceneAsync(string sceneName, bool useFade = false)
    {
        _eventBus.Publish(new SceneLoadStartedEvent(sceneName));

        if (useFade)
        {
            await _screenFade.FadeInAsync();
        }

        var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!asyncOperation.isDone)
        {
            await UniTask.Yield();
        }

        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        _currentScene = sceneName;

        _eventBus.Publish(new SceneLoadedEvent(sceneName));

        if (useFade)
        {
            await _screenFade.FadeOutAsync();
        }
    }
}
