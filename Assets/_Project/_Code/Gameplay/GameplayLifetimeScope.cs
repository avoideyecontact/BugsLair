using VContainer;
using VContainer.Unity;

public class GameplayLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IPlayerDataLoader, PlayerDataLoader>(Lifetime.Singleton).As<IInitializable>();
        builder.RegisterComponentInHierarchy<PlayerContext>();

        builder.RegisterEntryPoint<GameplayEntryPoint>();
    }
}
