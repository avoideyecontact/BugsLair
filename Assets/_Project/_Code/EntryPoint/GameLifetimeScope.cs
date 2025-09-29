using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IEventBus, EventBus>(Lifetime.Singleton);
        builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
        builder.Register<IPauseService, PauseService>(Lifetime.Singleton);

        var screenFade = Instantiate(Resources.Load("ScreenFade"));
        screenFade.name = "ScreenFade";
        DontDestroyOnLoad(screenFade);

        builder.RegisterComponentInHierarchy<ScreenFade>();

        builder.RegisterEntryPoint<GameEntryPoint>();

        DontDestroyOnLoad(this.gameObject);
    }
}
