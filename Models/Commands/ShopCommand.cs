using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using System.Linq;

namespace TelegramAspBot.Models.Commands
{
    public class ShopCommand : ICommand
    {
        public List<string> Name { get; } = new List<string>() { "/shop" };
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, ShopCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            await using var dbContext = new ApplicationContext();
            
            var character = dbContext.Characters.First(p => p.TelegramId == message.From.Id);
            var shopSystem = new ShopSystem(character, telegramBot);
            
            await shopSystem.SendShopListAsync();
        }

        public bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
            {
                return false;
            }
            
            using var dbContext = new ApplicationContext();
            var userId = message.From.Id;
            if (message.Chat.Id != userId)
            {
                return false;
            }

            var character = dbContext.Characters.FirstOrDefault(p => p.TelegramId == userId);
            
            foreach (var command in Name)
            {
                if (message.Text.Contains(command) && character != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}