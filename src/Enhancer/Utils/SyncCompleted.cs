using System;
using HarmonyLib;
using Lethal_Company_Enhancer.Patches;

namespace Lethal_Company_Enhancer.Utils;

public static class SyncCompleted
{
    public static bool HasAlreadyPatched { get; private set; } = false;

    public static void SyncCompletedCallback(object Sender, EventArgs e)
    {

        if (HasAlreadyPatched || !Plugin.Cfg.Enabled)
        {
            Plugin.Log.LogInfo("Globally disabled or already patched, exiting");
            return;
        }

        Harmony patcher = new(LCMPluginInfo.PLUGIN_GUID);

        Plugin.Log.LogInfo($"Sync complete, patching now");
        patcher.PatchAll(typeof(HangarShipDoorPatches));
        patcher.PatchAll(typeof(RoundManagerPatches));
        patcher.PatchAll(typeof(StartOfRoundPatches));
        patcher.PatchAll(typeof(TerminalPatches));
        patcher.PatchAll(typeof(TimeOfDayPatches));

        HasAlreadyPatched = true;
    }
}
