using BepInEx.Logging;

namespace Shikaru.SoCQoL.Core;
using Logger = BepInEx.Logging.Logger;

public sealed class ModComponent : MonoBehaviour
{
    public static ModComponent Instance { get; private set; }
    public static ManualLogSource Log { get; private set; }

    public void Awake()
    {
        Log = Logger.CreateLogSource("Shikaru.SoCQoL");
        Log.LogMessage("Awake component");
        try
        {
            Instance = this;

            // Some logic

            Log.LogMessage("Processed!");
        }
        catch (Exception ex)
        {
            Log.LogError(ex);
        }
    }

    public void OnDestroy()
    {
        Log.LogWarning("OnDestroy");
    }
}

