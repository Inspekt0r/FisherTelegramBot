using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.SignalR.Protocol;
using Telegram.Bot;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class BuyAndSellSystem
    {
        private readonly Character _character;
        private readonly ITelegramBotClient _telegramBot;
        private readonly StringBuilder _sb = new StringBuilder();

        public BuyAndSellSystem(Character character, ITelegramBotClient telegramBot)
        {
            _character = character;
            _telegramBot = telegramBot;
        }

        public async Task BuyItemAsync(Item item)
        {
            if (item.IsEvent || !item.IsForShopSale)
            {
                _sb.AppendLine($"Нельзя купить этот предмет");
                await _telegramBot.SendTextMessageAsync(_character.TelegramId, _sb.ToString());
                return;
            }
            var bufPlayerMoney = _character.Money;
            bufPlayerMoney -= item.ShopPrice;

            if (bufPlayerMoney < 0)
            {
                _sb.AppendLine($"У тебя нет денежек на покупку {item.Name}");
            }
            else
            {
                if (CanBuyItem(item))
                {
                    _character.Money -= item.ShopPrice;
                    _sb.AppendLine($"Ты купил {item.Name} за {item.ShopPrice}");
                    AddItem(item);
                }
            }
            
            await _telegramBot.SendTextMessageAsync(_character.TelegramId, _sb.ToString());
            
            _sb.Clear();
        }
        
        public async Task SellItemAsync(BackpackItem backpackItem, int count)
        {
            var price = backpackItem.PlayerPrice;
            var durabilityMultiply = backpackItem.Durability / 200.0; 
            
            if (price == 0)
            {
                price = backpackItem.ShopPrice / 2;
            }

            if (backpackItem.ItemType == ItemType.Fish)
            {
                price = backpackItem.GetFishCost();
            }

            price = (int) (price * durabilityMultiply);

            if (backpackItem.Count <= 0 || backpackItem.IsDeleted)
            {
                await _telegramBot.SendTextMessageAsync(_character.TelegramId, $"У тебя нет этого предмета");
                return;
            }
            
            if (RemoveItem(backpackItem, count))
            {
                _character.Money += price * count;
                _sb.AppendLine($"Ты продал {backpackItem.ItemName} за {price * count}\nНа твоём балансе: {_character.Money}");
            }
            else
            {
                _sb.AppendLine($"Я не нашел такого предмета на продажу");
            }
            
            await _telegramBot.SendTextMessageAsync(_character.TelegramId, _sb.ToString());
            
            _sb.Clear();
        }

        public async Task MassTransfer(string rarity)
        {
            var rare = Parse(rarity);
            var rareItems = _character.Backpack.BackpackItems
                .Where(p => !p.IsDeleted && p.ItemType == ItemType.Fish && p.Rarity == rare)
                .ToList();
            if (rareItems.IsNullOrEmpty())
            {
                await _telegramBot.SendTextMessageAsync(_character.TelegramId,
                    $"Извини, но у тебя нет на продажу такой рыбы");
                return;
            }

            SellForEachItemResult(rareItems, rare);

            await _telegramBot.SendTextMessageAsync(_character.TelegramId, $"{_sb}");
            _sb.Clear();
        }

        private void SellForEachItemResult(List<BackpackItem> rareItems, Rarity rarity)
        {
            var totalCost = rareItems.Sum(p => p.GetFishCost());
            _sb.AppendLine($"Рыба {BackpackTextGenerator.GetRarityType(rarity)} была продана на сумму {totalCost}");
            rareItems.ForEach(p => p.IsDeleted = true);

            _character.Money += totalCost;
        }

        private static Rarity Parse(string rarity)
        {
            return rarity switch
            {
                "common" => Rarity.Common,
                "uncommon" => Rarity.Uncommon,
                "rare" => Rarity.Rare,
                "elite" => Rarity.Elite,
                "mythical" => Rarity.Mythical,
                "legendary" => Rarity.Legendary,
                _ => throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null)
            };
        }
        
        private void AddItem(Item item)
        {
            if (item.ItemType == ItemType.Lure)
            {
                //TODO поменять поиск соответсвия по ID на поиск по имени
                var existedItem = _character.Backpack.BackpackItems.FirstOrDefault(p => p.ItemName == item.Name);
                if (existedItem != null)
                {
                    existedItem.Count++;
                    existedItem.IsDeleted = false;
                    return;
                }
            }

            var backpackItem = item.ConvertToBackpack();
            backpackItem.Count = 1;
            _character.Backpack.BackpackItems.Add(backpackItem);
        }

        private bool RemoveItem(BackpackItem item, int count)
        {
            var backpackItems = _character.Backpack.BackpackItems.Where(p => !p.IsDeleted).ToList();
            foreach (var backpackItem in backpackItems)
            {
                if (backpackItem.ItemName == item.ItemName && backpackItem.Count >= count)
                {
                    backpackItem.Count-= count;
                    if (backpackItem.Count <= 0)
                    {
                        backpackItem.IsDeleted = true;
                    }
                    return true;
                }
            }

            return false;
        }

        private bool CanBuyItem(Item item)
        {
            var backpackItems = _character.Backpack.BackpackItems;
            
            switch (item.ItemType)
            {
                case ItemType.FishingRod:
                {
                    var fishingRodInBackpack = backpackItems
                        .Where(p => !p.IsDeleted && p.ItemType == ItemType.FishingRod).ToList();
                    if (fishingRodInBackpack.Count < 10)
                    {
                        return true;
                    }

                    _sb.AppendLine($"У тебя уже 10 удочек, больше вместить не получится");
                    return false;
                }
                case ItemType.Bait:
                {
                    var baitInBackpack = backpackItems
                        .Where(p => !p.IsDeleted && p.ItemType == ItemType.Bait).ToList();
                    if (baitInBackpack.Count < 10)
                    {
                        return true;
                    }

                    _sb.AppendLine($"У тебя уже 10 крючков/паучков, продай что-то или доломай");
                    return false;
                }
                case ItemType.Lure:
                {
                    var lureInBackpack = backpackItems
                        .Where(p => !p.IsDeleted && p.ItemType == ItemType.Lure).ToList();
                    if (lureInBackpack.Count < 5)
                    {
                        return true;
                    }

                    _sb.AppendLine($"В твоём рюкзаке 5 приманок, больше нельзя с собой носить :с");
                    return false;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}