/**********************************************************
    Plugin Config Information Class

    All configuration options go in here
***********************************************************/

using BepInEx.Configuration;
using CSync.Lib;
using CSync.Extensions;
using Lethal_Company_Enhancer.Enums;

namespace Lethal_Company_Enhancer.Config;
public class PluginConfig : SyncedConfig2<PluginConfig>
{
    [field: SyncedEntryField] public SyncedEntry<bool> Enabled { get; }
    [field: SyncedEntryField] public SyncedEntry<bool> KeepConsoleEnabled { get; }
    [field: SyncedEntryField] public SyncedEntry<bool> UseRandomPrices { get; }

    [field: SyncedEntryField] public SyncedEntry<float> TimeScale { get; }
    [field: SyncedEntryField] public SyncedEntry<float> MinimumBuyRate { get; }
    [field: SyncedEntryField] public SyncedEntry<float> DoorTimer { get; }

    [field: SyncedEntryField] public SyncedEntry<int> DaysPerQuota { get; }
    [field: SyncedEntryField] public SyncedEntry<int> ThreatScannerType { get; }

    [field: SyncedEntryField] public SyncedEntry<ProtectionType> ScrapProtection { get; }

    public PluginConfig(ConfigFile Config) : base(LCMPluginInfo.PLUGIN_GUID)
    {
        Enabled = Enabled = Config.BindSyncedEntry(LCMPluginInfo.PLUGIN_GUID, "bEnabled", true, "Globally enable/disable the plugin");
        KeepConsoleEnabled = KeepConsoleEnabled = Config.BindSyncedEntry(LCMPluginInfo.PLUGIN_GUID, "bAlwaysShowTerminal", true, "Whether to keep the terminal enabled after a player stops using it\nHost Required: No");
        UseRandomPrices = UseRandomPrices = Config.BindSyncedEntry(LCMPluginInfo.PLUGIN_GUID, "bUseRandomPrices", false, "Enables the random prices setting. Great if you're using longer quota deadlines.\nThis uses a variety of things to randomize prices such as the company mood, time passed in the quota, etc.\nRespects the minimum sale value, too.\nHost Required: Yes");

        TimeScale = TimeScale = Config.BindSyncedEntry(LCMPluginInfo.PLUGIN_GUID, "fTimeScale", 1.5f, "How fast time passes on moons. Lower values mean time passes more slowly.\nRecommended value for single play: 1.15\nHost Required: Yes");
        MinimumBuyRate = MinimumBuyRate = Config.BindSyncedEntry(LCMPluginInfo.PLUGIN_GUID, "fMinCompanyBuyPCT", 0.0f, "The default formula for selling items to the company doesn't allow days remaining above 3.\nAlways keep this set to at least 0.0 but you probably want something higher if you have more days set for the quota.\nRecommended values for games above 3 days: 0.3 - 0.5\nHost Required: Yes");
        DoorTimer = DoorTimer = Config.BindSyncedEntry(LCMPluginInfo.PLUGIN_GUID, "fDoorTimer", 30.0f, "How long the hangar door can be kept shut at a time (in seconds)\nRecommended values: 60.0 - 180.0\nHost Required: All players should use the same setting here");

        DaysPerQuota = DaysPerQuota = Config.BindSyncedEntry(LCMPluginInfo.PLUGIN_GUID, "iQuotaDays", 3, "How long you have to meet each quota (in days)\nRecommended values: 3 - 7\nHost Required: Yes");
        ThreatScannerType = ThreatScannerType = Config.BindSyncedEntry(LCMPluginInfo.PLUGIN_GUID, "eThreatScannerType", 0, "How the threat scanner functions. Valid types:\n0 - Disabled\n1 - Number of Enemies on level\n2 - Percentage of max enemies on level\n3 - Vague Text description (In order of threat level) [Clear -> Green -> Yellow -> Orange - Red]\nHost Required: No");

        ScrapProtection = ScrapProtection = Config.BindSyncedEntry(LCMPluginInfo.PLUGIN_GUID, "eScrapProtection", ProtectionType.SAVE_NONE, "Sets how scrap will be handled when all players die in a round.\nSAVE_NONE: Default all scrap is deleted\nSAVE_ALL: No scrap is removed\nSAVE_COINFLIP: Each piece of scrap has a 50/50 of being removed\nHost Required: Yes");

        ConfigManager.Register(this);
    }
}
