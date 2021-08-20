using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAspBot.Models.LocationModule;

namespace TelegramAspBot.Models.Commands
{
    public class MovingCommand : ICommand
    {
        public List<string> Name { get; } = new List<string>() { "/move" };
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, MovingCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            using var dbContext = new ApplicationContext();
            var userId = message.From.Id;

            var character = dbContext.Characters.First(p => p.TelegramId == userId);
            var walkingSystem = new WalkingModule(telegramBot, character);

            var msg = message.Text.Split('_').Last();

            if (int.TryParse(msg, out int spotId))
            {
                var spotToMove = dbContext.Spots.FirstOrDefault(p => p.Id == spotId && p.IsActive);
                if (spotToMove == null)
                {
                    await telegramBot.SendTextMessageAsync(userId, $"Данная локация не найдена");
                    return;
                }

                if (spotToMove.Name == character.Spot.Name)
                {
                    await telegramBot.SendTextMessageAsync(userId, $"Ты уже здесь");
                    return;
                }
                await walkingSystem.TryGoToSpotAsync(spotToMove);
                await dbContext.SaveChangesAsync();

                return;
            }
            
            await telegramBot.SendTextMessageAsync(userId, $"Данная локация не найдена");
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