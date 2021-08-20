using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using MoreLinq.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models
{
    public class ShopSystem
    {
        private readonly Character _character;
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly ITelegramBotClient _telegramBot;

        public ShopSystem(Character character, ITelegramBotClient telegramBot)
        {
            _character = character;
            _telegramBot = telegramBot;
        }

        public async Task SendShopListAsync()
        {
            GenerateShopText();
            await _telegramBot.SendTextMessageAsync(_character.TelegramId, _sb.ToString(), ParseMode.Html, replyMarkup: CallBackKeyboard.GetShopKeyboard(_character));
            _sb.Clear();
        }

        public async Task EditShopMessageAsync(CallbackQuery message)
        {
            await _telegramBot.AnswerCallbackQueryAsync(message.Id);
            
            await _telegramBot.EditMessageTextAsync(_character.TelegramId, message.Message.MessageId, _sb.ToString(), ParseMode.Html, replyMarkup: CallBackKeyboard.GetShopKeyboard(_character));
            
            _sb.Clear();
        }

        private void GenerateShopText()
        {
            _sb.AppendLine($"<b>Привет {_character.Name}!</b>");
            _sb.AppendLine($"Выбирай что тебе нужно");
        }

        public void AddTextComboItems()
        {
            using var dbContext = new ApplicationContext();
            var shopItems = dbContext.Items.Where(p => p.ShopPrice > 0 
                                                       && p.FishBiteType == FishType.Combo 
                                                       && p.ItemType == ItemType.Bait
                                                       && p.IsForShopSale).ToList();
            shopItems = shopItems.OrderBy(p => p.Rarity)
                .ThenBy(p => p.ShopPrice)
                .ToList();
            _sb.AppendLine($"<b>Комбо штуки, подойдут для любой рыбы:</b>");
            foreach (var shopItem in shopItems)
            {
                _sb.AppendLine($"*{BackpackTextGenerator.GetRarityType(shopItem.Rarity)} {shopItem.Name} " +
                               $"{(int) (shopItem.CatchBonus * 1000)}🔼 - 💰{shopItem.ShopPrice} купить: /buy_{shopItem.Id}");
            }
        }
        
        public void AddTextHerbivorousItems()
        {
            using var dbContext = new ApplicationContext();
            var shopItems = dbContext.Items.Where(p => p.ShopPrice > 0 
                                                       && p.FishBiteType == FishType.Herbivorous 
                                                       && p.ItemType == ItemType.Bait
                                                       && p.IsForShopSale).ToList();
            shopItems = shopItems.OrderBy(p => p.Rarity)
                .ThenBy(p => p.ShopPrice)
                .ToList();
            _sb.AppendLine($"<b>Всё для нехищной рыбы</b>");
            foreach (var shopItem in shopItems)
            {
                _sb.AppendLine($"*{BackpackTextGenerator.GetRarityType(shopItem.Rarity)} {shopItem.Name} " +
                               $"{(int) (shopItem.CatchBonus * 1000)}🔼 - 💰{shopItem.ShopPrice} купить: /buy_{shopItem.Id}");
            }
        }
        
        public void AddTextCarnivorousItems()
        {
            using var dbContext = new ApplicationContext();
            var shopItems = dbContext.Items.Where(p => p.ShopPrice > 0 
                                                       && p.FishBiteType == FishType.Carnivorous 
                                                       && p.ItemType == ItemType.Bait
                                                       && p.IsForShopSale).ToList();
            shopItems = shopItems.OrderBy(p => p.Rarity)
                .ThenBy(p => p.ShopPrice)
                .ToList();
            _sb.AppendLine($"<b>Всё для хищной рыбы</b>");
            foreach (var shopItem in shopItems)
            {
                _sb.AppendLine($"*{BackpackTextGenerator.GetRarityType(shopItem.Rarity)} {shopItem.Name} " +
                               $"{(int) (shopItem.CatchBonus * 1000)}🔼 - 💰{shopItem.ShopPrice} купить: /buy_{shopItem.Id}");
            }
        }

        public void AddTextFishingRods()
        {
            //todo сделать тип IsSell который будет отвечать за возможность покупки предмета у торговца
            using var dbContext = new ApplicationContext();
            var shopItems = dbContext.Items.Where(p => p.ShopPrice > 0 
                                                       && p.ItemType == ItemType.FishingRod 
                                                       && p.IsForShopSale).ToList();
            shopItems = shopItems.OrderBy(p => p.Rarity)
                .ThenBy(p => p.ShopPrice)
                .ToList();
            _sb.AppendLine($"<b>А вот и удочки:</b>");
            foreach (var shopItem in shopItems)
            {
                _sb.AppendLine($"*{BackpackTextGenerator.GetRarityType(shopItem.Rarity)} {shopItem.Name} " +
                               $"<i>{FishPediaTextGenerator.GetFishType(shopItem.FishBiteType)}</i> {(int) (shopItem.CatchBonus * 1000)}🔼 " +
                               $"- 💰{shopItem.ShopPrice} купить: /buy_{shopItem.Id}");
            }
        }
        
        public void AddTextLures()
        {
            using var dbContext = new ApplicationContext();
            var shopItems = dbContext.Items.Where(p => p.ShopPrice > 0 
                                                       && p.ItemType == ItemType.Lure 
                                                       && p.IsForShopSale).ToList();
            shopItems = shopItems.OrderBy(p => p.Rarity)
                .ThenBy(p => p.ShopPrice)
                .ToList();
            _sb.AppendLine($"<b>Лучший прикорм!</b>");
            foreach (var shopItem in shopItems)
            {
                _sb.AppendLine($"*{BackpackTextGenerator.GetRarityType(shopItem.Rarity)} {shopItem.Name} " +
                               $"<i>{FishPediaTextGenerator.GetFishType(shopItem.FishBiteType)}</i> {(int) (shopItem.CatchBonus * 1000)}🔼 " +
                               $"- 💰{shopItem.ShopPrice} купить: /buy_{shopItem.Id}");
            }
        }

        public void AddSellDevices()
        {
            _sb.AppendLine($"<b>Всё, что ты можешь продать из девайсов:</b>");
            var bodyFlag = false;
            var count = 0;
            var sellBackpackItems = _character.Backpack.BackpackItems
                .Where(p => !p.IsDeleted && p.Count > 0 && p.ItemType != ItemType.Fish)
                .OrderBy(p => p.Rarity)
                .ThenBy(p => p.ItemName)
                .ToList();
            //todo сделать пролистывание списка продажи
            foreach (var backpackItem in sellBackpackItems)
            {
                if (count == 30)
                {
                    break;
                }
                
                bodyFlag = true;
                
                _sb.AppendLine(GetSellingItemNaming(backpackItem));
                
                count++;
            }

            if (!bodyFlag)
            {
                _sb.AppendLine($"<i>У тебя ничего нет</i>");
            }
        }
        
        public void AddSellFishes()
        {
            _sb.AppendLine($"<b>Вся твоя рыба:</b>");
            var bodyFlag = false;
            var count = 0;
            var sellBackpackItems = _character.Backpack.BackpackItems
                .Where(p => !p.IsDeleted && p.Count > 0 && p.ItemType == ItemType.Fish)
                .OrderBy(p => p.Rarity)
                .ThenBy(p => p.ItemName)
                .ToList();
            GenerateMassTransferText(sellBackpackItems);
            //todo сделать пролистывание списка продажи
            foreach (var backpackItem in sellBackpackItems)
            {
                if (count == 30)
                {
                    break;
                }
                
                bodyFlag = true;
                
                _sb.AppendLine(GetSellingItemNaming(backpackItem));
                
                count++;
            }

            if (!bodyFlag)
            {
                _sb.AppendLine($"<i>У тебя ничего нет</i>");
            }
        }
        private void GenerateMassTransferText(List<BackpackItem> fishItems)
        {
            var common = fishItems.FirstOrDefault(p => p.Rarity == Rarity.Common);
            var uncommon = fishItems.FirstOrDefault(p => p.Rarity == Rarity.Uncommon);
            var rare = fishItems.FirstOrDefault(p => p.Rarity == Rarity.Rare);
            var elite = fishItems.FirstOrDefault(p => p.Rarity == Rarity.Elite);
            var mythical = fishItems.FirstOrDefault(p => p.Rarity == Rarity.Mythical);
            var legendary = fishItems.FirstOrDefault(p => p.Rarity == Rarity.Legendary);
            
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

        private string GetSellingItemNaming(BackpackItem backpackItem)
        {
            var price = backpackItem.PlayerPrice;
            
            var durabilityMultiply = backpackItem.Durability / 200.0;
            
            if (price == 0)
            {
                price = backpackItem.ShopPrice / 2;
            }
            
            if (backpackItem.ItemType == ItemType.Fish)
            {
                return $"*{BackpackTextGenerator.GetRarityType(backpackItem.Rarity)} {backpackItem.ItemName} 💰{backpackItem.GetFishCost()} /sell_{backpackItem.Id}";
            }
            
            price = (int) (price * durabilityMultiply);

            return
                $"*{BackpackTextGenerator.GetRarityType(backpackItem.Rarity)} {backpackItem.ItemName} 💰{price}({price * backpackItem.Count}), кол-во: {backpackItem.Count}\n" +
                $"продать: /sell_{backpackItem.Id}, продать всё: /sell_{backpackItem.Id}_{backpackItem.Count}";
        }
        public static List<Item> GetItemsInShop()
        {
            return new List<Item>()
            {
                new Item() { Name = "Простой крючок", Rarity = Rarity.Common, CatchBonus = 0.01, FishBiteType = FishType.Combo, ShopPrice = 10, ItemType = ItemType.Bait, IsForShopSale = true},
                new Item() { Name = "Блесна", Rarity = Rarity.Common, CatchBonus = 0.05, FishBiteType = FishType.Combo, ShopPrice = 50, ItemType = ItemType.Bait, IsForShopSale = true },
                new Item() { Name = "Дождевой червь", Rarity = Rarity.Common, CatchBonus = 0.1, FishBiteType = FishType.Carnivorous, ShopPrice = 100, ItemType = ItemType.Bait, IsForShopSale = true },
                new Item() { Name = "Хлеб", Rarity = Rarity.Common, CatchBonus = 0.1, FishBiteType = FishType.Herbivorous, ShopPrice = 100, ItemType = ItemType.Bait, IsForShopSale = true },
                new Item() { Name = "Воблер", Rarity = Rarity.Common, CatchBonus = 0.2, FishBiteType = FishType.Carnivorous, ShopPrice = 200, ItemType = ItemType.Bait, IsForShopSale = true },
                new Item() { Name = "Сухари", Rarity = Rarity.Common, CatchBonus = 0.05, FishBiteType = FishType.Combo, ShopPrice = 50, ItemType = ItemType.Lure, IsForShopSale = true },
                new Item() { Name = "Конопляное масло", Rarity = Rarity.Common, CatchBonus = 0.2, FishBiteType = FishType.Combo, ShopPrice = 300, ItemType = ItemType.Lure, IsForShopSale = true },
                new Item() { Name = "Спининг новичка", Rarity = Rarity.Common, CatchBonus = 0.2, HookBonus = 0.25,  FishBiteType = FishType.Combo, ShopPrice = 500, ItemType = ItemType.FishingRod, IsForShopSale = true },
                new Item() { Name = "Удилище", Rarity = Rarity.Common, CatchBonus = 0.25, HookBonus = 0.3, FishBiteType = FishType.Combo, ShopPrice = 1500, ItemType = ItemType.FishingRod, IsForShopSale = true },
            };
        }
    }
}