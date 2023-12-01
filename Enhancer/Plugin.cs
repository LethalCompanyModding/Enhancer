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
using BepInEx;
using BepInEx.Logging;
using Enhancer.Patches;
using HarmonyLib;

namespace Enhancer;

[BepInPlugin("com.github.lordfirespeed.augmented_enhancer", PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static ManualLogSource Log;
    public static PluginConfig BoundConfig;
    private void Awake()
    {
        // Plugin startup logic
        Log = Logger;

        BoundConfig = new(this);

        if (!BoundConfig.Enabled)
        {
            Logger.LogInfo("Globally disabled, exit");
            return;
        }

        Harmony patcher = new(PluginInfo.PLUGIN_GUID);

        var patches = new PatchInfo[]
        {
            new("Configured values", typeof(ConfiguredValues)),
            new("Improved scan command", typeof(ImprovedScanCommand)),
            new("Item protection", typeof(ItemProtection)),
            new("Price randomizer", typeof(PriceRandomizer)),
            new("Suit unlockables", typeof(SuitUnlockables), loadCondition: () => BoundConfig.DoSuitPatches),
        };
        
        Logger.LogInfo("Enabled, applying all patches");
        foreach (var patch in patches)
        {
            if (!patch.ShouldLoad()) continue;
            Logger.LogInfo($"Applying {patch.Name} patches...");
            patcher.PatchAll(patch.PatchType);
        }
    }

    private class PatchInfo {
        public PatchInfo(string name, Type patchType, Func<bool>? loadCondition = null) => 
            (Name, PatchType, LoadCondition) = (name, patchType, loadCondition);
        
        public string Name { get; }
        public Type PatchType { get; }
        private Func<bool> LoadCondition;
        public bool ShouldLoad() => LoadCondition == null || LoadCondition();
    }
}