using HarmonyLib;

namespace Enhancer.Patches;

public static class ImprovedScanCommand
{
    //Todo: This should probably be changed to a postfix on the text modifier
    //function so I can add custom tags to terminal nodes
    [HarmonyPatch(typeof(Terminal), nameof(Terminal.LoadNewNode))]
    [HarmonyPostfix]
    public static void TerminalLoadHackPost(Terminal __instance, TerminalNode node)
    {
        //Inject data into the scan command
        if (node.name == "ScanInfo")
        {

            if (Plugin.BoundConfig.ThreatScannerType == 0)
                return;

            if (RoundManager.Instance.currentLevel.spawnEnemiesAndScrap)
            {
                /*
                    We cache these values (and the ones in the switch below) because
                    The actual in-game terminal crashes when accessing RoundManager
                    sometimes and I don't know why but this configuration works

                    Recommendation: Do not modify this function ever, it is a headache
                */
                int power = RoundManager.Instance.currentEnemyPower;
                int maxp = RoundManager.Instance.currentLevel.maxEnemyPowerCount;
                string threatString = "\nThreat Level: ";

                switch (Plugin.BoundConfig.ThreatScannerType)
                {
                    case 1:
                        int contacts = RoundManager.Instance.numberOfEnemiesInScene;
                        threatString = "\nHostile Contacts: " + contacts.ToString();
                        break;
                    case 2:
                        threatString += ((float)power / maxp * 100).ToString("N1") + "%";
                        break;
                    case 3:
                        string desc = "CLEAR";
                        float threatLevel = (float)power / maxp;

                        if (threatLevel > 0.99f)
                            desc = "OMEGA";
                        else if (threatLevel > 0.69f)
                            desc = "RED";
                        else if (threatLevel > 0.49)
                            desc = "ORANGE";
                        else if (threatLevel > 0.24)
                            desc = "YELLOW";
                        else if (threatLevel > 0)
                            desc = "GREEN";

                        threatString += desc;
                        break;

                }

                threatString += "\n";

                __instance.screenText.text += threatString;
                __instance.currentText = __instance.screenText.text;
                __instance.textAdded = 0;
            }
        }
    }
}