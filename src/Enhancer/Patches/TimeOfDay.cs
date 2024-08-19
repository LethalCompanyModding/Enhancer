using HarmonyLib;
using Lethal_Company_Enhancer.Utils;

namespace Lethal_Company_Enhancer.Patches;

public class TimeOfDayPatches
{
    [HarmonyPatch(typeof(TimeOfDay), nameof(TimeOfDay.SetBuyingRateForDay))]
    [HarmonyPostfix]
    public static void BuyingRatePost(TimeOfDay __instance)
    {
        Plugin.Log.LogInfo("TimeOfDay SetBuyingRateForDay");

        if (Plugin.Cfg.UseRandomPrices)
        {
            PriceRandomizer.Randomize();
        }

        //Minimum sale rate fixes negative rates
        if (StartOfRound.Instance.companyBuyingRate < Plugin.Cfg.MinimumBuyRate)
            StartOfRound.Instance.companyBuyingRate = Plugin.Cfg.MinimumBuyRate;

        //Make sure clients are up to date
        //StartOfRound.Instance.SyncCompanyBuyingRateClientRpc(StartOfRound.Instance.companyBuyingRate);
        StartOfRound.Instance.SyncCompanyBuyingRateServerRpc();

    }

    [HarmonyPatch(typeof(TimeOfDay), "Start")]
    [HarmonyPostfix]
    public static void TimeOfDayOnStartPost(TimeOfDay __instance)
    {
        Plugin.Log.LogInfo("TimeOfDay Start");

        //Sets game speed
        __instance.globalTimeSpeedMultiplier = Plugin.Cfg.TimeScale;
    }
}
