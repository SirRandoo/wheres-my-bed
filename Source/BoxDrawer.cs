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

using UnityEngine;
using Verse;

namespace SirRandoo.WheresMyBed;

public static class BoxDrawer
{
    public static void DrawBox(Rect region, int thickness = 1, Color? color = null)
    {
        var drawRegion = new Rect();
        Color previousColor = GUI.color;

        float thicknessForUiScale = Prefs.UIScale * thickness;
        float finalThickness = Mathf.Floor(thickness - (thicknessForUiScale - Mathf.Ceil(thicknessForUiScale)) / Prefs.UIScale);

        if (color != null)
        {
            GUI.color = color.Value;
        }

        // Draw left side
        RectExtensions.SetX(ref drawRegion, region.x);
        RectExtensions.SetY(ref drawRegion, region.y);
        RectExtensions.SetWidth(ref drawRegion, finalThickness);
        RectExtensions.SetHeight(ref drawRegion, region.height);
        GUI.DrawTexture(drawRegion, BaseContent.WhiteTex);

        // Draw right side
        RectExtensions.SetX(ref drawRegion, region.x + region.width - 1);
        GUI.DrawTexture(drawRegion, BaseContent.WhiteTex);

        // Draw top side
        RectExtensions.SetX(ref drawRegion, region.x);
        RectExtensions.SetHeight(ref drawRegion, finalThickness);
        RectExtensions.SetWidth(ref drawRegion, region.width);
        GUI.DrawTexture(drawRegion, BaseContent.WhiteTex);

        // Draw bottom side
        RectExtensions.SetY(ref drawRegion, region.y + region.height - 1);
        GUI.DrawTexture(drawRegion, BaseContent.WhiteTex);

        if (color != null)
        {
            GUI.color = previousColor;
        }
    }
}
