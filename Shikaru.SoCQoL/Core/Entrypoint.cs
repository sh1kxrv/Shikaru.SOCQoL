using BepInEx;

namespace Shikaru.SoCQoL.Core;

[BepInPlugin(ModConstant.Id, "Shikaru.SoCQoL", "1.0.0.0")]
public class Entrypoint : BaseUnityPlugin
{
    void Awake()
    {
        try
        {
            Logger.LogMessage("Initializing...");

            PatchMethods();

            RegisterModComponent();

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

    private void RegisterModComponent()
    {
        try
        {
            GameObject go = new GameObject(ModConstant.Id);
            go.hideFlags = HideFlags.HideAndDontSave;
            DontDestroyOnLoad(go);

            ModComponent component = go.AddComponent<ModComponent>();
            if (component is null)
            {
                Destroy(go);
                Logger.LogError("Missing required component");
            }

            Logger.LogInfo("Component successfully initialized!");
        }
        catch(Exception ex)
        {
            Logger.LogError($"{ModConstant.Id}: {ex}");
        }
    }
}
