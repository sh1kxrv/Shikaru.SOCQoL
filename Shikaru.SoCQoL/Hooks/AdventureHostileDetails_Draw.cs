using Lavapotion.Client.InputManagement;
using Shikaru.SoCQoL.Handler;
using SongsOfConquest.Client.Gamestate.Facade;
using SongsOfConquest.Client.InputManagement;
using System.Globalization;

namespace Shikaru.SOC.Hooks;

[HarmonyPatch(typeof(AdventureHostileDetails), nameof(AdventureHostileDetails.Draw))]
public static class AdventureHostileDetails_Draw
{
    private static NumberFormatInfo format = new NumberFormatInfo { NumberGroupSeparator = " " };
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
                    drawer.AddTextLeftRight("Сложность", "подробно");
                    drawer.AddSpace(DetailsEmptySpace.DetailsSpaceSize.Medium);

                    var currentCommander = AdventureHandler.Instance.SelectionHandler.SelectedCommander;
                    var troops = AdventureHandler.Instance.AdventureFacade.Troops.GetForCommander(currentCommander.Id);
                    
                    var defendingCommander = AdventureHandler.Instance.AdventureFacade.Commanders.Get(__instance.Id);
                    var defendingTroops = AdventureHandler.Instance.AdventureFacade.Troops.GetForCommander(defendingCommander.Id);

                    float attackingThreatValue = AdventureHandler.Instance.ThreatLevelUtility.GetThreatValue(currentCommander, troops, null, false);
                    float defendingThreatValue = AdventureHandler.Instance.ThreatLevelUtility.GetThreatValue(defendingCommander, defendingTroops, null, false);

                    // TODO: Добавить расчёт дальнего и ближнего боя отрядов
                    if (attackingThreatValue > defendingThreatValue)
                    {
                        drawer.AddText($"Вы {"сильнее".Green().Bold()} противника в {Math.Round(attackingThreatValue / defendingThreatValue, 1)} раз(а)", FontType.LabelMedium);
                    }
                    else if (attackingThreatValue < defendingThreatValue)
                    {
                        drawer.AddText($"Вы {"слабее".SoftRed().Bold()} противника в {Math.Round(defendingThreatValue / attackingThreatValue, 1)} раз(а)", FontType.LabelMedium);
                    }
                    else
                    {
                        drawer.AddText("Ваши силы равны", FontType.LabelMedium);
                        drawer.AddBottomFade(Color.grey);
                    }

                    drawer.AddSpace(DetailsEmptySpace.DetailsSpaceSize.Small);

                    drawer.AddText($"< Вы: ({Math.Round(attackingThreatValue, 1).ToString("n", format)}) vs ({Math.Round(defendingThreatValue, 1).ToString("n", format)}) >", FontType.LabelTiny, HorizontalAlignment.Center, VerticalAlignment.Middle, FontColor.Granite);

                    drawer.AddSpace(DetailsEmptySpace.DetailsSpaceSize.Medium);

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
