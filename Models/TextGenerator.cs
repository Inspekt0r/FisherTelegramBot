namespace TelegramAspBot.Models
{
    public static class TextGenerator
    {
        public static string GetSettingEmoji(bool setting)
        {
            return setting ? "✅" : "⛔";
        }
    }
}