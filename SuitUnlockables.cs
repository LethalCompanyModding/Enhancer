using HarmonyLib;
using System.Reflection;

namespace Enhancer
{
    public class SuitPatches
    {
        private static readonly MethodInfo unlockItem = typeof(StartOfRound).GetMethod("SpawnUnlockable", BindingFlags.NonPublic | BindingFlags.Instance);
        
        public static void SpawnUnlockableDelegate(StartOfRound instance, int ID)
        {
            unlockItem.Invoke(instance, new object[] {ID});
        }
        
        [HarmonyPatch(typeof(StartOfRound), "Start")]
        [HarmonyPostfix]
        public static void StartOfRoundSuitPatch(StartOfRound __instance)
        {
            Plugin.Log.LogInfo("Setting unlocked suits this round");

            //Green Suit
            SpawnUnlockableDelegate(__instance, 1);
            //Hazard Suit
            SpawnUnlockableDelegate(__instance, 2);
        }
    }
}