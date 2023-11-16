using UnityEngine;
using HarmonyLib;
using Unity.Netcode;

namespace Enhancer
{
    public class SPProtectionPatches
    {

        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.DespawnPropsAtEndOfRound))]
        [HarmonyPrefix]
        public static bool ProtectionPrefix(RoundManager __instance, bool despawnAllItems)
        {

            Plugin.Log.LogInfo("ProtectionPatch -> " + despawnAllItems + " : " + StartOfRound.Instance.allPlayersDead.ToString());

            //check if we're needed at all
            if (despawnAllItems || StartOfRound.Instance.allPlayersDead)
            {

                //There should probably be a host check here but roundmanager uses
                //base.ishost and I dunno what to make of the right now.

                GrabbableObject[] allItems = GameObject.FindObjectsOfType<GrabbableObject>();

                foreach (GrabbableObject item in allItems)
                {
                    //is this an item that would normally be destroyed after a failed round?
                    if (item.itemProperties.isScrap)
                        if (item.isInShipRoom)
                        {
                            Plugin.Log.LogInfo("Saving scrap item " + item.name);
                        }
                        else
                        {
                            //despawn network item
                            item.gameObject.GetComponent<NetworkObject>().Despawn();

                            //destroy synced object
                            if (__instance.spawnedSyncedObjects.Contains(item.gameObject))
                            {
                                __instance.spawnedSyncedObjects.Remove(item.gameObject);
                            }
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

            return true;
        }
    }
}