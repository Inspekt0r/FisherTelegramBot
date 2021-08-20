using System;
using System.ComponentModel.DataAnnotations;

namespace TelegramAspBot.Models.Entity
{
    public class CharStat
    {
        [Key]
        public int Id { get; set; }
        public int FishCaughtCount { get; set; }
        public int HookCount { get; set; }
        public double MostWeightFish { get; set; }
        public double MostHeightFish { get; set; }
        public int FishingTry { get; set; }
        public int BaitUsingCount { get; set; }
        /// <summary>
        /// Неудачные попытки подряд
        /// </summary>
        public int UnluckyTry { get; set; }
        public int FishOfMyDreams { get; set; } //Сколько поймано Язя 
        public int Percent { get; set; }
        //сезонная статистика
        public int SeasonFishCaughtCount { get; set; }
        public int SeasonHookCount { get; set; }
        public double SeasonMostWeightFish { get; set; }
        public string SeasonMostWeightName { get; set; }
        public double SeasonMostHeightFish { get; set; }
        public string SeasonMostHeighеName { get; set; }
        public int SeasonFishingTry { get; set; }
        public int EventFishCount { get; set; } = 0;
        /// <summary>
        /// Дата поимки Эвентовой рыбы
        /// </summary>
        public DateTime FirstEventFishCatchTime { get; set; } = DateTime.UtcNow;
        public void IsBiggerWeight(BackpackItem fish)
        {
            if (fish.Weight > MostWeightFish)
            {
                MostWeightFish = Math.Round(fish.Weight);
                SeasonMostWeightFish = Math.Round(fish.Weight);
                SeasonMostWeightName = fish.ItemName;
            }
        }

        public void IsBiggerHeight(BackpackItem fish)
        {
            if (fish.Height > MostHeightFish)
            {
                MostHeightFish = Math.Round(fish.Height, 2);
                SeasonMostHeightFish = Math.Round(fish.Height, 2);
                SeasonMostHeighеName = fish.ItemName;
            }
        }

        public void IsFishOfMyDream(BackpackItem fish)
        {
            if (fish.ItemName == "Язь")
            {
                FishOfMyDreams++;
            }
        }
        
        public int PercentCatches()
        {
            var percent = 0;
            if (FishingTry > 0)
            {
                var rawPercent = (FishCaughtCount * 1.0) / (FishingTry * 1.0);
                percent = (int) (rawPercent * 100);
            }

            Percent = percent;

            return percent;
        }
        public int SeasonPercentCatches()
        {
            var percent = 0;
            if (SeasonFishingTry > 0)
            {
                var rawPercent = (SeasonFishCaughtCount * 1.0) / (SeasonFishingTry * 1.0);
                percent = (int) (rawPercent * 100);
            }

            return percent;
        }
    }
}