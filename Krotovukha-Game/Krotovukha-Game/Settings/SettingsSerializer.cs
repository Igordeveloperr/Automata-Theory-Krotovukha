using Newtonsoft.Json;

namespace Krotovukha_Game.Settings
{
    internal static class SettingsSerializer
    {
        public static string Serialize(GameSettings settings) => JsonConvert.SerializeObject(settings);
        public static GameSettings Deserialize(string json) => JsonConvert.DeserializeObject<GameSettings>(json);
    }
}
