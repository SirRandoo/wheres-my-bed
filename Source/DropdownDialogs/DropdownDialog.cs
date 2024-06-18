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
using UnityEngine;
using Verse;

namespace SirRandoo.WheresMyBed.DropdownDialogs;

internal class DropdownDialog : Window
{
    private const int TotalDisplayableOptions = 9;
    private readonly Action<Actions> _setter;

    private readonly int _totalOptions;
    protected readonly Actions? CurrentOption;
    protected readonly Rect DropdownRegion;
    private Vector2 _scrollPos = new(0f, 0f);
    protected bool IsReversed;
    protected float ViewHeight;

    protected internal DropdownDialog(Rect parentRegion, Actions current, Actions[] allOptions, Action<Actions> setter)
    {
        _setter = setter;
        Options = allOptions;
        CurrentOption = current;
        DropdownRegion = parentRegion;
        _totalOptions = allOptions.Length;

        doCloseX = false;
        drawShadow = false;
        doCloseButton = false;
        layer = WindowLayer.Dialog;

        closeOnClickedOutside = true;
        absorbInputAroundWindow = true;
        forceCatchAcceptAndCancelEventEvenIfUnfocused = true;
    }

    /// <inheritdoc />
    protected override float Margin => 5f;

    public IReadOnlyList<Actions> Options { get; set; }

    /// <inheritdoc />
    protected override void SetInitialSizeAndPosition()
    {
        float yPosition = IsReversed ? DropdownRegion.y - ViewHeight - 10f : DropdownRegion.y + DropdownRegion.height + 5f;

        windowRect = new Rect(DropdownRegion.x, yPosition, DropdownRegion.width, ViewHeight);
        windowRect = windowRect.ExpandedBy(5f);
    }

    /// <inheritdoc />
    public override void DoWindowContents(Rect inRect)
    {
        var dropdownOptionsRegion = new Rect(0f, 0f, inRect.width, inRect.height);

        GUI.BeginGroup(inRect);

        Widgets.DrawLightHighlight(dropdownOptionsRegion);
        Widgets.DrawLightHighlight(dropdownOptionsRegion);
        Widgets.DrawLightHighlight(dropdownOptionsRegion);

        GUI.BeginGroup(dropdownOptionsRegion);
        DrawDropdownOptions(dropdownOptionsRegion.AtZero());
        GUI.EndGroup();

        GUI.EndGroup();
    }

    protected void DrawDropdownOptions(Rect region)
    {
        float viewportWidth = region.width - (_totalOptions > TotalDisplayableOptions ? 16f : 0f);
        var viewport = new Rect(0f, 0f, viewportWidth, UiConstants.LineHeight * _totalOptions);

        GUI.BeginGroup(region);
        _scrollPos = GUI.BeginScrollView(region.AtZero(), _scrollPos, viewport);

        for (var i = 0; i < _totalOptions; i++)
        {
            var lineRegion = new Rect(0f, UiConstants.LineHeight * i, viewportWidth, UiConstants.LineHeight);

            if (!lineRegion.IsVisible(viewport, _scrollPos))
            {
                continue;
            }

            Actions item = Options[i];
            var textRegion = new Rect(5f, lineRegion.y, lineRegion.width - 10f, lineRegion.height);

            LabelDrawer.DrawLabel(textRegion, item.ToStringFast());
            Widgets.DrawHighlightIfMouseover(lineRegion);

            if (item == CurrentOption)
            {
                Widgets.DrawHighlightSelected(lineRegion);
            }

            if (i % 2 == 0)
            {
                GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.44f);

                Widgets.DrawLineHorizontal(lineRegion.x, lineRegion.y, lineRegion.width);
                Widgets.DrawLineHorizontal(lineRegion.x, lineRegion.y + lineRegion.height, lineRegion.width);

                GUI.color = Color.white;
            }

            if (Widgets.ButtonInvisible(lineRegion, false))
            {
                _setter(item);

                Close();
            }
        }

        GUI.EndScrollView();
        GUI.EndGroup();
    }

    /// <inheritdoc />
    public override void PreOpen()
    {
        ViewHeight = UiConstants.LineHeight * (_totalOptions > TotalDisplayableOptions ? TotalDisplayableOptions : _totalOptions);
        IsReversed = DropdownRegion.y + ViewHeight > UI.screenHeight;

        base.PreOpen();
    }

    /// <inheritdoc />
    public override void PostOpen()
    {
        if (Options.Count <= 0)
        {
            Log.Error("[Where's My Bed] Attempted to create a dropdown menu with no options.");

            Close(false);

            return;
        }

        if (CurrentOption != null)
        {
            ScrollToItem(CurrentOption.Value);
        }
    }

    private void ScrollToItem(Actions item)
    {
        float totalViewHeight = _totalOptions * UiConstants.LineHeight;

        for (var i = 0; i < _totalOptions; i++)
        {
            if (Options[i] == item)
            {
                continue;
            }

            int startingPageItem = i - Mathf.FloorToInt(TotalDisplayableOptions / 2f) - 1;

            _scrollPos = new Vector2(0f, Mathf.Clamp(startingPageItem * UiConstants.LineHeight, 0f, totalViewHeight));
        }
    }
}
