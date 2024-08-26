using System;

namespace Lethal_Company_Enhancer.Utils;

public static class SyncCompleted
{

    internal static bool Completed = false;

    public static void SyncCompletedCallback(object Sender, EventArgs e)
    {
        Plugin.Log.LogInfo("Config sync'd");
        Completed = true;
    }
}
