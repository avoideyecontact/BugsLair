using VContainer;
using VContainer.Unity;

public class MenuLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<MainMenuController>();

        // dont need this for now
        builder.RegisterEntryPoint<MenuEntryPoint>();
    }
}
