using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramAspBot.Models.Commands
{
    public class FishPediaShowCommand : ICommand
    {
        public List<string> Name { get; } = new List<string>() { "/fishpedia" };
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, FishPediaShowCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            await using var dbContext = new ApplicationContext();
            var character = dbContext.Characters.FirstOrDefault(p => p.TelegramId == message.From.Id);
            
            var generateText = new FishPediaTextGenerator(character);

            await telegramBot.SendTextMessageAsync(character.TelegramId, $"{generateText.GetAllFishPediaInfo()}", ParseMode.Html);
        }

        public bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
            {
                return false;
                
            }
            var userId = message.From.Id;
            if (message.Chat.Id != userId)
            {
                return false;
            }
            
            using var dbContext = new ApplicationContext();
            var character = dbContext.Characters.FirstOrDefault(p => p.TelegramId == userId);

            if (character == null) return false;
            
            foreach (var comm in Name)
            {
                if (message.Text.Contains(comm))
                {
                    return true;
                }
            }

            return false;
        }
    }
}