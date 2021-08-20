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
    public class StartCommand : ICommand
    {
        public List<string> Name { get; } = new List<string>() { "/start" };
        private readonly JobManager _jobManager;
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, StartCommand>();
        }

        public StartCommand(JobManager jobManager)
        {
            _jobManager = jobManager;
        }
        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            var user = message.From;
            await using var dbContext = new ApplicationContext();
            var character = dbContext.Characters.FirstOrDefault(p => p.TelegramId == user.Id);
            
            if (character == null)
            {
                if (user.Username != null)
                {
                    await telegramBot.SendTextMessageAsync(user.Id, $"Привет {user.Username} напиши свой никнейм (до 16 символов)");
                }
                else
                {
                    await telegramBot.SendTextMessageAsync(user.Id, $"Привет {user.FirstName} {user.LastName} напиши свой никнейм (до 16 символов)");
                }
                
                dbContext.Add(FactoryObject.GetNewCharWithId(user.Id));
                await dbContext.SaveChangesAsync();
            }
            else
            {
                var activeLure = _jobManager.GetActiveLureJob(character);
                var profileGen = new ProfileGenerator(character, activeLure);
                await telegramBot.SendTextMessageAsync(user.Id, $"{profileGen.GetStartProfile()}", ParseMode.Html);
            }
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
            var text = message.Text;

            return Name.Any(p => p.Contains(text));
        }
    }
}