using System;
using System.Windows.Forms;
using Krotovukha_Game.Presentation;

namespace Krotovukha_Game
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var images = new ImageCatalog())
            using (var window = new MainWindow(images))
            {
                Application.Run(window);
                // Auxiliary windows can remain open when the main window is closed.
                // Dispose them before disposing the shared image catalog.
                while (Application.OpenForms.Count > 0)
                {
                    Application.OpenForms[0].Dispose();
                }
            }
        }
    }
}
