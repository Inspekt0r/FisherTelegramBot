using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAspBot.Job;

namespace TelegramAspBot.Models.Commands
{
    public class ProfileCommand : ICommand
    {
        public List<string> Name { get; } = new List<string>() { "/profile", "/me" };
        private readonly JobManager _jobManager;

        public ProfileCommand(JobManager jobManager)
        {
            _jobManager = jobManager;
        }

        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, ProfileCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            var userId = message.From.Id;
            
            await using var dbContext = new ApplicationContext();
            var character = dbContext.Characters.First(p => p.TelegramId == userId);
            var lure = _jobManager.GetActiveLureJob(character);
            
            var profileGen = new ProfileGenerator(character, lure);

            await telegramBot.SendTextMessageAsync(character.TelegramId, profileGen.GetProfile(), ParseMode.Html);
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
                if (message.Text.Contains(comm))
                {
                    return true;
                }
            }

            return false;
        }
    }
}