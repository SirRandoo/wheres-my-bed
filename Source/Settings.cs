using SirRandoo.WheresMyBed.Utils;
using UnityEngine;
using Verse;

namespace SirRandoo.WheresMyBed
{
    public enum Actions
    {
        Select, Jump
        //Arrow 
    }

    public class Settings : ModSettings
    {
        public static bool ShowGizmoText = true;
        public static string GizmoAction = Actions.Select.ToString();

        public static void Draw(Rect canvas)
        {
            GUI.BeginGroup(canvas);
            var panel = new Listing_Standard();
            panel.Begin(canvas);

            panel.Label("WMB.Groups.Gizmo.Label".Translate(), tooltip: "WMB.Groups.Gizmo.Tooltip");
            panel.GapLine();
            panel.ComboBox(
                "WMB.Settings.Gizmo.ActionType.Label".Translate(),
                typeof(Actions),
                ref GizmoAction,
                var => GizmoAction = var,
                "WMB.Settings.Gizmo.ActionType.Tooltip".Translate()
            );
            panel.CheckboxLabeled(
                "WMB.Settings.Gizmo.TextEnabled.Label".Translate(),
                ref ShowGizmoText,
                "WMB.Settings.Gizmo.TextEnabled.Tooltip".Translate()
            );

            panel.End();
            GUI.EndGroup();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref ShowGizmoText, "drawText", true);
            Scribe_Values.Look(ref GizmoAction, "action", Actions.Select.ToString());
        }
    }
}
