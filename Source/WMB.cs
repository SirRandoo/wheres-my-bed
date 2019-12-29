using System.Reflection;

using Harmony;

using UnityEngine;

using Verse;

namespace SirRandoo.WheresMyBed
{
    public class WMB : Mod
    {
        internal static HarmonyInstance Harmony;
        public static Settings Settings;

        public WMB(ModContentPack content) : base(content)
        {
            Log.Message("Where's My Bed :: Have you seen my bed?");

            Harmony = HarmonyInstance.Create("sirrandoo.wmb");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());

            Settings = GetSettings<Settings>();
        }

        public override string SettingsCategory() => "Where's My Bed";
        public override void DoSettingsWindowContents(Rect inRect) => Settings.Draw(inRect);
    }
}
