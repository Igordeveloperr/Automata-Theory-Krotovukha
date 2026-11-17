using System;
using System.Collections.Generic;
using System.Drawing;
using Krotovukha_Game.Domain;

namespace Krotovukha_Game.Settings
{
    /// <summary>Legacy JSON contract: property names and declaration order must remain stable.</summary>
    [Serializable]
    public sealed class GameSettings
    {
        public Color Color0 { get; set; }
        public Color Color1 { get; set; }
        public Color Color2 { get; set; }
        public Color Color3 { get; set; }
        public Color Color4 { get; set; }
        public Color Color5 { get; set; }
        public Color Color6 { get; set; }
        public Color Color7 { get; set; }
        public Color Color8 { get; set; }
        public Color Color9 { get; set; }
        public int Speed { get; set; }
        public int Probability { get; set; }
        public string[] Field { get; set; } = new string[GameField.CellCount];

        public Color[] GetColors()
        {
            return new[] { Color0, Color1, Color2, Color3, Color4, Color5, Color6, Color7, Color8, Color9 };
        }

        public void SetColors(IReadOnlyList<Color> colors)
        {
            Color0 = colors[0];
            Color1 = colors[1];
            Color2 = colors[2];
            Color3 = colors[3];
            Color4 = colors[4];
            Color5 = colors[5];
            Color6 = colors[6];
            Color7 = colors[7];
            Color8 = colors[8];
            Color9 = colors[9];
        }
    }
}
