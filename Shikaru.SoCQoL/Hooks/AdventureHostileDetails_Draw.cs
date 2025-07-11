using Lavapotion.Client.InputManagement;
using SongsOfConquest.Client.Gamestate.Facade;
using SongsOfConquest.Client.InputManagement;

namespace Shikaru.SOC.Hooks;

[HarmonyPatch(typeof(AdventureHostileDetails), nameof(AdventureHostileDetails.Draw))]
public static class AdventureHostileDetails_Draw
{
    private static string MakeField(string name, int value)
    {
        return $"{name.White()} - {value.ToString().Grey()}";
    }
    private static void DrawCharacteristic(IDetailsDrawer drawer, string field)
    {
        drawer.AddText(field, FontType.LabelMedium, HorizontalAlignment.Center);
        drawer.AddSpace(DetailsEmptySpace.DetailsSpaceSize.Small);
    }
    public static bool Prefix(IDetailsDrawer drawer, ILocalizationHandler loc, AdventureHostileDetails __instance)
    {
        bool expandedTooltipsActive = GamepadGlobalHotkeysStaticAccessUnsafe.Current.ExpandedTooltipsActive;
        IInputManager inputManager = InputManagerStaticAccessUnsafe.Current;
        bool flag = (inputManager != null && inputManager.CurrentInputMode == InputMode.KeyboardMouse) || expandedTooltipsActive;
        drawer.AddHeaderDivider(__instance.GetLocalizedName(loc), __instance.IsHostile ? "" : __instance.TeamName, __instance.TeamColor, FontType.TitleSmall, FontType.LabelSmall, HorizontalAlignment.Center, VerticalAlignment.Middle);
        string text = __instance.ComparisonCommanderShortNameKey.Localize(loc);
        if (__instance.ScoutingDetailLevel > ScoutingDetailLevel.Far)
        {
            if (__instance.HasThreatLevel)
            {
                bool hasScoutingInfo = !string.IsNullOrEmpty(__instance.ScoutingProviderKey) && __instance.ScoutingProviderKey != __instance.ComparisonCommanderShortNameKey;
                if (hasScoutingInfo && flag)
                {
                    drawer.AddImageDivider();
                    drawer.AddSpace(DetailsEmptySpace.DetailsSpaceSize.Medium);
                    drawer.AddText(loc.GetText("Adventure/Scouting/ProvidingSource", [loc.GetText(__instance.ScoutingProviderKey)]), FontType.LoreMedium, HorizontalAlignment.Center, VerticalAlignment.Middle, FontColor.Granite);
                    drawer.AddSpace(DetailsEmptySpace.DetailsSpaceSize.Large);
                }
                AdventureHostileTownDetails.DrawThreatLevel(drawer, loc, text, __instance.ThreatLevel, false);
                if (__instance.Troops.Count > 0 && flag)
                {
                    drawer.AddTextLeftRight("Расширенная информация", "отряды");
                    drawer.AddSpace(DetailsEmptySpace.DetailsSpaceSize.Medium);

                    int attacks = __instance.Troops.Select((x) => x.Stats.Damage.GetValue()).Aggregate(0, (acc, x) => acc + x.average);
                    int defense = __instance.Troops.Select((x) => x.Stats.Defense.GetValue()).Aggregate(0, (acc, x) => acc + x);
                    int rangedAttacks = __instance.Troops.Select((x) => x.Stats.RangedAttack.Offense.GetValue()).Aggregate(0, (acc, x) => acc + x);
                    int initiatives = __instance.Troops.Select((x) => x.Stats.Initiative.GetValue()).Aggregate(0, (acc, x) => acc + x);

                    string attackField = MakeField("Урон", attacks);
                    string defenceField = MakeField("Защита", defense);

                    DrawCharacteristic(drawer, attackField);
                    DrawCharacteristic(drawer, defenceField);

                    if (rangedAttacks > 0)
                    {
                        string rangedField = MakeField("Дальний бой", rangedAttacks);
                        DrawCharacteristic(drawer, rangedField);
                    }

                    string initiativeField = MakeField("Инициатива", initiatives);
                    DrawCharacteristic(drawer, initiativeField);

                    drawer.AddTextLeftRight(loc.GetText("Commanders/Tooltip/Troops"), string.Empty);
                    drawer.AddSpace(DetailsEmptySpace.DetailsSpaceSize.Medium);
                    AdventureDetailUtilities.AddTroopTexts(__instance.Troops, __instance.ScoutingDetailLevel, drawer, loc, __instance.FactionLookup);
                    drawer.AddSpace(DetailsEmptySpace.DetailsSpaceSize.Large);
                }
            }
        }
        else
        {
            AdventureHostileTownDetails.DrawThreatLevel(drawer, loc, text, ThreatLevel.Insignificant, true);
        }
        if (flag)
        {
            DetailUtilities.DrawModifierTraces(drawer, loc, __instance.ModifierTraces, __instance.Id);
            DetailUtilities.DrawBacterias(drawer, loc, __instance.Bacterias, __instance.Id);
        }
        if (__instance.HasThreatLevel)
        {
            drawer.AddSpace(DetailsEmptySpace.DetailsSpaceSize.Small);
            if (!__instance.BattlefieldIsOccupied)
            {
                drawer.AddLabelWithImage(loc.GetText("Adventure/TooltipInstruction/Attack"), InputType.LeftMouseClickOrConfirm);
                return false;
            }
            string text2 = loc.GetText("Adventure/Tooltips/BattlefieldIsOccupied");
            drawer.AddLabelWithImage(text2.Red(), InputType.NoInput);
        }

        return false;
    }
}
