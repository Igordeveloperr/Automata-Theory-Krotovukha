using System.IO;
using System.Windows.Forms;
using Krotovukha_Game.Settings;
using Newtonsoft.Json;

namespace Krotovukha_Game.Presentation
{
    internal static class SettingsDialogs
    {
        public static void Save(GameSettings settings)
        {
            string json = SettingsSerializer.Serialize(settings);
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                using (var writer = new StreamWriter(dialog.FileName, false))
                {
                    writer.WriteLine(json);
                }
            }
        }

        public static GameSettings Open()
        {
            string json;
            using (var dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog() == DialogResult.Cancel)
                {
                    return null;
                }
                using (var reader = new StreamReader(dialog.FileName))
                {
                    json = reader.ReadToEnd();
                }
            }

            try
            {
                return SettingsSerializer.Deserialize(json);
            }
            catch (JsonReaderException)
            {
                MessageBox.Show("Файл поврежден!");
                return null;
            }
        }
    }
}
