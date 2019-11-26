using System.Collections.Generic;

using Harmony;

using UnityEngine;

using Verse;

namespace SirRandoo.WheresMyBed.Patches
{
    [HarmonyPatch(typeof(Pawn), "GetGizmos")]
    public static class Pawn_GetGizmos__Postfix
    {
        [HarmonyPostfix]
        public static void GetGizmos(Pawn __instance, ref IEnumerable<Gizmo> __result)
        {
            if(__instance == null || __instance.ownership == null || __instance.ownership.OwnedBed == null)
                return;
            if(!__instance.IsColonistPlayerControlled)
                return;

            __result = __result.Add(new Command_Action
            {
                defaultLabel = "WMB.Gizmo.Label".Translate(),
                defaultDesc = "WMB.Gizmo.Description".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/Icons/WMB_Gizmo"),
                activateSound = SoundDef.Named("Click"),
                action = delegate { CameraJumper.TryJumpAndSelect(__instance.ownership.OwnedBed); }
            });
        }
    }
}
