using System.Reflection;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.DialogSystem;
using Kingmaker.DialogSystem.Blueprints;
using UnityModManagerNet;

namespace ParadoxDeparadoxified
{
    public class Main
    {
        public static bool Enabled;
        public static UnityModManager.ModEntry mod;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            mod = modEntry;
            modEntry.OnToggle = OnToggle;
            return true;
        }
        
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            Enabled = value;
            return true;
        }
        
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool loaded = false;
            
            static void Postfix()
            {
                if (loaded) return;
                loaded = true;
                
                PatchAnswer("9fc697452dfd3014b8282acf84973e4d"); // World
                PatchAnswer("cde488feccbeb1b4094691e9bd607e9d"); // Arcana
                PatchAnswer("4092d0d8beaff2c4891e15ef01e04723"); // Nature
                PatchAnswer("d4f41f12693cd6d4b864ea1c39cdbb57"); // Religion
                PatchAnswer("aa68a92b209268b498e249605c5ff2aa"); // Perception
            }

            static void PatchAnswer(string id)
            {
                var answer = ResourcesLibrary.TryGetBlueprint<BlueprintAnswer>(id);
                if (answer != null)
                {
                    answer.CharacterSelection.SelectionType = CharacterSelection.Type.Clear;
                    mod.Logger.Log($"Patched Answer: {id}");
                }
            }
        }
    }
}