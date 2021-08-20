using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAspBot.Job;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models.Commands
{
    public class TurnOnLureCommand : ICommand
    {
        private readonly JobManager _jobManager;
        public List<string> Name { get; } = new List<string>() { "/lure" };
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, TurnOnLureCommand>();
        }

        public TurnOnLureCommand(JobManager jobManager)
        {
            _jobManager = jobManager;
        }
        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            await using var dbContext = new ApplicationContext();

            var character = dbContext.Characters.FirstOrDefault(p => p.TelegramId == message.From.Id);
            var equipBackpackId = message.Text.Split("/lure_").Last();

            if (int.TryParse(equipBackpackId, out int lureBackPackItemId))
            {
                var luredItem = character.Backpack.BackpackItems.FirstOrDefault(p => p.Id == lureBackPackItemId
                                                                            && p.ItemType == ItemType.Lure
                                                                            && p.Count > 0);
                if (luredItem == null)
                {
                    await telegramBot.SendTextMessageAsync(character.TelegramId, $"У тебя этого предмета нет");
                }
                else
                {
                    await _jobManager.AddLure(character, luredItem, telegramBot);
                }
            }
            else
            {
                await telegramBot.SendTextMessageAsync(character.TelegramId, $"Произошли технические шоколадки с приманкой");
            }

            await dbContext.SaveChangesAsync();
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

            return character != null && Name.Any(command => message.Text.Contains(command));
        }
    }
}