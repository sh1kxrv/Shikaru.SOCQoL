using Shikaru.SoCQoL.Handler;

namespace Shikaru.SoCQoL.Installer;

public class AdventureInstaller : InstallerBase
{
    public override void InstallBindings()
    {
        Container.Bind<AdventureHandler>().FromNew().AsSingle().NonLazy();
    }
}
