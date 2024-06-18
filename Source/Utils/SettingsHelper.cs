using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

// Some code was taken from HugsLib (https://github.com/unlimitedhugs/rimworldhugslib). Some code
// was taken from RimWorld's UI methods.
namespace SirRandoo.WheresMyBed.Utils;

public static class SettingsHelper
{
    public static void ComboBox(this Listing listing, string label, Type type, ref string value, Action<string> callback, string? tooltip = null)
    {
        float lineHeight = Text.LineHeight;
        Rect rect = listing.GetRect(lineHeight);

        if (!tooltip.NullOrEmpty())
        {
            if (Mouse.IsOver(rect))
            {
                Widgets.DrawHighlight(rect);
            }

            TooltipHandler.TipRegion(rect, tooltip);
        }

        DrawComboBox(rect, label, type, ref value, callback);
        listing.Gap(listing.verticalSpacing);
    }

    private static void DrawComboBox(Rect rect, string label, Type type, ref string value, Action<string> callback)
    {
        Widgets.Label(rect, label);

        var controlRect = new Rect(rect.x + rect.width - Text.CalcSize(value).x - 24f, rect.y, 60f, 24f);

        if (!Widgets.ButtonText(controlRect, value))
        {
            return;
        }

        var options = new List<FloatMenuOption>();

        foreach (string name in Enum.GetNames(type))
        {
            options.Add(new FloatMenuOption(name, () => callback(name)));
        }

        Find.WindowStack.Add(new FloatMenu(options));
    }
}
