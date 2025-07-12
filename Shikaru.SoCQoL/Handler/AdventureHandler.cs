using SongsOfConquest.Client.Gamestate;
using SongsOfConquest.Common.Gamestate;

namespace Shikaru.SoCQoL.Handler;
public class AdventureHandler
{
    public static AdventureHandler Instance { get; private set; }

    [Inject]
    public ICommonAdventureFacade AdventureFacade;

    [Inject]
    public ISelectionHandler SelectionHandler;

    [Inject]
    public IThreatLevelUtility ThreatLevelUtility;

    public AdventureHandler()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
