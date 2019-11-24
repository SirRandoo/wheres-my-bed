using System.Collections.Generic;

using Harmony;

using RimWorld;

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
            if(__instance != null && __instance.IsColonistPlayerControlled)
            {
                foreach(var map in Find.Maps)
                {
                    if(!map.IsPlayerHome)
                        continue;

                    foreach(var thing in map.listerThings.AllThings)
                    {
                        if(!(thing is Building_Bed))
                            continue;
                        var bed = (Building_Bed) thing;

                        if(bed.owners.Contains(__instance))
                        {
                            __result = __result.Add(new Command_Action
                            {
                                defaultLabel = "WMB.Gizmo.Label".Translate(),
                                defaultDesc = "WMB.Gizmo.Description".Translate(),
                                icon = ContentFinder<Texture2D>.Get("UI/Icons/WMB_Gizmo"),
                                activateSound = SoundDef.Named("Click"),
                                action = delegate { CameraJumper.TryJumpAndSelect(bed); }
                            });
                            return;
                        }
                    }
                }
            }
        }
    }
}
