using System;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Krotovukha_Game.Presentation
{
    internal static class NumericInput
    {
        private static readonly Regex Digits = new Regex("^[0-9]+$");

        public static bool IsDigits(string text) => Digits.IsMatch(text);

        public static void Synchronize(TextBox input, TrackBar track, Action applyValue)
        {
            if (IsDigits(input.Text))
            {
                // Retain the original parsing and clamping behavior.
                int value = int.Parse(input.Text);
                if (value < track.Minimum)
                {
                    SystemSounds.Beep.Play();
                    value = track.Minimum;
                }
                if (value > track.Maximum)
                {
                    SystemSounds.Beep.Play();
                    value = track.Maximum;
                }
                track.Value = value;
            }
            else if (input.Text.Length == 0)
            {
                SystemSounds.Beep.Play();
                return;
            }
            else
            {
                SystemSounds.Beep.Play();
                track.Value = track.Minimum;
            }

            applyValue();
            input.SelectionStart = input.Text.Length;
        }
    }
}
