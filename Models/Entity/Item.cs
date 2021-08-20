using System;
using System.ComponentModel.DataAnnotations;
using TelegramAspBot.Models.Enum;
using TelegramAspBot.Models.Interfaces;

namespace TelegramAspBot.Models.Entity
{
    public class Item : IItem
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Параметр для предметов которые можно купить в магазине
        /// </summary>
        public bool IsForShopSale { get; set; }
        public ItemType ItemType { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public Rarity Rarity { get; set; }
        /// <summary>
        /// Параметр для удочек и приманок разного рода
        /// увеличивает шанс на поимку на N
        /// </summary>
        public double CatchBonus { get; set; } = 0.0;
        /// <summary>
        /// Параметр который увеличивает шанс успешной подсечки
        /// Работает только у FishingRod
        /// </summary>
        public double HookBonus { get; set; } = 0.0;
        /// <summary>
        /// Что приманивает и каким предметом/рыбой является
        /// </summary>
        public FishType FishBiteType { get; set; }
        /// <summary>
        /// Цена предмета/рыбы при покупке в магазине
        /// </summary>
        public int ShopPrice { get; set; } = 0;
        /// <summary>
        /// Цена предмета при продаже в магазин
        /// </summary>
        public int PlayerPrice { get; set; } = 0;
        public bool IsEvent { get; set; }

        /// <summary>
        /// Прочность предмета
        /// </summary>
        public int Durability { get; set; } = 200;

        /// <summary>
        /// Делитель прочности.
        /// 2.0 стандарт для Durability == 200
        /// </summary>
        public double DurabilityDenominator { get; set; } = 2.0;
        //Параметры для рыбы
        public double MaxTemperature { get; set; }
        public double MinTemperature { get; set; }
        public WeatherType WeatherType { get; set; } = WeatherType.None;
        //Если предмет Эвентовый то позиция нужна
        public EventPosition EventPosition { get; set; }
        
        public BackpackItem ConvertToBackpack()
        {
            return new BackpackItem()
            {
                ItemName = Name,
                ItemType = ItemType,
                Rarity = Rarity,
                FishBiteType = FishBiteType,
                Durability = Durability,
                DurabilityDenominator = DurabilityDenominator,
                Weight = Weight,
                Height = Height,
                CatchBonus = CatchBonus,
                HookBonus = HookBonus,
                IsEvent = IsEvent,
                ShopPrice = ShopPrice,
                PlayerPrice = PlayerPrice
            };
        }
    }

    public enum ItemType
    {
        Fish,
        FishingRod,
        Bait,
        Lure
    }
    
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Elite,
        Mythical,
        Legendary
    }

    /// <summary>
    /// Тип рыбы, а также приманки которая может привлечь рыбу схожего типа
    /// </summary>
    public enum FishType
    {
        Herbivorous, //растительноядные 
        Carnivorous, //плотоядные
        Combo
    }
}