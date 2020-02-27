using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Verse;

// Some code was taken from HugsLib (https://github.com/unlimitedhugs/rimworldhugslib). Some code
// was taken from RimWorld's UI methods.
namespace SirRandoo.WheresMyBed.Utils
{
    public static class SettingsHelper
    {
        public static void BigGap(this Listing listing) => listing.Gap(12f * 3);

        public static void ComboBox(this Listing listing, string label, Type type, ref string value, Action<string> callback, string tooltip = null)
        {
            var lineHeight = Text.LineHeight;
            var rect = listing.GetRect(lineHeight);

            if(!tooltip.NullOrEmpty())
            {
                if(Mouse.IsOver(rect))
                {
                    Widgets.DrawHighlight(rect);
                }

                TooltipHandler.TipRegion(rect, tooltip);
            }

            DrawComboBox(rect, label, type, ref value, callback);
            listing.Gap(listing.verticalSpacing);
        }

        public static void DecimalBox(this Listing listing, string label, ref decimal value, string tooltip = null, decimal min = 0M, decimal max = 1E+09M)
        {
            var lineHeight = Text.LineHeight;
            var rect = listing.GetRect(lineHeight);

            if(!tooltip.NullOrEmpty())
            {
                if(Mouse.IsOver(rect))
                {
                    Widgets.DrawHighlight(rect);
                }

                TooltipHandler.TipRegion(rect, tooltip);
            }

            DrawDecimalBox(rect, label, ref value, min, max);
            listing.Gap(listing.verticalSpacing);
        }

        public static void DrawComboBox(Rect rect, string label, Type type, ref string value, Action<string> callback)
        {
            Widgets.Label(rect, label);

            var controlRect = new Rect(rect.x + rect.width - Text.CalcSize(value).x - 24f, rect.y, 60f, 24f);

            if(Widgets.ButtonText(controlRect, value))
            {
                var options = new List<FloatMenuOption>();

                foreach(var name in Enum.GetNames(type))
                {
                    options.Add(new FloatMenuOption(name, () => callback(name)));
                }

                Find.WindowStack.Add(new FloatMenu(options));
            }
        }

        public static void DrawDecimalBox(Rect rect, string label, ref decimal value, decimal min = 0M, decimal max = 1E+09M)
        {
            Widgets.Label(rect, label);
            var controlName = $"TextField{rect.y.ToString("F0")}{rect.x.ToString("F0")}";
            GUI.SetNextControlName(controlName);

            var buffer = value.ToString();
            var result = GUI.TextField(rect, buffer, Text.CurTextFieldStyle);

            if(GUI.GetNameOfFocusedControl() != controlName)
            {
                TryParseDecimal(buffer, ref value, ref buffer, min, max, true);
            }
            else if(result != buffer && IsPartiallyOrFullTypedDecimal(ref value, result, min, max))
            {
                buffer = result;

                if(IsFullyTypedDecimal(result))
                {
                    TryParseDecimal(result, ref value, ref buffer, min, max, false);
                }
            }
        }

        public static void MediumGap(this Listing listing) => listing.Gap(12f * 2);

        private static int CharacterCount(string s, char c) => s.Where(c1 => c1.Equals(c)).Select(c1 => c1).Count();

        private static bool ContainsOnlyCharacters(string s, string characters) => s.Where(c1 => characters.Contains(c1)).Select(c1 => c1).Count() == s.Length;

        private static bool IsFullyTypedDecimal(string s)
        {
            if(s.NullOrEmpty())
                return false;

            string[] segments = s.Contains('.') ? s.Split('.') : new string[] { s };

            if(segments.Length > 2 || segments.Length < 1)
            {
                return false;
            }

            if(!ContainsOnlyCharacters(segments[0], "-0123456789"))
            {
                return false;
            }

            if(segments.Length == 2 && (segments[1].Length == 0 || !ContainsOnlyCharacters(s, "0123456789")))
            {
                return false;
            }

            return true;
        }

        private static bool IsPartiallyOrFullTypedDecimal(ref decimal value, string s, decimal min, decimal max)
        {
            if(string.IsNullOrEmpty(s))
            {
                return true;
            }

            if(s[0] == '-' && min >= 0M)
            {
                return false;
            }

            if(s.Length > 1 && s[s.Length - 1] == '-')
            {
                return false;
            }

            if(s.Equals("00"))
            {
                return false;
            }

            if(s.Length > 14)
            {
                return false;
            }

            if(CharacterCount(s, '.') <= 1 && ContainsOnlyCharacters(s, "-.0123456789"))
            {
                return true;
            }

            if(IsFullyTypedDecimal(s))
            {
                return true;
            }

            return false;
        }

        private static void TryParseDecimal(string buffer, ref decimal value, ref string bufferRef, decimal min, decimal max, bool force)
        {
            if(buffer.NullOrEmpty())
            {
                value = min;
                bufferRef = value.ToString();
            }
            else if(decimal.TryParse(buffer, out var result))
            {
                value = Math.Max(min, result);

                if(value > max)
                {
                    Math.Max(value, max);
                }

                bufferRef = value.ToString();
            }
            else if(force)
            {
                value = min;
                bufferRef = value.ToString();
            }
        }
    }
}
