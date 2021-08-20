using System.Linq;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    /// <summary>
    /// Класс обработчик информации и о рыбе, добавляет новый FishPediaInfo если рыба впервые поймана.
    /// Обновляет текущий FishPediaInfo если рыба была уже поймана ранее.
    /// </summary>
    public class FishpediaWorker
    {
        public void AddInfoAboutFishToCharacter(Character character, BackpackItem fish)
        {
            var fishPedia = character.FishPedia;

            if (fishPedia.FishPediaInfoList.Count == 0)
            {
                AddFish(character, fish);
                return;
            }
            
            foreach (var fishInfo in fishPedia.FishPediaInfoList)
            {
                if (fish.ItemName == fishInfo.Name)
                {
                    fishInfo.PlusCaught();
                    fishInfo.UpdateHeight(fish.Height);
                    fishInfo.UpdateWeight(fish.Weight);
                    return;
                }
            }
            //добавляем рыбу если варианты выше не подошли
            AddFish(character, fish);
        }

        private static void AddFish(Character character, BackpackItem fish)
        {
            var fishPediaInfo = new FishPediaInfo()
            {
                Name = fish.ItemName,
                Rarity = fish.Rarity,
                FishType = fish.FishBiteType,
                MaxHeight = fish.Height,
                MaxWeight = fish.Weight,
                Caught = 1
            };

            character.FishPedia.FishPediaInfoList.Add(fishPediaInfo);
        }
    }
}