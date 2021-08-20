using System;
using System.Collections.Generic;
using System.Linq;
using TelegramAspBot.Job;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class WeightedFishingModule
    {
        private readonly Character _character;
        private readonly Random _random = new Random();
        private readonly bool _isHooked;
        private readonly JobLure _activeLure;

        public WeightedFishingModule(bool isHooked, Character character, JobLure activeLure)
        {
            _isHooked = isHooked;
            _character = character;
            _activeLure = activeLure;
        }
        
        public BackpackItem CatchFishOrDefault()
        {
            var fishList = _character.Spot.FishReferenceSpots.ToList();
            var sumFishWeight = fishList.Sum(p => FishWeightByRarity(p.FishReference.Rarity));
            var eventFish = GetEventFishName();
            var fishCatchInSpot = fishList.FirstOrDefault(
                p => Randomizer(sumFishWeight, p.FishReference, eventFish));
            
            return fishCatchInSpot == null ? null : BoxingFishReference(fishCatchInSpot.FishReference);
        }

        public BackpackItem CatchFishTest(List<FishReferenceSpot> fishList)
        {
            if (fishList == null)
            {
                return null;
            }
            var sumFishWeight = fishList.Sum(p => FishWeightByRarity(p.FishReference.Rarity));
            var fishCatchInSpot = fishList.FirstOrDefault(
                p => Randomizer(sumFishWeight, p.FishReference, null));
            
            return fishCatchInSpot == null ? null : BoxingFishReference(fishCatchInSpot.FishReference);
        }

        private BackpackItem BoxingFishReference(FishReference fishReference)
        {
            var multiply = _random.Next(50, 101) / 100.0;
            var generateFish = new BackpackItem
            {
                ItemName = fishReference.Name,
                FishBiteType = fishReference.FishType,
                Rarity = fishReference.Rarity,
                Height = fishReference.MaxHeight * multiply,
                Weight = fishReference.MaxWeight * multiply,
                Count = 1
            };
            return generateFish;
        }

        private bool Randomizer(int sumFishWeight, FishReference fishReference, string fishName)
        {
            var finalFishWeight = fishName == null 
                ? FishTestWeightByRarity(fishReference.Rarity)
                : FishWeightByRarity(fishReference.Rarity);

            if (_isHooked)
            {
                finalFishWeight += sumFishWeight / 40;
            }

            finalFishWeight += fishName == null
                ? GetTestFishingRodBonus(fishReference.FishType, sumFishWeight)
                : GetFishingRodBonus(sumFishWeight, fishReference.FishType);
            finalFishWeight += GetBaitBonus(sumFishWeight, fishReference.FishType);
            if (_activeLure != null)
            {
                finalFishWeight += GetLureBonus(sumFishWeight, fishReference.FishType);
            }

            finalFishWeight += UnluckyBonus();

            if (fishName != null && fishName == fishReference.Name)
            {
                finalFishWeight += sumFishWeight / 20;
            }
            return fishName == null ? _random.Next(0, sumFishWeight) < finalFishWeight
                : _random.Next(0, sumFishWeight) - finalFishWeight < 0;
            return _random.Next(0, sumFishWeight) - finalFishWeight < 0;
            //TODO Сделать погоду и бонусы от погоды
        }

        private static string GetEventFishName()
        {
            var dbContext = new ApplicationContext();
            var currentEvent = dbContext.Events.FirstOrDefault(p => p.IsActive);
            return currentEvent != null ? currentEvent.FishName : string.Empty;
        }

        private int UnluckyBonus()
        {
            return _character.CharStat.UnluckyTry * 100;
        }
        private int GetLureBonus(int sumFishWeight, FishType fishType)
        {
            var weightBonus = sumFishWeight / 15;
            if (_activeLure.LureItem.FishBiteType == fishType)
            {
                return (int) (_activeLure.LureItem.CatchBonus * 1000) + weightBonus;
            }
            
            if (fishType == FishType.Combo)
            {
                return (int) (_activeLure.LureItem.CatchBonus * 500) + weightBonus;
            }

            return 0;
        }
        
        private int GetBaitBonus(int sumFishWeight, FishType fishType)
        {
            var weightBonus = sumFishWeight / 20;
            var bait = _character.Backpack.BackpackItems.FirstOrDefault(p =>
                p.IsEquipped && p.ItemType == ItemType.Bait);
            if (bait == null)
            {
                return 0;
            }
            
            if (bait.FishBiteType == fishType)
            {
                return (int) (bait.CatchBonus * 1000) + weightBonus;
            }
            
            if (fishType == FishType.Combo)
            {
                return (int) (bait.CatchBonus * 500) + weightBonus;
            }
            
            return 0;
        }
        private int GetFishingRodBonus(int sumFishWeight, FishType fishType)
        {
            var weightBonus = sumFishWeight / 10;
            var fishingRod =
                _character.Backpack.BackpackItems.FirstOrDefault(p =>
                    p.IsEquipped && p.ItemType == ItemType.FishingRod);
            
            if (fishingRod == null)
            {
                return 0;
            }

            if (fishingRod.FishBiteType == fishType)
            {
                return (int) (fishingRod.CatchBonus * 1000) + weightBonus;
            }

            if (fishType == FishType.Combo)
            {
                return (int) (fishingRod.CatchBonus * 500) + weightBonus;
            }

            return 0;
        }
        private int GetTestFishingRodBonus(FishType fishType, int sumFishWeight)
        {
            var fishingRod =
                _character.Backpack.BackpackItems.FirstOrDefault(p =>
                    p.IsEquipped && p.ItemType == ItemType.FishingRod);
            
            if (fishingRod == null)
            {
                return 0;
            }
            var fivePercentOfSum = sumFishWeight / 20;
            if (fishingRod.FishBiteType == fishType)
            {
                
                return (int) (fishingRod.CatchBonus * fivePercentOfSum);
            }

            if (fishingRod.FishBiteType == FishType.Combo)
            {
                return (int) (fishingRod.CatchBonus * fivePercentOfSum);
            }

            return 0;
        }
        private static int FishWeightByRarity(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common => 10000,
                Rarity.Uncommon => 8020,
                Rarity.Rare => 5010,
                Rarity.Elite => 2510,
                Rarity.Mythical => 500,
                Rarity.Legendary => 60,
                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };
        }
        private static int FishTestWeightByRarity(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common => 8010,
                Rarity.Uncommon => 8510,
                Rarity.Rare => 5060,
                Rarity.Elite => 2560,
                Rarity.Mythical => 350,
                Rarity.Legendary => 60,
                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };
        }
    }
}
