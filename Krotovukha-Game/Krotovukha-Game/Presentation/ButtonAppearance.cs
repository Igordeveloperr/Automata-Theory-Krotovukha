using System.Drawing;
using System.Windows.Forms;

namespace Krotovukha_Game.Presentation
{
    internal static class ButtonAppearance
    {
        public static void ShowStart(Button button) => Apply(button, "Старт", Color.ForestGreen);
        public static void ShowStop(Button button) => Apply(button, "Стоп", Color.Red);
        public static void ShowPause(Button button) => Apply(button, "Пауза", Color.LightSeaGreen);
        public static void ShowContinue(Button button) => Apply(button, "Продолжить", Color.DarkGoldenrod);

        private static void Apply(Button button, string text, Color color)
        {
            button.BackColor = color;
            button.Text = text;
        }
    }
}
