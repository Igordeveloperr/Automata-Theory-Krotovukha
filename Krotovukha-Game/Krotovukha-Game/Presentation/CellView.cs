using System.Windows.Forms;
using Krotovukha_Game.Domain;

namespace Krotovukha_Game.Presentation
{
    internal static class CellView
    {
        // These tags are part of the existing settings-file contract.
        public const string MoleTag = "krot";
        public const string CabbageTag = "kacan";

        public static CellKind ParseTag(string tag) => tag == MoleTag ? CellKind.Mole : CellKind.Cabbage;
        public static string ToTag(CellKind kind) => kind == CellKind.Mole ? MoleTag : CabbageTag;

        public static PictureBox Create()
        {
            return new PictureBox
            {
                Width = 65,
                Height = 50,
                SizeMode = PictureBoxSizeMode.CenterImage
            };
        }

        public static void ClearControls(Control container)
        {
            // Controls.Clear alone does not dispose the removed controls.
            while (container.Controls.Count > 0)
            {
                Control control = container.Controls[0];
                container.Controls.RemoveAt(0);
                control.Dispose();
            }
        }
    }
}
