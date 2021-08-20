using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NLog.Fluent;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramAspBot.Models.Commands
{
    public class ShopCallback : ICallback
    {
        public List<string> Name { get; } = new List<string>() {"FishigRod", "Lure", "Combo", "Herb", "Carni", "SellDevices", "SellFish" };
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICallback, ShopCallback>();
        }

        public async Task ExecuteCommand(CallbackQuery message, ITelegramBotClient telegramBot)
        {
            var chatId = message.Message.Chat.Id;
            
            var shopCommand = message.Data.Split("shop").Last();
            
            await using var dbContext = new ApplicationContext();
            var character = dbContext.Characters.First(p => p.TelegramId == message.From.Id);
            
            var shopSystem = new ShopSystem(character, telegramBot);
            
            switch (shopCommand)
            {
                case "FishigRod":
                    shopSystem.AddTextFishingRods();
                    break;
                case "Lure":
                    shopSystem.AddTextLures();
                    break;
                case "Combo":
                    shopSystem.AddTextComboItems();
                    break;
                case "Herb":
                    shopSystem.AddTextHerbivorousItems();
                    break;
                case "Carni":
                    shopSystem.AddTextCarnivorousItems();
                    break;
                case "SellDevices":
                    shopSystem.AddSellDevices();
                    break;
                case "SellFish":
                    shopSystem.AddSellFishes();
                    break;
                default:
                    Log.Error($"Пришел неверный callback по ShopCallBack от {chatId}\nТело {message.Data}");
                    break;
            }

            await shopSystem.EditShopMessageAsync(message);
        }

        public bool Contains(CallbackQuery callback)
        {
            var chatId = callback.Message.Chat.Id;
            
            var hookBody = callback.Data.Split("shop").Last();
           
            //обращаемся к бд за персонажем
            using var dbContext = new ApplicationContext();
            var character = dbContext.Characters.FirstOrDefault(p => p.TelegramId == chatId);

            if (character == null || chatId != character.TelegramId) return false;

            return Name.Any(p => p.Contains(hookBody));
        }
    }
}