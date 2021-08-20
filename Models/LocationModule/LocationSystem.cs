using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenWeatherMap.Standard;
using OpenWeatherMap.Standard.Models;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models.LocationModule
{
    public class LocationSystem
    {
        //todo сделать уже хоть что-то блядь с этим
        private readonly Current _weather;

        private const string ClearSky = "clear sky";
        private const string FewClouds = "few clouds";
        private const string ScatteredClouds = "scattered clouds";
        private const string BrokenClouds = "broken clouds";
        private const string ShowerRain = "shower rain";
        private const string Rain = "rain";
        private const string Thunderstorm = "thunderstorm";
        private const string Snow = "snow";
        private const string Fog = "mist";

        private readonly ILogger<LocationSystem> _logger;
        private static readonly Random _random = new Random();

        public LocationSystem(ILogger<LocationSystem> logger)
        {
            _logger = logger;
            _weather = new Current("4021c1cdd4764fb320952dbf8f07420c");

            Task.Run(async () =>
            {
                while (true)
                {
                    await UpdateAllWeatherAsync();
                    await Task.Delay(1800000);
                }
            });
        }

        public Weather GetWeather(Spot spot)
        {
            var weather = new Weather
            {
                Temperature = GetTemperature(spot.Temperature)
            };
            weather.WeatherType = GetWeather(weather.Temperature);
            
            return new Weather();
        }

        private static double GetTemperature(double prevTemp)
        {
            var newTemperature = prevTemp + (_random.Next(0, 5) * GetMinusOrPlus());

            if (newTemperature > 40)
            {
                newTemperature = _random.Next(15, 35);
            }

            if (newTemperature < -35)
            {
                newTemperature = _random.Next(-30, -5);
            }

            return Math.Round(newTemperature, 1);
        }

        private static int GetMinusOrPlus()
        {
            if (_random.NextDouble() > 0.5)
            {
                return -1;
            }

            return 1;
        }

        private static WeatherType GetWeather(double temp)
        {
            var weatherModify = _random.NextDouble();

            if (temp > 7)
            {
                if (weatherModify > 0.8)
                {
                    return WeatherType.Rainy;
                }

                if (weatherModify > 0.5)
                {
                    return WeatherType.Cloudy;
                }

                if (weatherModify > 0.3)
                {
                    return WeatherType.PartyCloudy;
                }

                return WeatherType.Windy;
            }

            if (temp > 2 && temp < 7)
            {
                return WeatherType.Fog;
            }

            if (weatherModify > 0.7)
            {
                return WeatherType.Snow;
            }

            if (weatherModify > 0.5)
            {
                return WeatherType.Cloudy;
            }

            if (weatherModify > 0.3)
            {
                return WeatherType.PartyCloudy;
            }

            return WeatherType.Clear;
        }

        private async Task UpdateAllWeatherAsync()
        {
            await using var dbContext = new ApplicationContext();
            var spotList = dbContext.Spots.ToList();
            if (spotList.Count == 0)
            {
                _logger.LogInformation($"UpdateAllWeatherAsync: нет спотов для обновления");
                return;
            }

            foreach (var spot in spotList)
            {
                UpdateWeatherSpotAsync(spot);
            }
        }

        private void UpdateWeatherSpotAsync(Spot spot)
        {
            //todo Сделать вызов метода асинхронным
            _logger.LogInformation($"Запрашиваю погоду для {spot.Name}");
            try
            {
                
            }
            catch (Exception e)
            {
                _logger.LogError($"Произошла ошибка при попытке взять погоду для {spot.Name}\n" +
                                 e.StackTrace);
            }

            _logger.LogInformation($"Успешно обновил погоду для {spot.Name}");
        }

        private static WeatherType GetWeather(WeatherData weatherData)
        {
            return weatherData.Weathers[0].Description switch
            {
                ClearSky => WeatherType.Clear,
                FewClouds => WeatherType.PartyCloudy,
                ScatteredClouds => WeatherType.Cloudy,
                BrokenClouds => WeatherType.Cloudy,
                ShowerRain => WeatherType.Rainy,
                Rain => WeatherType.Rainy,
                Thunderstorm => WeatherType.Rainy,
                Snow => WeatherType.Snow,
                Fog => WeatherType.Fog,
                _ => WeatherType.Clear
            };
        }
    }
}