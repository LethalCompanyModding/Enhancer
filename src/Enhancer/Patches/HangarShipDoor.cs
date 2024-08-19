using HarmonyLib;

namespace Lethal_Company_Enhancer.Patches;

public class HangarShipDoorPatches
{
    [HarmonyPatch(typeof(HangarShipDoor), "Start")]
    [HarmonyPostfix]
    public static void HangarShipDoorPost(HangarShipDoor __instance)
    {
        Plugin.Log.LogInfo("HangarShipDoor Start");

        //Sets hangar door close timer
        __instance.doorPowerDuration = Plugin.Cfg.DoorTimer;
    }
}
