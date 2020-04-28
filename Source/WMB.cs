using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace SirRandoo.WheresMyBed
{
    public class Wmb : Mod
    {
        private static Settings _settings;

        public Wmb(ModContentPack content) : base(content)
        {
            Log.Message("Where's My Bed :: Have you seen my bed?");

            var harmony = new Harmony("sirrandoo.wmb");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            _settings = GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Settings.Draw(inRect);
        }

        public override string SettingsCategory()
        {
            return "Where's My Bed";
        }
    }

    [StaticConstructorOnStartup]
    public static class WmbStatic
    {
        internal static Texture2D GizmoIcon = ContentFinder<Texture2D>.Get("UI/Icons/WMB_Gizmo");
    }
}
