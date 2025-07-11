using SongsOfConquest.Client.Gamestate;

namespace Shikaru.SoCQoL.Handler;
public class AdventureHandler
{
    public static AdventureHandler Instance { get; private set; }

    [Inject]
    public IClientAdventureFacade ClientAdventureFacade;

    [Inject]
    public ISelectionHandler SelectionHandler;

    public AdventureHandler()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
