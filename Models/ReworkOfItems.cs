using System.Linq;
using TelegramAspBot.Models.CreatingScript;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public static class ReworkOfItems
    {
        public static void LetsRockNRoll(ApplicationContext dbContext)
        {
            ClearItems(dbContext);
            CreateFishAsItem(dbContext);
            CreateShopItems(dbContext);
            CreateEventItems(dbContext);
            AddLure(dbContext);
        }
        private static void ClearItems(ApplicationContext dbContext)
        {
            var allBackpackItems = dbContext.BackpackItems.ToList();
            dbContext.RemoveRange(allBackpackItems);
            var allItems = dbContext.Items.ToList();
            dbContext.RemoveRange(allItems);
            dbContext.SaveChanges();
        }
        private static void CreateFishAsItem(ApplicationContext dbContext)
        {
            var fishGen = new FishPattern();
            var fishRefList = fishGen.GetFishes();
            var fishItemList = fishRefList.Select(fishRef => fishRef.ConvertFishRefToItem()).ToList();
            dbContext.AddRange(fishItemList);
            dbContext.SaveChanges();
        }

        private static void CreateShopItems(ApplicationContext dbContext)
        {
            var items = ShopSystem.GetItemsInShop();
            dbContext.AddRange(items);
            dbContext.SaveChanges();
        }

        private static void CreateEventItems(ApplicationContext dbContext)
        {
            var eventItems = AddItemReferenceOne.GetEventItemList();
            dbContext.AddRange(eventItems);
            dbContext.SaveChanges();
        }

        private static void AddLure(ApplicationContext dbContext)
        {
            var lure = new Item()
            {
                Name = "Приманка новичка",
                ItemType = ItemType.Lure,
                Rarity = Rarity.Legendary,
                CatchBonus = 0.7,
                IsForShopSale = false
            };
            dbContext.Add(lure);
            dbContext.SaveChanges();
        }
    }
}