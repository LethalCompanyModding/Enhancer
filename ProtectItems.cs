using UnityEngine;
using HarmonyLib;
using Unity.Netcode;

namespace Enhancer
{
    public class SPProtectionPatches
    {

        public enum ProtectionType
        {
            SAVE_NONE,
            SAVE_ALL,
            SAVE_COINFLIP
        }

        private static System.Random rng;

        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.DespawnPropsAtEndOfRound))]
        [HarmonyPrefix]
        public static bool ProtectionPrefix(RoundManager __instance, bool despawnAllItems)
        {

            //Early exit if we're not even supposed to run
            if (Plugin.Cfg.ScrapProtection == ProtectionType.SAVE_NONE || despawnAllItems)
                return true;

            //Fixes non hosts getting errors when trying to destroy items
            if (__instance.IsHost || __instance.IsServer)
            {
                Plugin.Log.LogInfo("ProtectionPatch -> " + despawnAllItems + " : " + StartOfRound.Instance.allPlayersDead.ToString());

                //check if we're needed at all
                if (StartOfRound.Instance.allPlayersDead)
                {
                    //There should probably be a host check here but roundmanager uses
                    //base.ishost and I dunno what to make of this right now.

                    GrabbableObject[] allItems = GameObject.FindObjectsOfType<GrabbableObject>();

                    //I don't know if the host/client sync despawned objects but using the
                    //same seed should make absolutely sure they do
                    rng = new System.Random(StartOfRound.Instance.randomMapSeed + 83);

                    void DeleteItem(GrabbableObject item)
                    {
                        Plugin.Log.LogInfo("Despawning item: "+ item.name);

                                //despawn network item
                                item.gameObject.GetComponent<NetworkObject>().Despawn();

                                //destroy synced object
                                if (__instance.spawnedSyncedObjects.Contains(item.gameObject))
                                {
                                    __instance.spawnedSyncedObjects.Remove(item.gameObject);
                                }
                    }

                    foreach (GrabbableObject item in allItems)
                    {

                        ProtectionType prot = Plugin.Cfg.ScrapProtection;

                        //is this an item that would normally be destroyed after a failed round?
                        if (item.itemProperties.isScrap)
                        {
                            if (item.isInShipRoom && ShouldSaveScrap(prot))
                            {
                                Plugin.Log.LogInfo("Saving scrap item " + item.name);
                            }
                            else
                            {
                                DeleteItem(item);   
                            }
                        }
                        else
                        {
                            DeleteItem(item);
                        }
                            
                    }

                    //destroy temp effects since we're skipping the destroy function
                    GameObject[] tempEffects = GameObject.FindGameObjectsWithTag("TemporaryEffect");
                    for (int i = 0; i < tempEffects.Length; i++)
                    {
                        Object.Destroy(tempEffects[i]);
                    }

                    //this might could maybe break things because I don't implement the
                    //destroy loop the same as the base game, I dunno.
                    return false;
                }
            }

            return true;
        }

        public static bool ShouldSaveScrap(ProtectionType pType)
        {
            switch (pType)
            {
                case ProtectionType.SAVE_ALL:
                    return true;
                case ProtectionType.SAVE_COINFLIP:
                    return rng.NextDouble() > 0.49;
                default:
                    return false;
            }
        }
    }
}