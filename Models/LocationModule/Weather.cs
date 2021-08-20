using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models.LocationModule
{
    public class Weather
    {
        public WeatherType WeatherType { get; set; }
        public double Temperature { get; set; }
    }
}