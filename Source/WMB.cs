using System.Reflection;

using HarmonyLib;

using UnityEngine;

using Verse;

namespace SirRandoo.WheresMyBed
{
    public class WMB : Mod
    {
        public static Settings Settings;
        internal static Harmony Harmony;

        public WMB(ModContentPack content) : base(content)
        {
            Log.Message("Where's My Bed :: Have you seen my bed?");

            Harmony = new Harmony("sirrandoo.wmb");
            Harmony.PatchAll(Assembly.GetExecutingAssembly());

            Settings = GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect) => Settings.Draw(inRect);

        public override string SettingsCategory() => "Where's My Bed";
    }
}
