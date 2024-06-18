// MIT License
//
// Copyright (c) 2024 SirRandoo
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using RimWorld;
using SirRandoo.WheresMyBed.DropdownDialogs;
using UnityEngine;
using Verse;

namespace SirRandoo.WheresMyBed;

/// <summary>
///     A specialized class for drawing dropdown menus.
/// </summary>
public static class DropdownDrawer
{
    /// <summary>
    ///     Draws a dropdown at the given region.
    /// </summary>
    /// <param name="region">The region to draw the dropdown in.</param>
    /// <param name="current">
    ///     The current item being displayed in the dropdown display.
    /// </param>
    /// <param name="allOptions">
    ///     A list containing all the available options a user can select.
    /// </param>
    /// <param name="setterFunc">
    ///     An action that's called when the user selects an option in the
    ///     dropdown menu.
    /// </param>
    public static void Draw(Rect region, Actions current, Actions[] allOptions, Action<Actions> setterFunc)
    {
        if (DrawButton(region, current.ToStringFast()))
        {
            Find.WindowStack.Add(new DropdownDialog(GetDialogPosition(region), current, allOptions, setterFunc));
        }
    }

    private static Rect GetDialogPosition(Rect parentRegion) => UI.GUIToScreenRect(parentRegion);

    public static bool DrawButton(Rect region, string label)
    {
        DrawDecorativeButton(region, label);

        return Widgets.ButtonInvisible(region);
    }

    public static void DrawDecorativeButton(Rect region, string label)
    {
        var labelRegion = new Rect(region.x + 5f, region.y, region.width - region.height, region.height);
        Rect iconRegion = LayoutHelper.IconRect(region.x + region.width - region.height, region.y, region.height, region.height, UiConstants.LineHeight * 0.3f);

        Widgets.DrawLightHighlight(region);

        BoxDrawer.DrawBox(region, color: Color.grey);
        LabelDrawer.DrawLabel(labelRegion, label);

        GUI.DrawTexture(iconRegion, TexButton.ReorderDown);

        Widgets.DrawHighlightIfMouseover(region);
    }
}
