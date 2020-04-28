using SirRandoo.WheresMyBed.Utils;

using UnityEngine;

using Verse;

namespace SirRandoo.WheresMyBed
{
    public enum Actions
    {
        Select,
        Jump,
        //Arrow 
    }

    public class Settings : ModSettings
    {
        private static Vector2 _scrollPos = Vector2.zero;

        public static bool ShowGizmoText = true;
        public static string GizmoAction = Actions.Select.ToString();

        public static void Draw(Rect canvas)
        {
            var panel = new Listing_Standard();

            var view = new Rect(0f, 0f, canvas.width, 36f * 26f);
            view.xMax *= 0.9f;

            panel.BeginScrollView(canvas, ref _scrollPos, ref view);

            panel.Gap();
            panel.Label("WMB.Groups.Gizmo.Label".Translate(), tooltip: "WMB.Groups.Gizmo.Tooltip");
            panel.GapLine();
            panel.CheckboxLabeled("WMB.Settings.Gizmo.TextEnabled.Label".Translate(), ref ShowGizmoText, "WMB.Settings.Gizmo.TextEnabled.Tooltip".Translate());
            panel.ComboBox("WMB.Settings.Gizmo.ActionType.Label".Translate(), typeof(Actions), ref GizmoAction, (var) => GizmoAction = var, "WMB.Settings.Gizmo.ActionType.Tooltip".Translate());

            panel.EndScrollView(ref view);
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref ShowGizmoText, "drawText", true);
            Scribe_Values.Look(ref GizmoAction, "action", Actions.Select.ToString());
        }
    }
}
