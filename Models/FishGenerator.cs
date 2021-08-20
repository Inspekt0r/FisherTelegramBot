using System;
using System.Collections.Generic;
using System.Linq;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class FishGenerator
    {
        private readonly Random _random = new Random();
        private readonly FishPattern _fishPattern = new FishPattern();
        private readonly List<string> _commonFish = new List<string>()
        {
            "Альбула", "Аравана", "Белый Басс", "Белый Толстолобик", "Берш", "Язь"
        };
        private readonly List<string> _uncommonFish = new List<string>()
        {
            "Американская Сельдь", "Арапаима", "Белый Осетр", "Биара"
        };
        private readonly List<string> _rareFish = new List<string>()
        {
            "Американский Голец", "Белый Амур", "Белый Сом"
        };
        private readonly List<string> _eliteFish = new List<string>()
        {
            "Апапа", "Атлантический Лосось", "Белуга"
        };
        private readonly List<string> _mythicalFish = new List<string>()
        {
            "Аллигаторова Щука", "Белый Краппи"
        };
        private readonly List<string> _legendaryFish = new List<string>()
        {
            "Армированный Черный Сом", "Белый Чукучан"
        };
        //TODO deprecated
        // public Item GetNewFish()
        // {
        //     var generateFish = new Item
        //     {
        //         ItemType = ItemType.Fish
        //     };
        //     
        //     var rarity = GetRandomRarity();
        //     generateFish.Rarity = rarity;
        //     generateFish.Name = GetNameByRarity(rarity);
        //
        //     var fishPattern = _fishPattern.GetFishByName(generateFish.Name);
        //     var multiply = _random.Next(50, 101) / 100.0;
        //
        //     generateFish.Height = fishPattern.MaxHeight * multiply;
        //     generateFish.Weight = fishPattern.MaxWeight * multiply;
        //
        //     return generateFish;
        // }
        //
        // public Item GetFishBySpot(Spot spot)
        // {
        //     var generateFish = new Item
        //     {
        //         ItemType = ItemType.Fish
        //     };
        //     
        //     FishReference fishFromSpot = null;
        //
        //     while (fishFromSpot == null)
        //     {
        //         var rarity = GetRandomRarity();
        //         var randomFishRefList = spot.FishReferenceSpots.Where(p => p.FishReference.Rarity == rarity).ToList();
        //         if (randomFishRefList.Count == 0)
        //         {
        //             randomFishRefList = spot.FishReferenceSpots.Where(p => p.FishReference.Rarity == Rarity.Common).ToList();
        //         }
        //         fishFromSpot = randomFishRefList[_random.Next(0, randomFishRefList.Count)].FishReference;
        //     }
        //
        //     generateFish.Name = fishFromSpot.Name;
        //     generateFish.Rarity = fishFromSpot.Rarity;
        //
        //     var multiply = _random.Next(50, 101) / 100.0;
        //
        //     generateFish.Height = fishFromSpot.MaxHeight * multiply;
        //     generateFish.Weight = fishFromSpot.MaxWeight * multiply;
        //
        //     return generateFish;
        // }
        
        private string GetNameByRarity(Rarity rarity)
        {
            var result = string.Empty;
            
            switch (rarity)
            {
                case Rarity.Common:
                    result = _commonFish[_random.Next(0, _commonFish.Count)];
                    break;
                case Rarity.Uncommon:
                    result = _uncommonFish[_random.Next(0, _uncommonFish.Count)];
                    break;
                case Rarity.Rare:
                    result = _rareFish[_random.Next(0, _rareFish.Count)];
                    break;
                case Rarity.Elite:
                    result = _eliteFish[_random.Next(0, _eliteFish.Count)];
                    break;
                case Rarity.Mythical:
                    result = _mythicalFish[_random.Next(0, _mythicalFish.Count)];
                    break;
                case Rarity.Legendary:
                    result = _legendaryFish[_random.Next(0, _legendaryFish.Count)];
                    break;
            }

            return result;
        }

        private Rarity GetRandomRarity()
        {
            var diceResult = DicesThrow();
            if (diceResult < 4 || diceResult > 17)
            {
                return Rarity.Legendary;
            } else if (diceResult < 5 || diceResult > 16)
            {
                return Rarity.Mythical;
            } else if (diceResult < 6 || diceResult > 15)
            {
                return Rarity.Elite;
            } else if (diceResult < 7 || diceResult > 14)
            {
                return Rarity.Rare;
            } else if (diceResult < 9 || diceResult > 13)
            {
                return Rarity.Uncommon;
            }
            else
            {
                return Rarity.Common;
            }
        }

        private int DicesThrow()
        {
            var diceOne = _random.Next(1, 7);
            var diceTwo = _random.Next(1, 7);
            var diceThree = _random.Next(1, 7);

            var average = (diceOne + diceThree + diceTwo) / 3;
            if (average <= diceOne)
            {
                diceOne = _random.Next(1, 7);
            } else if (average <= diceTwo)
            {
                diceTwo = _random.Next(1, 7);
            } else if (average <= diceThree)
            {
                diceThree = _random.Next(1, 7);
            }

            return diceOne + diceTwo + diceThree;
        }
    }
}

