using System.Collections.Generic;
using System.Linq;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models.LocationModule
{
    public class LocationStorage
    {
        private static readonly List<string> StartedFishList = new List<string>()
        {
            "Альбула", "Аравана", "Белый Басс", "Белый Толстолобик", "Берш", "Американская Сельдь",
            "Арапаима", "Белый Осетр", "Биара", "Американский Голец", "Белый Амур", "Белый Сом",
            "Апапа", "Аллигаторова Щука", "Язь", "Белый Чукучан", "Лещ Обыкновенный", "Кижуч","Длинноголовый голец"
        };
        
        private static readonly List<string> MoreThanBigLakeFishList = new List<string>()
        {
            "Альбула", "Сом Обыкновенный", "Белый Толстолобик", "Берш", "Американская Сельдь",
            "Арапаима", "Белый Осетр", "Биара", "Американский Голец", "Белый Амур",
            "Апапа", "Аллигаторова Щука", "Язь", "Армированный Черный Сом",
            "Белый Краппи", "Белуга", "Атлантический Лосось", "Золотой Карась",
            "Орегонская Форель", "Пятнистый Басс", "Толстогубая Кефаль", "Лещ Обыкновенный"
        };
        
        private static readonly List<string> StinkyRiver = new List<string>()
        {
            "Речная Камбала", "Пиранья", "Ёрш Обыкновенный", "Лещ Обыкновенный",
            "Пескарь Обыкновенный", "Язь", "Белый Басс", "Плотва", "Толстогубая Кефаль",
            "Серебряный Карась", "Белый Чукучан", "Чехонь", 
        };
        
        private static readonly List<string> IsleInOcean = new List<string>()
        {
            "Речная Камбала", "Пиранья", "Белый Басс", "Толстогубая Кефаль", "Полосатый Лаврак",
            "Серебряный Карась", "Белый Чукучан", "Радужная Форель", "Сом Обыкновенный", "Нерка",
            "Золотой Карась", "Обыкновенная Щука", "Орегонская Форель", "Кижуч",
            "Крапчатый Павлиний Басс", "Тигровый Маскинонг", "Серебряный Карась", "Радужная Форель"
        };
        
        public static readonly List<Spot> Spots = new List<Spot>()
        {
            new Spot() { Name = "Стартовое озеро", },
            new Spot() { Name = "Озеро побольше", },
            new Spot() { Name = "Речка-вонючка", },
            new Spot() { Name = "Остров в океане", },
        };

        public static void UpdateSpotByFishRef(ApplicationContext dbContext)
        {
            var spotList = dbContext.Spots.ToList();
            foreach (var spot in spotList)
            {
                if (spot.Name == "Стартовое озеро")
                {
                    var tempRef = GetRefForLocation(dbContext, StartedFishList, spot.Name);
                    dbContext.AddRange(tempRef);
                    spot.FishReferenceSpots.AddRange(tempRef);
                }

                if (spot.Name == "Озеро побольше")
                {
                    var tempRef = GetRefForLocation(dbContext, MoreThanBigLakeFishList, spot.Name);
                    dbContext.AddRange(tempRef);
                    spot.FishReferenceSpots.AddRange(tempRef);
                }

                if (spot.Name == "Речка-вонючка")
                {
                    var tempRef = GetRefForLocation(dbContext, StinkyRiver, spot.Name);
                    dbContext.AddRange(tempRef);
                    spot.FishReferenceSpots.AddRange(tempRef);
                }
                
                if (spot.Name == "Остров в океане")
                {
                    var tempRef = GetRefForLocation(dbContext, IsleInOcean, spot.Name);
                    dbContext.AddRange(tempRef);
                    spot.FishReferenceSpots.AddRange(tempRef);
                }
            }
        }

        public static List<FishReferenceSpot> GetRefForLocation(ApplicationContext dbContext, IEnumerable<string> fishNameList, string locationName)
        {
            var fishAddReferenceList =  new List<FishReferenceSpot>();

            foreach (var fishName in fishNameList)
            {
                var fishReference = dbContext.FishReferences.FirstOrDefault(p => p.Name == fishName);
                if (fishReference != null)
                {
                    fishAddReferenceList.Add(new FishReferenceSpot() { FishReference = fishReference });
                }
            }

            var spotToCreate = dbContext.Spots.FirstOrDefault(p => p.Name == locationName);
            if (spotToCreate == null) return fishAddReferenceList;
            
            var fishExistedList = spotToCreate.FishReferenceSpots;
                
            var newFishList = new List<FishReferenceSpot>();
            if (fishExistedList == null) return fishAddReferenceList;
            foreach (var newFishOnAdd in fishAddReferenceList)
            {
                bool isExist = false;
                foreach (var existedFish in fishExistedList)
                {
                    
                    if (existedFish.FishReference.Name == newFishOnAdd.FishReference.Name)
                    {
                        isExist = true;
                    }
                }
                
                if (!isExist)
                {
                    newFishList.Add(new FishReferenceSpot() { FishReference = newFishOnAdd.FishReference });
                }
            }

            return newFishList;
        }
    }
}