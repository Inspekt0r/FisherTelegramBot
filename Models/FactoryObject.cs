using System;
using System.Collections.Generic;
using System.Linq;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class FactoryObject
    {
        private readonly Random _random = new Random();
        private readonly List<string> _fishNames = new List<string>()
        {
            "Щука","Лосось","Ёрш","Плотва","Камбала","Карась","Пескарик","Сёмга с сыром"
        };
        /*
        Common,
        Uncommon,
        Rare,
        Elite,
        Mythical,
        Legendary
         */
        
        public static Character GetNewCharWithIdAndName(int telegramId, string name)
        {
            return new Character()
            {
                Name = name,
                TelegramId = telegramId,
                IsSetupNickname = false
            };
        }
        
        public static Character GetNewCharWithId(int telegramId)
        {
            return new Character()
            {
                TelegramId = telegramId,
                IsSetupNickname = false,
                Backpack = new Backpack(),
                CharStat = new CharStat(),
                FishPedia = new FishPedia()
            };
        }

        public BackpackItem GetNewFish()
        {
            var generateFish = new BackpackItem
            {
                Rarity = (Rarity) _random.Next(0, 6),
                ItemName = _fishNames[_random.Next(0, _fishNames.Count())],
                Weight = 1.5,
                Height = 0.2,
                ItemType = ItemType.Fish
            };

            return generateFish;
        }
    }
}
