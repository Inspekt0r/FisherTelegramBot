using System.Collections.Generic;
using Telegram.Bot;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class FishReference
    {
        public int Id {get; set;} 
        public double MaxWeight {get;set;} 
        public double MaxHeight {get;set;} 
        public string Name {get; set;}
        public Rarity Rarity { get; set; }
        public bool IsEvent { get; set; }
        public FishType FishType { get; set; } = FishType.Combo;
        //TODO Параметры погоды
        public WeatherType WeatherType { get; set; } = WeatherType.None;
        public int MinTemperature { get; set; }
        public int MaxTemperature { get; set; }

        public Item ConvertFishRefToItem()
        {
            return new Item()
            {
                Name = Name,
                Weight = MaxWeight,
                Height = MaxHeight,
                Rarity = Rarity,
                IsEvent = IsEvent,
                FishBiteType = FishType,
                WeatherType = WeatherType,
                MinTemperature = MinTemperature,
                MaxTemperature = MaxTemperature
            };
        }
    }
}