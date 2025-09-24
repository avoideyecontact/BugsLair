using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IEventBus, EventBus>(Lifetime.Singleton);
        builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);

        builder.RegisterEntryPoint<GameEntryPoint>();

        DontDestroyOnLoad(this.gameObject);
    }
}
