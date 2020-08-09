using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Verse;

namespace SirRandoo.WheresMyBed.Patches
{
    [HarmonyPatch(typeof(Pawn), "GetGizmos")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class PawnGetGizmos
    {
        [HarmonyPostfix]
        public static void GetGizmos(Pawn __instance, ref IEnumerable<Gizmo> __result)
        {
            if (__instance?.ownership?.OwnedBed == null)
            {
                return;
            }

            WmbStatic.GizmoLabel ??= "WMB.Gizmo.Label".TranslateSimple();
            WmbStatic.GizmoDescription ??= "WMB.Gizmo.Description".TranslateSimple();

            __result = __result.AddItem(
                new Command_Action
                {
                    defaultLabel = Settings.ShowGizmoText ? WmbStatic.GizmoLabel : null,
                    defaultDesc = WmbStatic.GizmoDescription,
                    icon = WmbStatic.GizmoIcon,
                    activateSound = WmbStatic.SoundDef,
                    action = delegate
                    {
                        switch (Settings.GizmoAction)
                        {
                            //case Actions.Arrow:
                            //    LookTargetsUtility.TryHighlight(__instance.ownership.OwnedBed);
                            //    break;

                            case "Jump":
                                if (CameraJumper.CanJump(__instance.ownership.OwnedBed))
                                {
                                    CameraJumper.TryJump(__instance.ownership.OwnedBed);
                                }

                                break;

                            case "Select":
                                if (CameraJumper.CanJump(__instance.ownership.OwnedBed))
                                {
                                    CameraJumper.TryJumpAndSelect(__instance.ownership.OwnedBed);
                                }

                                break;
                        }
                    }
                }
            );
        }
    }
}
