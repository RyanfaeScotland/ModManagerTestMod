using CustomModManager.API;
using System.Reflection;
using static CustomModManager.API.ModManagerAPI.ModSettings;

class ModManagerTestMod : IModApi
{
    private int test123;  // Note: This value will be the default setting when reset.
    private ModSetting<int> test123M;
    public void InitMod(Mod _modInstance)
    {
        var harmony = new HarmonyLib.Harmony(GetType().ToString());
        Log.Warning($"{GetType()}: Patching...");

        harmony.PatchAll(Assembly.GetExecutingAssembly());

        if (ModManagerAPI.IsModManagerLoaded())
        {
            ModManagerAPI.ModSettings settings = ModManagerAPI.GetModSettings(_modInstance);
            test123M = settings.Hook(
                "test123",                                     // This is the Mod Setting key. This must always be unique for all settings for the mod.
                "xuiModSettingTest123",                        // This is the localization key for the setting's label. The value is fetched from the Localization.txt file in your mod's Config folder.
                value => { this.test123 = value; },                 // This is the value setter, it updates the value of the variable we have hooked onto.
                () => this.test123,                            // This is the value getter, it gets the value of the variable we have hooked onto.
                toStr => { return (toStr.ToString(), toStr.ToString()); }, // This is the string representation of the currently applied setting.
                str =>                                         // This is the converter from the String representation of the setting, back to the variable type of the setting.
                    {
                        bool success = int.TryParse(str, out int val); // Attempt to convert the input to an integer. 
                                                                       // If success is true, the input will be accepted.
                                                                       // If success is false, the input will be rejected, and the text will change to red to indicate
                                                                       // to the user that an invalid input has been entered and will not be saved.
                        return (val, success);
                    }
             );

            test123M.SetAllowedValues(new int[] { 0, 5, 10, 15, 20, 25, 30 });
            test123M.SetWrap(true);
        }
    }
}

