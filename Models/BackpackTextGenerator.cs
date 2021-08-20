using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class BackpackTextGenerator
    {
        private readonly Character _character;
        private readonly StringBuilder _sb = new StringBuilder();

        private List<BackpackItem> _items;

        public BackpackTextGenerator(Character character)
        {
            _character = character;
        }

        public StringBuilder GetMessage(string text)
        {
            switch (text)
            {
                case "/backpack":
                {
                    GetMainMenu();
                }
                    break;
                case "/fishpack":
                {
                    SortingItem();
                    GetFishBackpack();
                }
                    break;
                case "/fishlure":
                {
                    SortingItem();
                    GetLureBackpack();
                }
                    break;
                case "/fishingear":
                {
                    SortingItem();
                    GetFishingGears();
                }
                    break;
                case "/fishingrod":
                {
                    SortingItem();
                    GetFishingRods();
                }
                    break;
                default:
                {
                    GetMainMenu();
                }
                    break;
            }
            return _sb;
        }
        private void GetMainMenu()
        {
            _sb.AppendLine($"<b>Рюкзак {_character.Name}:</b>");

            _sb.AppendLine($"Вся твоя рыба здесь: /fishpack");

            _sb.AppendLine($"Удочки положил сюда: /fishingrod");

            _sb.AppendLine($"Блёсна, воблеры, крючки: /fishingear");

            _sb.AppendLine($"Приманки /fishlure");
        }
        
        private void GetFishBackpack()
        {
            _sb.AppendLine($"<b>Рыба {_character.Name}:</b>");

            var backpackItems = _items
                .Where(p => p.ItemType == ItemType.Fish && p.IsDeleted == false)
                .ToList();
            if (backpackItems.Count > 20)
            {
                for (int i = 0; i < 20; i++)
                {
                    _sb.AppendLine($"<b>*</b> {GetRarityType(backpackItems[i].Rarity)} {backpackItems[i].ItemName}" +
                                   $"\nТип рыбы: {FishPediaTextGenerator.GetFishType(backpackItems[i].FishBiteType)} - " +
                                   $"{Math.Round(backpackItems[i].Weight, 2)}кг. {Math.Round(backpackItems[i].Height, 2)}м. - 🐟");
                }

                return;
            }
            foreach (var bpItems in backpackItems)
            {
                _sb.AppendLine($"<b>*</b> {GetRarityType(bpItems.Rarity)} {bpItems.ItemName}" +
                               $"\nТип рыбы: {FishPediaTextGenerator.GetFishType(bpItems.FishBiteType)} - {Math.Round(bpItems.Weight, 2)}кг. {Math.Round(bpItems.Height, 2)}м. - 🐟");
            }
        }

        private void GetFishingRods()
        {
            _sb.AppendLine($"<b>Удочки {_character.Name}:</b>");

            var backpackItems = _items
                .Where(p => p.ItemType == ItemType.FishingRod && p.IsDeleted == false)
                .ToList();
            foreach (var bpItems in backpackItems)
            {
                _sb.AppendLine($"<b>*</b> {GetRarityType(bpItems.Rarity)} {bpItems.ItemName}" +
                               $"\nТип рыбы: {FishPediaTextGenerator.GetFishType(bpItems.FishBiteType)} {(int) (bpItems.CatchBonus * 1000)}🔼" +
                               $"\nПрочность: {bpItems.GetDurability()}/100%" +
                               $"\n{GenerateTextForEquipped(bpItems)} 🎣");
            }
        }
        
        private void GetFishingGears()
        {
            _sb.AppendLine($"<b>Крючки паучки букашки и жучки {_character.Name}:</b>");

            var backpackItems = _items
                .Where(p => p.ItemType == ItemType.Bait && p.IsDeleted == false)
                .ToList();
            foreach (var bpItems in backpackItems)
            {
                _sb.AppendLine($"<b>*</b> {GetRarityType(bpItems.Rarity)} {bpItems.ItemName}" +
                               $"\nТип рыбы: {FishPediaTextGenerator.GetFishType(bpItems.FishBiteType)} {(int) (bpItems.CatchBonus * 1000)}🔼" +
                               $"\nПрочность: {bpItems.GetDurability()}/100%" +
                               $"\n{GenerateTextForEquipped(bpItems)} 🎣");
            }
        }

        private string GenerateTextForEquipped(BackpackItem backpackItem)
        {
            if (backpackItem.IsEquipped)
            {
                return $"Снять /unequip_{backpackItem.Id}";
            }
            else
            {
                return $"Экипировать /equip_{backpackItem.Id}";
            }
        }

        private void GetLureBackpack()
        {
            _sb.AppendLine($"<b>Приманки {_character.Name}:</b>");
            
            var backpackItems = _items
                .Where(p => p.ItemType == ItemType.Lure && p.IsDeleted == false && p.Count > 0)
                .ToList();
            foreach (var bpItems in backpackItems)
            {
                _sb.AppendLine($"<b>*</b> {GetRarityType(bpItems.Rarity)} {bpItems.ItemName} - {bpItems.Count}" +
                               $"\nТип рыбы: {FishPediaTextGenerator.GetFishType(bpItems.FishBiteType)} {(int) (bpItems.CatchBonus * 1000)}🔼" +
                               $"\n/lure_{bpItems.Id} ");
            }
        }

        public static string GetRarityType(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common => "⬜️",
                Rarity.Uncommon => "⬛️",
                Rarity.Rare => "🟦",
                Rarity.Elite => "🟩",
                Rarity.Mythical => "🟪",
                Rarity.Legendary => "🟧",
                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };
        }

        private void SortingItem()
        {
            _items = _character.Backpack.BackpackItems
                .OrderBy(p => p.Rarity)
                .ThenBy(p => p.ItemName)
                .ToList();   
        }
    }
}