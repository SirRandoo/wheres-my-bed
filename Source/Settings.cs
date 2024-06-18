using NetEscapades.EnumGenerators;
using UnityEngine;
using Verse;

namespace SirRandoo.WheresMyBed;

[EnumExtensions]
public enum Actions
{
    Select, Jump

    //Arrow
}

public class Settings : ModSettings
{
    public static bool ShowGizmoText = true;
    public static Actions GizmoAction = Actions.Select;
    private static string _gizmoActionString = GizmoAction.ToStringFast();

    public static void Draw(Rect canvas)
    {
        GUI.BeginGroup(canvas);

        var panel = new Listing_Standard();
        panel.Begin(canvas);

        Rect actionTypeRegion = panel.GetRect(UiConstants.LineHeight);
        (Rect labelRegion, Rect fieldRegion) = actionTypeRegion.Split();

        LabelDrawer.DrawLabel(labelRegion, "WMB.Settings.Gizmo.ActionType.Label".TranslateSimple());
        TooltipHandler.TipRegion(actionTypeRegion, "WMB.Settings.Gizmo.ActionTYpe.Tooltip".TranslateSimple());

        DropdownDrawer.Draw(
            fieldRegion,
            GizmoAction,
            WmbStatic.GizmoActions,
            actions =>
            {
                GizmoAction = actions;
                _gizmoActionString = actions.ToStringFast();
            }
        );


        Rect textEnabledRegion = panel.GetRect(UiConstants.LineHeight);
        (Rect textLabelRegion, Rect textFieldRegion) = textEnabledRegion.Split();

        LabelDrawer.DrawLabel(textLabelRegion, "WMB.Settings.Gizmo.TextEnabled.Label".TranslateSimple());
        TooltipHandler.TipRegion(textEnabledRegion, "WMB.Settings.Gizmo.TextEnabled.Tooltip".TranslateSimple());

        CheckboxDrawer.DrawCheckbox(textFieldRegion, ref ShowGizmoText);

        panel.End();
        GUI.EndGroup();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref ShowGizmoText, "drawText", true);
        Scribe_Values.Look(ref _gizmoActionString, "action", Actions.Select.ToStringFast());

        if (Scribe.mode == LoadSaveMode.PostLoadInit && !ActionsExtensions.TryParse(_gizmoActionString, out GizmoAction))
        {
            GizmoAction = Actions.Select;
        }
    }
}
