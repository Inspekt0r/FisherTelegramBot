using System;
using System.ComponentModel.DataAnnotations;

namespace TelegramAspBot.Models.Entity
{
    public class BackpackItem
    {
        [Key]
        public int Id { get; set; }
        public virtual Item Item { get; set; }
        public string ItemName { get; set; }
        public ItemType ItemType { get; set; }
        public Rarity Rarity { get; set; }
        /// <summary>
        /// Что приманивает и каким предметом является
        /// </summary>
        public FishType FishBiteType { get; set; }
        public int Count { get; set; }
        public bool IsEquipped { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        /// <summary>
        /// Прочность предмета, скорость поломки определяется редкостью рыбы пойманой с его помощью
        /// </summary>
        public int Durability { get; set; } = 200;
        /// <summary>
        /// Параметр веса рыбы
        /// </summary>
        public double Weight { get; set; } 
        /// <summary>
        /// Параметр длины рыбы
        /// </summary>
        public double Height { get; set; }  
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
        public double DurabilityDenominator { get; set; } = 2.0;
        public bool IsEvent { get; set; }
        /// <summary>
        /// Цена предмета/рыбы при покупке в магазине
        /// </summary>
        public int ShopPrice { get; set; } = 0;
        /// <summary>
        /// Цена предмета при продаже в магазин
        /// </summary>
        public int PlayerPrice { get; set; } = 0;
        /// <summary>
        /// Узнать остаточную прочность предмета
        /// </summary>
        public double GetDurability()
        {
            return Math.Round((Durability / DurabilityDenominator), 0);
        }
        /// <summary>
        /// Пересчёт прочности в зависимости от пойманой рыбы (редкости)
        /// </summary>
        public void ReCalcDurability(Rarity rarity)
        {
            Durability -= rarity switch
            {
                Rarity.Common => 2,
                Rarity.Uncommon => 5,
                Rarity.Rare => 8,
                Rarity.Elite => 12,
                Rarity.Mythical => 15,
                Rarity.Legendary => 20,
                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };
        }
        /// <summary>
        /// Время которое действует приманка
        /// todo реализовать поле в Item которое будет отображать время действия для Bite в секундах
        /// </summary>
        public DateTime TimeElapsed { get; set; } = DateTime.UtcNow;

        public void FillBackpackItem()
        {
            ItemName = Item.Name;
            Rarity = Item.Rarity;
            ItemType = Item.ItemType;
        }
        public int GetFishCost()
        {
            if (IsEvent)
            {
                return 70;
            }
            
            if (ItemType != ItemType.Fish)
            {
                return 0;
            }

            var cost = (int) (CostByRare(Rarity) + (WeightCostBonus() * 3));

            return cost;
        }
        private static int CostByRare(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common => 10,
                Rarity.Uncommon => 20,
                Rarity.Rare => 30,
                Rarity.Elite => 50,
                Rarity.Mythical => 70,
                Rarity.Legendary => 100,
                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };
        }
        private int WeightCostBonus()
        {
            return (int) (Math.Sqrt(Weight) * 2);
        }
    }
}