using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using JetBrains.Annotations;
using Verse;

namespace SirRandoo.WheresMyBed.Patches;

[PublicAPI]
[HarmonyPatch(typeof(Pawn), "GetGizmos")]
internal static class PawnGetGizmos
{
    [HarmonyPostfix]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static void GetGizmos(Pawn? __instance, ref IEnumerable<Gizmo> __result)
    {
        if (__instance is not { IsColonistPlayerControlled: true, ownership.OwnedBed: not null })
        {
            return;
        }

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
                        case Actions.Jump:
                            CameraJumper.TryJump(__instance.ownership.OwnedBed);

                            break;

                        case Actions.Select:
                            CameraJumper.TryJumpAndSelect(__instance.ownership.OwnedBed);

                            break;
                    }
                }
            }
        );
    }
}
