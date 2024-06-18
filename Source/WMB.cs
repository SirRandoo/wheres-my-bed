using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace SirRandoo.WheresMyBed;

[PublicAPI]
public class Wmb : Mod
{
    public Wmb(ModContentPack content) : base(content)
    {
        var harmony = new Harmony("sirrandoo.wmb");
        harmony.PatchAll(Assembly.GetExecutingAssembly());

        GetSettings<Settings>();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Settings.Draw(inRect);
    }

    public override string SettingsCategory() => Content.Name;
}
