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

            Logger.LogInfo($"Enabled, patching now");

            Harmony patcher = new(PluginInfo.PLUGIN_GUID);
            patcher.PatchAll(typeof(SPPatcher));
        }
    }

    public class PluginConfig
    {

        public readonly bool Enabled;
        public readonly bool KeepConsoleEnabled;
        public readonly bool UseRandomPrices;

        public readonly float TimeScale;
        public readonly float MinimumBuyRate;
        public readonly float DoorTimer;

        public readonly int DaysPerQuota;
        public readonly int ThreatScannerType;

        public PluginConfig(Plugin BindingPlugin)
        {
            Enabled = BindingPlugin.Config.Bind(PluginInfo.PLUGIN_GUID, "bEnabled", true, "Globally enable/disable the plugin").Value;
            KeepConsoleEnabled = BindingPlugin.Config.Bind(PluginInfo.PLUGIN_GUID, "bAlwaysShowTerminal", true, "Whether to keep the terminal enabled after a player stops using it").Value;
            UseRandomPrices = BindingPlugin.Config.Bind(PluginInfo.PLUGIN_GUID, "bUseRandomPrices", false, "Enables the random prices setting. Great if you're using longer quota deadlines.\nThis uses a variety of things to randomize prices such as the company mood, time passed in the quota, etc.\nRespects the minimum sale value, too.").Value;

            TimeScale = BindingPlugin.Config.Bind(PluginInfo.PLUGIN_GUID, "fTimeScale", 1.5f, "How fast time passes on moons. Lower values mean time passes more slowly.\nRecommended value for single play: 1.15").Value;
            MinimumBuyRate = BindingPlugin.Config.Bind(PluginInfo.PLUGIN_GUID, "fMinCompanyBuyPCT", 0.0f, "The default formula for selling items to the company doesn't allow days remaining above 3.\nAlways keep this set to at least 0.0 but you probably want something higher if you have more days set for the quota.\nRecommended values for games above 3 days: 0.3 - 0.5").Value;
            DoorTimer = BindingPlugin.Config.Bind(PluginInfo.PLUGIN_GUID, "fDoorTimer", 30.0f, "How long the hangar door can be kept shut at a time (in seconds)\nRecommended values: 60.0 - 180.0").Value;

            DaysPerQuota = BindingPlugin.Config.Bind(PluginInfo.PLUGIN_GUID, "iQuotaDays", 3, "How long you have to meet each quota (in days)\nRecommended values: 3 - 7").Value;
            ThreatScannerType = BindingPlugin.Config.Bind(PluginInfo.PLUGIN_GUID, "eThreatScannerType", 0, "How the threat scanner functions. Valid types:\n0 - Disabled\n1 - Number of Enemies on level\n2 - Percentage of max enemies on level\n3 - Vague Text description (In order of threat level) [Clear -> Green -> Yellow -> Orange - Red]").Value;
        }
    }
}
