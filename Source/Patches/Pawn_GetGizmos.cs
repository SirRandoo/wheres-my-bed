using System;
using System.Collections.Generic;

using HarmonyLib;

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

            __result = __result.AddItem(new Command_Action
            {
                defaultLabel = Settings.ShowGizmoText ? "WMB.Gizmo.Label".Translate() : null,
                defaultDesc = "WMB.Gizmo.Description".Translate(),
                icon = WmbStatic.GizmoIcon,
                activateSound = SoundDef.Named("Click"),
                action = delegate
                {
                    switch(Enum.Parse(typeof(Actions), Settings.GizmoAction))
                    {
                        //case Actions.Arrow:
                        //    LookTargetsUtility.TryHighlight(__instance.ownership.OwnedBed);
                        //    break;

                        case Actions.Jump:
                            if(CameraJumper.CanJump(__instance.ownership.OwnedBed)) CameraJumper.TryJump(__instance.ownership.OwnedBed);
                            break;

                        case Actions.Select:
                            if(CameraJumper.CanJump(__instance.ownership.OwnedBed)) CameraJumper.TryJumpAndSelect(__instance.ownership.OwnedBed);
                            break;
                    }
                }
            });
        }
    }
}
