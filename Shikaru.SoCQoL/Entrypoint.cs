using BepInEx;
using Bepinject;
using I2.Loc;
using Shikaru.SoCQoL.Core;
using Shikaru.SoCQoL.Installer;
using SongsOfConquest.SocialPlatforms;

namespace Shikaru.SoCQoL;

[BepInPlugin(ModConstant.Id, "Shikaru.SoCQoL", "1.0.0.0")]
public class Entrypoint : BaseUnityPlugin
{
    public static Entrypoint Instance { get; private set; }
    void Awake()
    {
        try
        {
            Instance = this;

            Logger.LogMessage("Initializing...");

            PatchMethods();

            Zenjector.Install<AdventureInstaller>().OnScene("AdventureScene");

            Logger.LogMessage("Successfully initialized!");
        }
        catch (Exception ex)
        {
            Logger.LogError($"Failed to initialize the mod: {ex}");
        }
    }

    private void PatchMethods()
    {
        try
        {
            Logger.LogInfo("[Harmony] Patching methods...");
            Harmony harmony = new Harmony(ModConstant.Id);
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (var type in AccessTools.GetTypesFromAssembly(assembly))
            {
                PatchClassProcessor processor = harmony.CreateClassProcessor(type);
                if (processor.Patch()?.Count > 0)
                    Logger.LogInfo($"[Harmony] {type.Name} successfully applied.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to patch methods.", ex);
        }
    }
}
