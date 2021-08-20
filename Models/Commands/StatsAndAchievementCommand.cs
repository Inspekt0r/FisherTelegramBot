using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramAspBot.Models.Commands
{
    public class StatsAndAchievementCommand : ICommand
    {
        public List<string> Name { get; } = new List<string>() { "/stats"};
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, StatsAndAchievementCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            await using var dbContext = new ApplicationContext();
            var character = dbContext.Characters.FirstOrDefault(p => p.TelegramId == message.From.Id);
            
            var statsGen = new StatisticGenerator();
            
            await telegramBot.SendTextMessageAsync(character.TelegramId, 
                $"{statsGen.GetStatistic(character)}", ParseMode.Html);
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

            if (character == null) return false;
            foreach (var comm in Name)
            {
                if (comm.Contains(message.Text))
                {
                    return true;
                }
            }

            return false;
        }
    }
}