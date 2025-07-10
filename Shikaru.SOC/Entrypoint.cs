using BepInEx;
using HarmonyLib;
using Shikaru.SOC.Core;
using System;
using System.Reflection;

namespace Shikaru.SOC
{
    [BepInPlugin(ModConstant.Id, "Shikaru SOC", "1.0.0.0")]
    public class Entrypoint : BaseUnityPlugin
    {
        void Awake()
        {
            try
            {
                Logger.LogMessage("Initializing...");

                PatchMethods();

                Logger.LogMessage("Successfully initialized!");
            }
            catch(Exception ex) 
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
}
