using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine.SceneManagement;

public class SceneLoader : ISceneLoader, IDisposable
{
    private readonly CancellationTokenSource _cts;
    private readonly IEventBus _eventBus;
    private readonly ScreenFade _screenFade;

    public static string CurrentScene => SceneManager.GetActiveScene().name;

    public SceneLoader(IEventBus eventBus, ScreenFade screenFade)
    {
        _eventBus = eventBus;
        _screenFade = screenFade;
        _cts = new CancellationTokenSource();
    }

    public async UniTask LoadSceneAsync(string sceneName, bool useFade = false)
    {
        _cts.Token.ThrowIfCancellationRequested();
        _eventBus.Publish(new SceneLoadStartedEvent(sceneName));

        if (useFade)
        {
            await _screenFade.FadeInAsync().AttachExternalCancellation(_cts.Token);
        }

        var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        await asyncOperation.ToUniTask(cancellationToken: _cts.Token);
        _eventBus.Publish(new SceneLoadedEvent(sceneName));

        if (useFade)
        {
            await UniTask.WaitForSeconds(0.25f, cancellationToken: _cts.Token);
            await _screenFade.FadeOutAsync().AttachExternalCancellation(_cts.Token);
        }
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
