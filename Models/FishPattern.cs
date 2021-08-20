using System;
using System.Collections.Generic;
using System.Linq;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class FishPattern
    {
        private readonly List<string> _allFishesName = new List<string>()
        {
            "Альбула", "Аравана", "Белый Басс", "Белый Толстолобик", "Берш",
            "Американская Сельдь", "Арапаима", "Белый Осетр", "Биара",
            "Американский Голец", "Белый Амур", "Белый Сом",
            "Апапа", "Атлантический Лосось", "Белуга","Аллигаторова Щука", 
            "Белый Краппи","Армированный Черный Сом", "Белый Чукучан", "Язь",
            "Речная Камбала", "Полосатый Лаврак", "Сом Обыкновенный", "Нерка",
            "Пиранья", "Чехонь", "Пятнистый Басс", "Золотой Карась", "Ёрш Обыкновенный",
            "Лещ Обыкновенный", "Обыкновенная Щука", "Пескарь Обыкновенный", "Орегонская Форель",
            "Плотва",  "Кижуч", "Толстогубая Кефаль", "Крапчатый Павлиний Басс", "Тигровый Маскинонг",
            "Серебряный Карась", "Радужная Форель", "Длинноголовый голец", 
            "Рыба сердце", "Розовая Форель", "Валентинка",
            
        };

        private readonly List<FishReference> _fishes;
        
        public FishPattern()
        {
            _fishes = new List<FishReference>();
            foreach (var name in _allFishesName)
            {
                _fishes.Add(CreateFishByName(name));
            }
        }

        public List<FishReference> GetFishes()
        {
            return _fishes;
        }

        public List<FishReference> EventFishList()
        {
            if (DateTime.UtcNow.Month == 2 && DateTime.UtcNow.Day == 14)
            {
                var valentineNames = new List<string>() {"Рыба сердце", "Розовая Форель", "Валентинка"};
                return (from fish in _fishes from str in valentineNames where fish.Name == str select fish).ToList();
            }
            var filteredFish = _fishes.Where(p => !p.IsEvent && p.Rarity == Rarity.Common 
                                                    || p.Rarity == Rarity.Uncommon).ToList();
            return filteredFish;
        }

        public FishReference GetFishByName(string name)
        {
            return _fishes.First(p => p.Name == name);
        }
        /*Введено:
         13 common
         10 uncommon
         6 rare
         5 elite
         4 mythical
         3 legendary
         */
        private static FishReference CreateFishByName(string name)
        {
            var fish = new FishReference();
            switch (name)
            {
                case "Рыба сердце":
                    fish.MaxHeight = 5;
                    fish.MaxWeight = 15;
                    fish.Name = "Рыба сердце";
                    fish.Rarity = Rarity.Common;
                    fish.IsEvent = true;
                    break;
                case "Розовая Форель":
                    fish.MaxHeight = 5;
                    fish.MaxWeight = 15;
                    fish.Name = "Розовая Форель";
                    fish.Rarity = Rarity.Common;
                    fish.IsEvent = true;
                    break;
                case "Валентинка":
                    fish.MaxHeight = 1;
                    fish.MaxWeight = 15;
                    fish.Name = "Валентинка";
                    fish.Rarity = Rarity.Common;
                    fish.IsEvent = true;
                    break;
                case "Альбула":
                    fish.MaxHeight = 1.04;
                    fish.MaxWeight = 10;
                    fish.Name = "Альбула";
                    fish.Rarity = Rarity.Common;
                    break;
                case "Длинноголовый голец":
                    fish.MaxHeight = 0.65;
                    fish.MaxWeight = 2.2;
                    fish.Name = "Длинноголовый голец";
                    fish.Rarity = Rarity.Common;
                    break;
                case "Аравана":
                    fish.MaxHeight = 1.2;
                    fish.MaxWeight = 6;
                    fish.Name = "Аравана";
                    fish.Rarity = Rarity.Common;
                    break;
                case "Белый Басс":
                    fish.MaxHeight = 0.48;
                    fish.MaxWeight = 3.1;
                    fish.Name = "Белый Басс";
                    fish.Rarity = Rarity.Common;
                    break;
                case "Белый Толстолобик":
                    fish.MaxHeight = 1.0;
                    fish.MaxWeight = 40;
                    fish.Name = "Белый Толстолобик";
                    fish.Rarity = Rarity.Common;
                    break;
                case "Берш":
                    fish.MaxHeight = 0.45;
                    fish.MaxWeight = 2.9;
                    fish.Name = "Берш";
                    fish.Rarity = Rarity.Common;
                    break;
                case "Язь":
                    fish.MaxHeight = 0.54;
                    fish.MaxWeight = 2.8;
                    fish.Name = "Язь";
                    fish.Rarity = Rarity.Common;
                    break;
                case "Речная Камбала":
                    fish.MaxHeight = 0.65;
                    fish.MaxWeight = 2.95;
                    fish.Name = "Речная Камбала";
                    fish.Rarity = Rarity.Common;
                    break;
                case "Полосатый Лаврак":
                    fish.MaxHeight = 1.2;
                    fish.MaxWeight = 58.0;
                    fish.Name = "Полосатый Лаврак";
                    fish.Rarity = Rarity.Common;
                    break; 
                case "Сом Обыкновенный":
                    fish.MaxHeight = 5.2;
                    fish.MaxWeight = 410.0;
                    fish.Name = "Сом Обыкновенный";
                    fish.Rarity = Rarity.Common;
                    break;
                case "Нерка":
                    fish.MaxHeight = 1.5;
                    fish.MaxWeight = 7.8;
                    fish.Name = "Нерка";
                    fish.Rarity = Rarity.Common;
                    break;
                case "Чехонь":
                    fish.MaxHeight = 0.6;
                    fish.MaxWeight = 2.1;
                    fish.Name = "Чехонь";
                    fish.Rarity = Rarity.Common;
                    break;
                case "Пиранья":
                    fish.MaxHeight = 0.5;
                    fish.MaxWeight = 3.9;
                    fish.Name = "Пиранья";
                    fish.Rarity = Rarity.Common;
                    break;
                case "Американская Сельдь":
                    fish.MaxHeight = 0.76;
                    fish.MaxWeight = 6;
                    fish.Name = "Американская Сельдь";
                    fish.Rarity = Rarity.Uncommon;
                    break;
                case "Арапаима":
                    fish.MaxHeight = 2.5;
                    fish.MaxWeight = 250;
                    fish.Name = "Арапаима";
                    fish.Rarity = Rarity.Uncommon;
                    break;
                case "Белый Осетр":
                    fish.MaxHeight = 6.1;
                    fish.MaxWeight = 816;
                    fish.Name = "Белый Осетр";
                    fish.Rarity = Rarity.Uncommon;
                    break;
                case "Биара":
                    fish.MaxHeight = 0.8;
                    fish.MaxWeight = 2.1;
                    fish.Name = "Биара";
                    fish.Rarity = Rarity.Uncommon;
                    break;
                case "Пятнистый Басс":
                    fish.MaxHeight = 0.64;
                    fish.MaxWeight = 1.51;
                    fish.Name = "Пятнистый Басс";
                    fish.Rarity = Rarity.Uncommon;
                    break;
                case "Золотой Карась":
                    fish.MaxHeight = 0.4;
                    fish.MaxWeight = 1.51;
                    fish.Name = "Золотой Карась";
                    fish.Rarity = Rarity.Uncommon;
                    break;
                case "Ёрш Обыкновенный":
                    fish.MaxHeight = 0.2;
                    fish.MaxWeight = 0.11;
                    fish.Name = "Ёрш Обыкновенный";
                    fish.Rarity = Rarity.Uncommon;
                    break;
                case "Лещ Обыкновенный":
                    fish.MaxHeight = 0.82;
                    fish.MaxWeight = 6.1;
                    fish.Name = "Лещ Обыкновенный";
                    fish.Rarity = Rarity.Uncommon;
                    break;
                case "Обыкновенная Щука":
                    fish.MaxHeight = 1.5;
                    fish.MaxWeight = 35.0;
                    fish.Name = "Обыкновенная Щука";
                    fish.Rarity = Rarity.Uncommon;
                    break;
                case "Пескарь Обыкновенный":
                    fish.MaxHeight = 0.22;
                    fish.MaxWeight = 0.15;
                    fish.Name = "Пескарь Обыкновенный";
                    fish.Rarity = Rarity.Uncommon;
                    break;
                case "Американский Голец":
                    fish.MaxHeight = 0.4;
                    fish.MaxWeight = 1.2;
                    fish.Name = "Американский Голец";
                    fish.Rarity = Rarity.Rare;
                    break;
                case "Белый Амур":
                    fish.MaxHeight = 1.5;
                    fish.MaxWeight = 45;
                    fish.Name = "Белый Амур";
                    fish.Rarity = Rarity.Rare;
                    break;
                case "Белый Сом":
                    fish.MaxHeight = 3.1;
                    fish.MaxWeight = 306;
                    fish.Name = "Белый Сом";
                    fish.Rarity = Rarity.Rare;
                    break;
                case "Орегонская Форель":
                    fish.MaxHeight = 0.25;
                    fish.MaxWeight = 1.81;
                    fish.Name = "Орегонская Форель";
                    fish.Rarity = Rarity.Rare;
                    break;
                case "Плотва":
                    fish.MaxHeight = 0.55;
                    fish.MaxWeight = 3.01;
                    fish.Name = "Плотва";
                    fish.Rarity = Rarity.Rare;
                    break;
                case "Кижуч":
                    fish.MaxHeight = 1.08;
                    fish.MaxWeight = 15.2;
                    fish.Name = "Кижуч";
                    fish.Rarity = Rarity.Rare;
                    break;
                case "Апапа":
                    fish.MaxHeight = 0.85;
                    fish.MaxWeight = 5.1;
                    fish.Name = "Апапа";
                    fish.Rarity = Rarity.Elite;
                    break;
                case "Атлантический Лосось":
                    fish.MaxHeight = 1.5;
                    fish.MaxWeight = 43;
                    fish.Name = "Атлантический Лосось";
                    fish.Rarity = Rarity.Elite;
                    break;
                case "Белуга":
                    fish.MaxHeight = 4.2;
                    fish.MaxWeight = 1500;
                    fish.Name = "Белуга";
                    fish.Rarity = Rarity.Elite;
                    break;
                case "Толстогубая Кефаль":
                    fish.MaxHeight = 0.34;
                    fish.MaxWeight = 2.01;
                    fish.Name = "Толстогубая Кефаль";
                    fish.Rarity = Rarity.Elite;
                    break;
                case "Крапчатый Павлиний Басс":
                    fish.MaxHeight = 1.0;
                    fish.MaxWeight = 13.3;
                    fish.Name = "Крапчатый Павлиний Басс";
                    fish.Rarity = Rarity.Elite;
                    break;
                case "Аллигаторова Щука":
                    fish.MaxHeight = 3.0;
                    fish.MaxWeight = 150;
                    fish.Name = "Аллигаторова Щука";
                    fish.Rarity = Rarity.Mythical;
                    break;
                case "Белый Краппи":
                    fish.MaxHeight = 0.49;
                    fish.MaxWeight = 2.47;
                    fish.Name = "Белый Краппи";
                    fish.Rarity = Rarity.Mythical;
                    break;
                case "Тигровый Маскинонг":
                    fish.MaxHeight = 1.31;
                    fish.MaxWeight = 25.01;
                    fish.Name = "Тигровый Маскинонг";
                    fish.Rarity = Rarity.Mythical;
                    break;
                case "Серебряный Карась":
                    fish.MaxHeight = 0.5;
                    fish.MaxWeight = 3.01;
                    fish.Name = "Серебряный Карась";
                    fish.Rarity = Rarity.Mythical;
                    break;
                case "Армированный Черный Сом":
                    fish.MaxHeight = 1.0;
                    fish.MaxWeight = 13;
                    fish.Name = "Армированный Черный Сом";
                    fish.Rarity = Rarity.Legendary;
                    break;
                case "Белый Чукучан":
                    fish.MaxHeight = 0.58;
                    fish.MaxWeight = 1.6;
                    fish.Name = "Белый Чукучан";
                    fish.Rarity = Rarity.Legendary;
                    break;
                case "Радужная Форель":
                    fish.MaxHeight = 1.7;
                    fish.MaxWeight = 0.6;
                    fish.Name = "Радужная Форель";
                    fish.Rarity = Rarity.Legendary;
                    break;
            }
            
            return fish;
        }
    }
}