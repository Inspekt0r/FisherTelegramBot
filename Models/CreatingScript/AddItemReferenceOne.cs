using System.Collections.Generic;
using TelegramAspBot.Models.Entity;
using TelegramAspBot.Models.Enum;

namespace TelegramAspBot.Models.CreatingScript
{
    public static class AddItemReferenceOne
    {
        public static List<Item> GetEventItemList()
        {
            var listReferences = new List<Item>
            {
                new Item()
                {
                    Name = "Удочка мастера",
                    ItemType = ItemType.FishingRod,
                    Rarity = Rarity.Elite,
                    CatchBonus = 0.35,
                    HookBonus = 0.3,
                    FishBiteType = FishType.Herbivorous,
                    ShopPrice = 1000,
                    PlayerPrice = 3000,
                    IsEvent = true,
                    EventPosition = EventPosition.First
                },
                new Item()
                {
                    Name = "Удочка подмастерья",
                    ItemType = ItemType.FishingRod,
                    Rarity = Rarity.Rare,
                    CatchBonus = 0.25,
                    HookBonus = 0.2,
                    FishBiteType = FishType.Herbivorous,
                    ShopPrice = 750,
                    PlayerPrice = 2000,
                    IsEvent = true,
                    EventPosition = EventPosition.Second
                },
                new Item()
                {
                    Name = "Потёртая удочка",
                    ItemType = ItemType.FishingRod,
                    Rarity = Rarity.Rare,
                    CatchBonus = 0.2,
                    HookBonus = 0.2,
                    FishBiteType = FishType.Herbivorous,
                    ShopPrice = 550,
                    PlayerPrice = 1500,
                    IsEvent = true,
                    EventPosition = EventPosition.Third
                },
                new Item()
                {
                    Name = "Еле живая удочка",
                    ItemType = ItemType.FishingRod,
                    Rarity = Rarity.Common,
                    CatchBonus = 0.15,
                    HookBonus = 0.15,
                    FishBiteType = FishType.Herbivorous,
                    ShopPrice = 250,
                    PlayerPrice = 500,
                    IsEvent = true,
                    EventPosition = EventPosition.Other
                },
                new Item()
                {
                    Name = "Спининг мастера",
                    ItemType = ItemType.FishingRod,
                    Rarity = Rarity.Elite,
                    CatchBonus = 0.35,
                    HookBonus = 0.3,
                    FishBiteType = FishType.Combo,
                    ShopPrice = 1000,
                    PlayerPrice = 3000,
                    IsEvent = true,
                    EventPosition = EventPosition.First
                },
                new Item()
                {
                    Name = "Спининг подмастерья",
                    ItemType = ItemType.FishingRod,
                    Rarity = Rarity.Rare,
                    CatchBonus = 0.25,
                    HookBonus = 0.2,
                    FishBiteType = FishType.Combo,
                    ShopPrice = 750,
                    PlayerPrice = 2000,
                    IsEvent = true,
                    EventPosition = EventPosition.Second
                },
                new Item()
                {
                    Name = "Потёртый спининг",
                    ItemType = ItemType.FishingRod,
                    Rarity = Rarity.Rare,
                    CatchBonus = 0.2,
                    HookBonus = 0.2,
                    FishBiteType = FishType.Combo,
                    ShopPrice = 550,
                    PlayerPrice = 1500,
                    IsEvent = true,
                    EventPosition = EventPosition.Third
                },
                new Item()
                {
                    Name = "Еле живой спининг",
                    ItemType = ItemType.FishingRod,
                    Rarity = Rarity.Common,
                    CatchBonus = 0.15,
                    HookBonus = 0.15,
                    FishBiteType = FishType.Combo,
                    ShopPrice = 250,
                    PlayerPrice = 500,
                    IsEvent = true,
                    EventPosition = EventPosition.Other
                },
                new Item()
                {
                    Name = "Бамбуковое удилище мастера",
                    ItemType = ItemType.FishingRod,
                    Rarity = Rarity.Elite,
                    CatchBonus = 0.35,
                    HookBonus = 0.3,
                    FishBiteType = FishType.Carnivorous,
                    ShopPrice = 1000,
                    PlayerPrice = 3000,
                    IsEvent = true,
                    EventPosition = EventPosition.First
                },
                new Item()
                {
                    Name = "Бамбуковое удилище подмастерья",
                    ItemType = ItemType.FishingRod,
                    Rarity = Rarity.Rare,
                    CatchBonus = 0.25,
                    HookBonus = 0.2,
                    FishBiteType = FishType.Carnivorous,
                    ShopPrice = 750,
                    PlayerPrice = 2000,
                    IsEvent = true,
                    EventPosition = EventPosition.Second
                },
                new Item()
                {
                    Name = "Потёртое бамбуковое удилище",
                    ItemType = ItemType.FishingRod,
                    Rarity = Rarity.Rare,
                    CatchBonus = 0.2,
                    HookBonus = 0.2,
                    FishBiteType = FishType.Carnivorous,
                    ShopPrice = 550,
                    PlayerPrice = 1500,
                    IsEvent = true,
                    EventPosition = EventPosition.Third
                },
                new Item()
                {
                    Name = "Еле живое бамбуковое удилище",
                    ItemType = ItemType.FishingRod,
                    Rarity = Rarity.Common,
                    CatchBonus = 0.15,
                    HookBonus = 0.15,
                    FishBiteType = FishType.Carnivorous,
                    ShopPrice = 250,
                    PlayerPrice = 500,
                    IsEvent = true,
                    EventPosition = EventPosition.Other
                }
            };

            return listReferences;
        }
    }
}