using HarmonyLib;

[HarmonyPatch(typeof(GameManager), "updateTimeOfDay")]
public class ModManagerTestModPatch
{
    private static bool LastLoggedNight = false;

    public static bool Prefix()
    {
        if (!ConnectionManager.Instance.IsServer)
        {
            return true;
        }

        bool isNight = GameManager.Instance.World.IsDark();

        if (isNight && !LastLoggedNight)
        {
            Log.Warning($"ModManagerTest: It's night");
            LastLoggedNight = true;
        }

        if (!isNight && LastLoggedNight)
        {
            Log.Warning($"ModManagerTest: It's day");
            LastLoggedNight = false;
            return true;
        }

        return true;
    }
}
