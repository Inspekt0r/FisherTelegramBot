using System.Collections.Generic;
using System.Linq;
using TelegramAspBot.Models.Entity;
using TelegramAspBot.Models.LocationModule;

namespace TelegramAspBot.Models
{
    /// <summary>
    /// Класс предназначенный для скриптов миграции на день святого Валентина (создание нового спота)
    /// </summary>
    public class StValentinesUpdate
    {
        public static void GenerateNewSpotAndFish(ApplicationContext dbContext)
        {
            var createdSpot = dbContext.Spots.FirstOrDefault(p => p.Name == "Озеро Влюбленных");
            if (createdSpot != null)
            {
                return;
            }
            
            var spot = new Spot() {Name = "Озеро Влюбленных",};
            dbContext.Add(spot);
            dbContext.SaveChanges();

            var fishName = new List<string>()
            {
                "Рыба сердце", "Розовая Форель", "Валентинка"
            };
            var pattern = new FishPattern();
            var fishReference = new List<FishReference>();
            foreach (var str in fishName)
            {
                fishReference.Add(pattern.GetFishByName(str));
            }
            dbContext.AddRange(fishReference);
            dbContext.SaveChanges();
            
            var updateSpot = dbContext.Spots.FirstOrDefault(p => p.Name == "Озеро Влюбленных");
            var tempRef = LocationStorage.GetRefForLocation(dbContext, fishName, spot.Name);
            updateSpot.FishReferenceSpots.AddRange(tempRef);
            
            dbContext.SaveChanges();
            
        }
    }
}