using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class SellFish
    {
        private readonly Character _character;
        private readonly StringBuilder _sb = new StringBuilder();

        public SellFish(Character character)
        {
            _character = character;
        }

        public StringBuilder GenerateFishSellText()
        {
            _sb.AppendLine($"Твой список рыбы на продажу:");
            var fishInBackpack =
                _character.Backpack.BackpackItems
                    .Where(p => p.IsDeleted == false && p.ItemType == ItemType.Fish)
                    .ToList();
            GenerateMassTransferText(fishInBackpack);
            foreach (var fish in fishInBackpack)
            {
                _sb.AppendLine($"{FishHandling(fish)}");
            }
            
            return _sb;
        }

        private void GenerateMassTransferText(IReadOnlyCollection<BackpackItem> fishItems)
        {
            var common = fishItems.FirstOrDefault(p => p.ItemType == ItemType.Fish && p.Rarity == Rarity.Common);
            var uncommon = fishItems.FirstOrDefault(p => p.ItemType == ItemType.Fish && p.Rarity == Rarity.Uncommon);
            var rare = fishItems.FirstOrDefault(p => p.ItemType == ItemType.Fish && p.Rarity == Rarity.Rare);
            var elite = fishItems.FirstOrDefault(p => p.ItemType == ItemType.Fish && p.Rarity == Rarity.Elite);
            var mythical = fishItems.FirstOrDefault(p => p.ItemType == ItemType.Fish && p.Rarity == Rarity.Mythical);
            var legendary = fishItems.FirstOrDefault(p => p.ItemType == ItemType.Fish && p.Rarity == Rarity.Legendary);
            
            if (common != null)
            {
                _sb.AppendLine($"Продать всю ⬜️ рыбу /transfer_common");
            }

            if (uncommon != null)
            {
                _sb.AppendLine($"Продать всю ⬛️ рыбу /transfer_uncommon");
            }
            
            if (rare != null)
            {
                _sb.AppendLine($"Продать всю 🟦 рыбу /transfer_rare");
            }
            
            if (elite != null)
            {
                _sb.AppendLine($"Продать всю 🟩 рыбу /transfer_elite");
            }
            
            if (mythical != null)
            {
                _sb.AppendLine($"Продать всю 🟪 рыбу /transfer_mythical");
            }
            
            if (legendary != null)
            {
                _sb.AppendLine($"Продать всю 🟧 рыбу /transfer_legendary");
            }

            _sb.AppendLine();
        }
        private static string FishHandling(BackpackItem fish)
        {
            return $"<b>*</b> <i>{fish.ItemName}</i> - {fish.GetFishCost()} /sell_{fish.Id}";
        }

        public StringBuilder TrySellFish(int backpackFishId)
        {
            var fish = _character.Backpack.BackpackItems.FirstOrDefault(p => p.IsDeleted == false &&
                                                                             p.Id == backpackFishId && p.ItemType == ItemType.Fish);

            if (fish == null)
            {
                _sb.AppendLine($"Такого предмета у тебя не нашлось");
            }

            else
            {
                _sb.AppendLine($"{fish.ItemName} приносит тебе {fish.GetFishCost()}");
                _character.Money += fish.GetFishCost();
                
                fish.IsDeleted = true;
            }
            return _sb;
        }
    }
}