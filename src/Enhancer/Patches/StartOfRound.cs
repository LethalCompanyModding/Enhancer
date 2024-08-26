using HarmonyLib;
using Unity.Netcode;
using Lethal_Company_Enhancer.Utils;

namespace Lethal_Company_Enhancer.Patches;

public class StartOfRoundPatches
{
    [HarmonyPatch(typeof(StartOfRound), "Start")]
    [HarmonyPrefix]
    public static bool StartOfRoundShipStartPre()
    {
        Plugin.Log.LogInfo("StartOfRound Start");

        if (!NetworkManager.Singleton.IsServer)
        {
            if (!SyncCompleted.Completed)
            {
                Plugin.Log.LogError("Client joined without receiving sync");
                Plugin.Cfg.Enabled.LocalValue = false;
                Harmony harmony = new(LCMPluginInfo.PLUGIN_GUID);
                harmony.UnpatchSelf();
                return true;
            }
        }

        TimeOfDay.Instance.quotaVariables.deadlineDaysAmount = Plugin.Cfg.DaysPerQuota;

        //never skip
        return true;
    }
}
