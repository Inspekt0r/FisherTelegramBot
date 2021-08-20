using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAspBot.Models.LocationModule;

namespace TelegramAspBot.Models.Commands
{
    public class ShowSpotsCommand : ICommand
    {
        public List<string> Name { get; } = new List<string>() { "/spots" };
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, ShowSpotsCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            using var dbContext = new ApplicationContext();
            var userId = message.From.Id;

            var character = dbContext.Characters.First(p => p.TelegramId == userId);
            var spots = dbContext.Spots.Where(p=> p.IsActive).ToList();

            var spotMessages = new SpotMessages(spots, character);

            await telegramBot.SendTextMessageAsync(userId, spotMessages.GetSpotList(), ParseMode.Html);
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