/**********************************************************
    Single Player Enhancements Mod for Lethal Company

    Authors:
        Mama Llama
        Flowerful

    See Docs/License for information about copying
    distributing this project
***********************************************************/

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace Enhancer
{
    [BepInPlugin(LCMPluginInfo.PLUGIN_GUID, LCMPluginInfo.PLUGIN_NAME, LCMPluginInfo.PLUGIN_VERSION)]
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

            Harmony patcher = new(LCMPluginInfo.PLUGIN_GUID);

            Logger.LogInfo($"Enabled, patching now");
            patcher.PatchAll(typeof(SPPatcher));

            /*
            Been meaning to do this for a while, completely ignore
            the old and broken suit patches.

            if (Cfg.DoSuitPatches)
            {
                Logger.LogInfo("Doing suit patches");
                patcher.PatchAll(typeof(SuitPatches));
            }
            */

            Logger.LogInfo("Doing protection patches");
            patcher.PatchAll(typeof(SPProtectionPatches));
        }
    }
}