/*
Большеголовый Голец \ Bull trout
Большеротый Басс \ Largemouth Bass
Большеротый Буффало \ Bigmouth Buffalo
Брикон \ Brycon
Бурый Паку \ Tambaqui
Бычок-Кругляк \ Round Goby
Бычок-Песочник \ Monkey Goby
Вобла \ Caspian Roach
Глазчатый Aстронотус \ Oscar
Голавль \ Chub
Голубой Сом \ Blue Catfish
Голый Карп \ Leather Carp
Голый Маскинонг \ Clear Muskie
Горбуша \ Pink Salmon
Густера \ Silver Bream
Даллия \ Alaska Blackfish
Длинноносый Панцирник \ Longnose Gar
Длинноносый Чукучан \ Longnose sucker
Европейская Кумжа \ European Brown Trout
Европейский Горчак \ European Bitterling
Европейский Угорь \ European Eel﻿﻿
Европейский Хариус \ European Grayling﻿
Желтый Окунь \ Yellow Perch
Жерех \ Asp
Зеленый Солнечник \ Green Sunfish
Зеркальный Карп \ Mirror Carp
Золотистый Шайнер \ Golden Shiner
Золотой Линь \ Golden Tench
Зунгаро \ Gilded Catfish
Ильная Рыба \ Bowfin
Калифорнийская Плотва \ California Roach
Канадский Судак \ Sauger
Канальный Сомик \ Channel Catfish
Карасекарп \ Hybrid F1 Carp
Карп (Сазан) \ Common Carp
Карп Альбинос \ Ghost Carp
Кета \ Chum Salmon
Коричневый Сомик \ Brown Bullhead
Красногорлая Форель \ Cutthroat Trout
Красногрудый Солнечник \ Redbreast Sunfish
Красноперая Щука \ Redfin Pickerel
Красноперка \ Common Rudd
Красноухий Солнечник \ Redear Sunfish
Краснохвостая баракуда \ Red Tail Barracuda
Красный Горбыль \ Red drum
Красный Павлиний Басс \ Red Peacock Bass
Крапчатый Павлиний Басс \ Speckled Peacock Bass
Круглый Трахинот \ Permit
Кумжа \ Brown Trout
Линь \ Tench
Малоротый Басс \ Smallmouth Bass
Малоротый Буффало \ Smallmouth Buffalo
Мальма \ Dolly Varden
Мраморная Форель \ Marble Trout
Налим \ Burbot
Обыкновенный Солнечник \ Pumpkinseed
Озерная Форель \ Lake Trout
Озерный Осетр \ Lake Sturgeon
Озерный Цезиус \ Lake Chub
Оливковый Сом \ Flathead Catfish
Павлиний Басс \ Butterfly Peacock Bass
Паяра \ Payara
Пестрый Толстолобик \ Bighead Carp
Пирарара \ Redtail Catfish
Плоскоголовый Усатый Сом \ Flatwhiskered Catfish
Полосатая Щука \ Chain Pickerel
Пресноводный Горбыль \ Freshwater Drum
Прибрежный Басс \ Shoal Bass
Пятнистый Араку \ Spotted Aracu
Речной Окунь \ European Perch
Ромбовидная Пиранья \ Redeye Piranha
Ручьевая Форель \ Creek Brown Trout
Светлоперый Судак \ Walleye
Северная доросома \ American Gizzard Shad
Сельдевидный Сиг \ Lake Whitefish
Синежаберный Солнечник \ Bluegill
Синец \ Blue Bream﻿
Скалистая Золотая Форель \ Colorado Golden Trout
Скальный Окунь \ Rock Bass
Снук Обыкновенный \ Common snook
Сом Вымпельный Морской \ Gafftopsail sea catfish
Сом Соруби \ Sorubim
Сплейк \ Splake Trout
Стальноголовый Лосось \ Steellhead
Судак Обыкновенный \ Zander
Тарпон \ Tarpon
Тёмный Горбыль \ Black Drum
Тигровая Форель \ Tiger Trout
Толстоголовый Гольян \ Bluntnose Minnow
Травяная Щука \ Grass Pickerel
Трахира \ Trahira
Уклейка Обыкновенная \ Common Bleak
Усач Обыкновенный \ Common Barbel
Флоридский Панцирник \ Florida Gar
Хараки \ Flag-Tailed Prochilodus
Чавыча \ Chinook
Чернохвостый Шайнер \ Blacktail Shiner
Черный Краппи \ Black Crappie
Черный Солнечник \ Warmouth
Черный Сомик \ Black Bullhead
Шайнер Обыкновенный
Щука-маскинонг
Южная Камбала
Южноамериканский Горбыль
Якунская рыба
*/