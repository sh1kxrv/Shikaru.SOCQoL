using SongsOfConquest.Client.Gamestate;

namespace Shikaru.SoCQoL.Handler;
public class AdventureHandler
{
    public static AdventureHandler Instance { get; private set; }

    [Inject]
    public IClientAdventureFacade ClientAdventureFacade;

    public AdventureHandler()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
