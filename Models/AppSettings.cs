using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace TelegramAspBot.Models
{
    public static class AppSettings
    {
        public static string Url { get; set; } = ConfigurationManager.AppSettings.Get("URL");
        public static string Name { get; set; } = ConfigurationManager.AppSettings.Get("BotName");
        public static string ApiToken { get; set; } = ConfigurationManager.AppSettings.Get("ApiToken");

        public static string ConnectionString { get; set; } = ConfigurationManager.AppSettings.Get("Database");
    }
}
