using System.Collections.Generic;
using System.Linq;
using TelegramAspBot.Models;
using TelegramAspBot.Models.Entity;
using TelegramAspBot.Models.Enum;
using TelegramAspBot.Models.LocationModule;

namespace TelegramAspBot
{
    public class CreateBaseEntity
    {
        private static void CreateShopItem(ApplicationContext dbContext)
        {
            var characters = dbContext.Characters.ToList();
            foreach (var character in characters)
            {
                character.CharState = State.Idle;
            }

            var shopItems = ShopSystem.GetItemsInShop();

            var existingItems = dbContext.Items.ToList();

            var itemsAddedToDatabase = new List<Item>();

            foreach (var shopItem in shopItems)
            {
                bool isExist = false;
                foreach (var existingItem in existingItems)
                {
                    if (existingItem.Name == shopItem.Name)
                    {
                        isExist = true;
                    }
                }

                if (!isExist)
                {
                    itemsAddedToDatabase.Add(shopItem);
                }
            }

            dbContext.AddRange(itemsAddedToDatabase);
        }

        private static void CreateSpots(ApplicationContext dbContext)
        {
            var existingSpots = dbContext.Spots.ToList();

            var spotStorage = LocationStorage.Spots;

            var itemsAddedToDatabase = new List<Spot>();

            foreach (var spotItem in spotStorage)
            {
                bool isExist = false;
                foreach (var existingSpot in existingSpots)
                {
                    if (existingSpot.Name == spotItem.Name)
                    {
                        isExist = true;
                    }
                }

                if (!isExist)
                {
                    itemsAddedToDatabase.Add(spotItem);
                }
            }
            
            dbContext.AddRange(itemsAddedToDatabase);
        }

        private static void CreateFishReferences(ApplicationContext dbContext)
        {
            var existingFishList = dbContext.FishReferences.ToList();
            
            var fishStorage = new FishPattern().GetFishes();
            
            var itemsAddedToDatabase = new List<FishReference>();
            
            foreach (var fish in fishStorage)
            {
                bool isExist = false;
                foreach (var existingFish in existingFishList)
                {
                    if (fish.Name == existingFish.Name)
                    {
                        isExist = true;
                    }
                }

                if (!isExist)
                {
                    itemsAddedToDatabase.Add(fish);
                }
            }
            
            dbContext.AddRange(itemsAddedToDatabase);
            //dbContext.SaveChanges();
        }

        public static void CreateOrUpdateAll()
        {
            using var dbContext = new ApplicationContext();
            CreateShopItem(dbContext);
            CreateSpots(dbContext);
            CreateFishReferences(dbContext);
            dbContext.SaveChanges();
            
            LocationStorage.UpdateSpotByFishRef(dbContext);
            var gS = new GlobalSetting() { LureTimeMinutes = 30};
            dbContext.Add(gS);

            dbContext.SaveChanges();
        }
    }
}