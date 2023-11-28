/**********************************************************
    Single Player Enhancements Mod for Lethal Company

    Authors:
        Mama Llama
        Flowerful

    See Docs/License for information about copying
    distributing this project

    See Docs/Installing for information on how to
    use this mod in your game
***********************************************************/

using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using Enhancer.Patches;
using HarmonyLib;

namespace Enhancer
{
    [BepInPlugin("mom.llama.enhancer", PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;
        public static PluginConfig Cfg;
        private void Awake()
        {
            // Plugin startup logic
            Log = Logger;

            Cfg = new(this);

            if (!Cfg.Enabled)
            {
                Logger.LogInfo("Globally disabled, exit");
                return;
            }

            Harmony patcher = new(PluginInfo.PLUGIN_GUID);

            var patches = new Dictionary<string, Type>
            {
                ["Configured values"] = typeof(ConfiguredValues),
                ["Improved scan command"] = typeof(ImprovedScanCommand),
                ["Item protection"] = typeof(ItemProtection),
                ["Price randomizer"] = typeof(PriceRandomizer),
                ["Suit unlockables"] = typeof(SuitUnlockables),
            };
            
            Logger.LogInfo("Enabled, applying all patches");
            foreach (var keyValuePair in patches)
            {
                Logger.LogInfo($"Applying {keyValuePair.Key} patches...");
                patcher.PatchAll(keyValuePair.Value);
            }
        }
    }
}
