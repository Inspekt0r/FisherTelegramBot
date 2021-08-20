using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TelegramAspBot.Models.Entity
{
    public class Spot
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Character> Characters { get; set; }
        public virtual List<FishReferenceSpot> FishReferenceSpots { get; set; } = new List<FishReferenceSpot>();
        public double Temperature { get; set; } = 20.0;
        public WeatherType WeatherType { get; set; } = WeatherType.None;
        public double CatchBonus { get; set; } = 0.0;

        /// <summary>
        /// Поле отвечающее за активность данного спота (актуально для эвентовых)
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
    
    public enum WeatherType
    {
        None,
        Cloudy,
        PartyCloudy,
        Clear,
        Rainy,
        Snow,
        Fog,
        Windy
    }
}