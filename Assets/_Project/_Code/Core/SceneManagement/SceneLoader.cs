using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneLoader : ISceneLoader
{
    private readonly IEventBus _eventBus;
    private readonly ScreenFade _screenFade;

    public string CurrentScene => SceneManager.GetActiveScene().name.ToString();

    public SceneLoader(IEventBus eventBus, ScreenFade screenFade)
    {
        _eventBus = eventBus;
        _screenFade = screenFade;
    }

    public async UniTask LoadSceneAsync(string sceneName, bool useFade = false)
    {
        _eventBus.Publish(new SceneLoadStartedEvent(sceneName));

        var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        asyncOperation.allowSceneActivation = false;

        if (useFade)
        {
            await _screenFade.FadeInAsync();
        }

        asyncOperation.allowSceneActivation = true;

        while (!asyncOperation.isDone)
        {
            await UniTask.Yield();
        }

        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        _eventBus.Publish(new SceneLoadedEvent(sceneName));

        if (useFade)
        {
            await UniTask.WaitForSeconds(0.25f);
            await _screenFade.FadeOutAsync();
        }
    }
}
