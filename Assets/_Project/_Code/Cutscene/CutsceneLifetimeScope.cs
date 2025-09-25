using VContainer;
using VContainer.Unity;

public class CutsceneLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<CutsceneEntryPoint>();
    }
}
