using HarmonyLib;

namespace Lethal_Company_Enhancer.Patches;

public class StartOfRoundPatches
{
    [HarmonyPatch(typeof(StartOfRound), "Start")]
    [HarmonyPrefix]
    public static bool StartOfRoundShipStartPre()
    {
        Plugin.Log.LogInfo("StartOfRound Start");

        TimeOfDay.Instance.quotaVariables.deadlineDaysAmount = Plugin.Cfg.DaysPerQuota;

        //never skip
        return true;
    }
}
