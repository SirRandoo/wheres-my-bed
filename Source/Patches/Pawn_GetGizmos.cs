using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using Verse;

namespace SirRandoo.WheresMyBed.Patches
{
    [HarmonyPatch(typeof(Pawn), "GetGizmos")]
    public static class PawnGetGizmos
    {
        [HarmonyPostfix]
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public static void GetGizmos(Pawn __instance, ref IEnumerable<Gizmo> __result)
        {
            if (!__instance?.IsColonistPlayerControlled ?? true)
            {
                return;
            }
            
            if (__instance.ownership?.OwnedBed == null)
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
                        if (!CameraJumper.CanJump(__instance.ownership.OwnedBed))
                        {
                            return;
                        }
                        
                        switch (Settings.GizmoAction)
                        {
                            case "Jump":
                                CameraJumper.TryJump(__instance.ownership.OwnedBed);
                                break;

                            case "Select":
                                CameraJumper.TryJumpAndSelect(__instance.ownership.OwnedBed);
                                break;
                        }
                    }
                }
            );
        }
    }
}
