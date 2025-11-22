using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<GameStateManager>(Lifetime.Singleton);
        builder.Register<IEventBus, EventBus>(Lifetime.Singleton);
        builder.Register<ISceneLoader, SceneLoader>(Lifetime.Singleton);
        builder.Register<ISettingsManager, SettingsManager>(Lifetime.Singleton).As<IInitializable>();

        builder.RegisterComponentInHierarchy<GlobalInputService>().As<IStartable>();
        builder.RegisterComponentInHierarchy<SoundMixerManager>();
        builder.RegisterComponentInHierarchy<ScreenFade>();

        builder.RegisterEntryPoint<GameEntryPoint>();

        DontDestroyOnLoad(this.gameObject);
    }
}
