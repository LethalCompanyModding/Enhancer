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

using BepInEx;
using BepInEx.Logging;
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

            Logger.LogInfo($"Enabled, patching now");
            patcher.PatchAll(typeof(SPPatcher));

            if (Cfg.DoSuitPatches)
            {
                Logger.LogInfo("Doing suit patches");
                patcher.PatchAll(typeof(SuitPatches));
            }
        }
    }
}
