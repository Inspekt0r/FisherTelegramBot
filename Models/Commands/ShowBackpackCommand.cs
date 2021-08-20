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
    public class ShowBackpackCommand : ICommand
    {
        public List<string> Name { get; } = new List<string>() { "/backpack", "/fishpack", "/fishingrod", "/fishingear", "/fishlure" };
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, ShowBackpackCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            await using var dbContext = new ApplicationContext();
            var userId = message.From.Id;
            var character = dbContext.Characters
                .FirstOrDefault(p => p.TelegramId == userId);
            if (character == null)
            {
                return;
            }
            
            var backpackGen = new BackpackTextGenerator(character);
            var sbBackpack = backpackGen.GetMessage(message.Text);
            if (message.Text == "/fishpack")
            {
                var backpackItems = character.Backpack.BackpackItems
                    .OrderBy(p => p.Rarity)
                    .ThenBy(p => p.ItemName)
                    .ToList();
                var fishKeyBoard = CallBackKeyboard.GetFishKeyboard(backpackItems, 1);
                await telegramBot.SendTextMessageAsync(userId, $"{sbBackpack}", ParseMode.Html, replyMarkup: fishKeyBoard);
                return;
            }
            
            await telegramBot.SendTextMessageAsync(userId, $"{sbBackpack}", ParseMode.Html);
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