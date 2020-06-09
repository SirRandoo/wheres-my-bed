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
        internal static Texture2D GizmoIcon;
        internal static SoundDef SoundDef;

        static WmbStatic()
        {
            GizmoIcon = ContentFinder<Texture2D>.Get("UI/Icons/WMB_Gizmo");
            SoundDef = SoundDef.Named("Click");
        }
    }
}
