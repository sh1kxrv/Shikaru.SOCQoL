using SongsOfConquest.Client.Gamestate.Facade;
using SongsOfConquest.Common.Utilities;

namespace Shikaru.SOC.Hooks;

[HarmonyPatch(typeof(AdventureHostileTownDetails), nameof(AdventureHostileTownDetails.DrawThreatLevel))]
public static class AdventureHostileTownDetails_DrawThreatLevel
{
    public static bool Prefix(IDetailsDrawer drawer, ILocalizationHandler loc, string localizedCommanderName, ThreatLevel threatLevel, bool isUnknown, AdventureHostileTownDetails __instance)
    {
        return true;
        if (isUnknown)
        {
            drawer.AddImageDivider();
        }
        else
        {
            drawer.AddTextLeftRight(loc.GetText("Commanders/Tooltip/ThreatLevelLabel"), string.Empty);
        }
        drawer.AddSpace(DetailsEmptySpace.DetailsSpaceSize.Medium);
        drawer.AddText("Commanders/Tooltip/ThreatLevel".Localize(loc, [localizedCommanderName]), FontType.LabelSmall, HorizontalAlignment.Center, VerticalAlignment.Middle, FontColor.Limestone);
        if (isUnknown)
        {
            string unknownLocalized = "Adventure/ThreatLevel/Unknown".Localize(loc).Color(ColorUtilities.ThreatLevelColors.Unknown);
            drawer.AddText(unknownLocalized, FontType.LabelSmall, HorizontalAlignment.Center, VerticalAlignment.Middle, FontColor.White);
            string unknownWalkCloser = "Adventure/ThreatLevel/Unknown/WalkCloser".Localize(loc).Color(ColorUtilities.ThreatLevelColors.Unknown);
            drawer.AddText(unknownWalkCloser, FontType.LabelSmall, HorizontalAlignment.Center, VerticalAlignment.Middle, FontColor.White);
        }
        else
        {
            string localizedThreat = "Adventure/ThreatLevel/";
            int numberishThreatLevel = (int)threatLevel;
            drawer.AddText(
                (localizedThreat + numberishThreatLevel.ToString()).Localize(loc).Color(threatLevel.GetColor(false)),
                FontType.LabelSmall, HorizontalAlignment.Center, VerticalAlignment.Middle, FontColor.White
            );
        }

        drawer.AddSpace(DetailsEmptySpace.DetailsSpaceSize.Large);
    }
}
