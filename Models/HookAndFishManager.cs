using System;
using System.Linq;
using TelegramAspBot.Job;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class HookAndFishManager
    {
        private readonly Character _character;
        private readonly Random _random = new Random();
        private readonly JobManager _jobManager;

        public HookAndFishManager(Character character, JobManager jobManager)
        {
            _character = character;
            _jobManager = jobManager;
        }
        //todo добавить шанс подсечки в зависимости от уровня рыбака(?)
        public bool IsHooked()
        {
            var fishingRod = EquippedFishingRod();
            var bait = EquippedBait();
            var baitBonus = 0.005;
            var fishingRodBonus = 0.01;
            
            if (bait != null)
            {
                baitBonus = bait.CatchBonus;
            }

            if (fishingRod != null)
            {
                fishingRodBonus = fishingRod.HookBonus;
            }
            
            var probability =
                CalculateProbability(fishingRodBonus, 1.0, baitBonus, 0.70);

            var generatedProbability = GenerateRandomProbability();

            return generatedProbability <= probability;
        }

        public bool IsFished(double hookBonus, Item fish)
        {
            var fishCaptureRate = 0.0;
            if (hookBonus == 0)
            {
                fishCaptureRate = (GetCatchRateByFishRarity(fish.Rarity) / 2.0) + UnluckyBonus(_character.CharStat.UnluckyTry);
            }
            else
            {
                fishCaptureRate = (GetCatchRateByFishRarity(fish.Rarity) * 0.75) + hookBonus + UnluckyBonus(_character.CharStat.UnluckyTry);
            }
            
            if (fishCaptureRate > 0.999)
            {
                return true;
            }
            
            var fishingRod = EquippedFishingRod();
            var bait = EquippedBait();
            var baitBonus = 0.005;
            var fishingRodBonus = 0.01;
            
            if (bait != null)
            {
                baitBonus = bait.CatchBonus;
                bait.ReCalcDurability(fish.Rarity);
            }

            if (fishingRod != null)
            {
                fishingRodBonus = fishingRod.CatchBonus;
            }

            var probability = 0.0;
            if (_character.Spot.CatchBonus > 0)
            {
                probability = CalculateProbabilitySpotWithBonus(fishingRodBonus,
                    _jobManager.GetLureRate(_character), baitBonus,
                    fishCaptureRate, _character.Spot.CatchBonus);
            }
            else
            {
                probability = CalculateProbability(fishingRodBonus, _jobManager.GetLureRate(_character), baitBonus,
                    fishCaptureRate);
            }
            var generatedProbability = GenerateRandomProbability();

            var fishingResult = generatedProbability <= probability;

            if (fishingResult)
            {
                fishingRod?.ReCalcDurability(fish.Rarity);
            }

            return fishingResult;
        }

        private static double GetCatchRateByFishRarity(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common => 0.9,
                Rarity.Uncommon => 0.75,
                Rarity.Rare => 0.6,
                Rarity.Elite => 0.45,
                Rarity.Mythical => 0.35,
                Rarity.Legendary => 0.1,
                _ => 1.00
            };
        }

        private BackpackItem EquippedFishingRod()
        {
            var fishingRod = _character.Backpack.BackpackItems.FirstOrDefault(p =>
                p.IsEquipped && p.ItemType == ItemType.FishingRod);
            if (fishingRod != null)
            {
                return fishingRod;
            }
            else
            {
                return null;
            }
        }

        private BackpackItem EquippedBait()
        {
            var bait = _character.Backpack.BackpackItems.FirstOrDefault(p =>
                p.IsEquipped && p.ItemType == ItemType.Bait);
            return bait;
        }

        private static double CalculateProbability(double fishingRodRate, double lureRate, double baitRate,
            double fishCaptureRate)
        {
            var probability = (fishCaptureRate + fishingRodRate + lureRate + baitRate) / 4;
            
            probability = Math.Sqrt(probability);
            
            return probability;
        }
        
        private static double CalculateProbabilitySpotWithBonus(double fishingRodRate, double lureRate, double baitRate,
            double fishCaptureRate, double spotBonus)
        {
            var probability = (fishCaptureRate + fishingRodRate + lureRate + baitRate + spotBonus) / 5;
            
            probability = Math.Sqrt(probability);
            
            return probability;
        }

        private double GenerateRandomProbability()
        {
            var generateNumOne = _random.Next(0, 101);
            var generateNumTwo = _random.Next(0, 101);
            var generateNumThree = _random.Next(0, 101);
            
            var average = (generateNumOne + generateNumTwo + generateNumThree) / 3;
            
            if (average <= generateNumOne && average <= generateNumTwo)
            {
                generateNumThree = _random.Next(0, 101);
                return generateNumThree / 100.0;
            }
            
            if (average <= generateNumOne && average <= generateNumThree)
            {
                generateNumTwo = _random.Next(0, 101);
                return generateNumTwo / 100.0;
            }
            
            generateNumOne = _random.Next(0, 101);
            return generateNumOne / 100.0;
        }

        private static double UnluckyBonus(int count)
        {
            if (count > 15)
            {
                return 0.45;
            }

            if (count > 10)
            {
                return 0.25;
            }

            if (count > 4)
            {
                return 0.15;
            }

            return 0.0;
        }
    }
}