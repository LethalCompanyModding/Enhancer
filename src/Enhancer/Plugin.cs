﻿/**********************************************************
    Single Player Enhancements Mod for Lethal Company

    Author:
        Robyn

    Maintainers:
        N/A

    See LICENSE file for information about copying
    or distributing this project
***********************************************************/

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Lethal_Company_Enhancer.Config;
using Lethal_Company_Enhancer.Patches;
using Lethal_Company_Enhancer.Utils;

[BepInDependency("com.sigurd.csync", "5.0.1")]
[BepInPlugin(LCMPluginInfo.PLUGIN_GUID, LCMPluginInfo.PLUGIN_NAME, LCMPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    /****

    Don't know how else to fix the warnings on these two fields
    so I made them "intentionally left null" for now

    ****/
    public static ManualLogSource Log = null!;
    public static PluginConfig Cfg = null!;
    private void Awake()
    {
        // Plugin startup logic
        Log = Logger;

        Config.SaveOnConfigSet = false;
        Cfg = new(base.Config);
        Config.Save();

        Harmony patcher = new(LCMPluginInfo.PLUGIN_GUID);

        Log.LogInfo($"Patching now");
        patcher.PatchAll(typeof(HangarShipDoorPatches));
        patcher.PatchAll(typeof(RoundManagerPatches));
        patcher.PatchAll(typeof(StartOfRoundPatches));
        patcher.PatchAll(typeof(TerminalPatches));
        patcher.PatchAll(typeof(TimeOfDayPatches));

        Logger.LogInfo("Attaching a sync listener");
        Cfg.InitialSyncCompleted += SyncCompleted.SyncCompletedCallback;
    }
}
