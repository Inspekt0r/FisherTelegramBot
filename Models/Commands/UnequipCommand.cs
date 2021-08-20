using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramAspBot.Models.Commands
{
    public class UnequipCommand : ICommand
    {
        public List<string> Name { get; } = new List<string>() {"/unequip"};
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, UnequipCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            await using var dbContext = new ApplicationContext();

            var character = dbContext.Characters.FirstOrDefault(p => p.TelegramId == message.From.Id);
            var equipBackpackId = message.Text.Split("/unequip_").Last();
            
            if (int.TryParse(equipBackpackId, out int equipItemId))
            {
                var equipItem = character.Backpack.BackpackItems.FirstOrDefault(p => p.Id == equipItemId && p.IsEquipped);
                if (equipItem == null)
                {
                    await telegramBot.SendTextMessageAsync(character.TelegramId, $"У тебя этого предмета нет");
                }
                else
                {
                    var itemControl = new ItemControlSystem(character);
                    var msg = itemControl.TryUnequipItem(equipItem);
                    await telegramBot.SendTextMessageAsync(character.TelegramId, msg.ToString() , ParseMode.Html);

                    await dbContext.SaveChangesAsync();
                }
            }
            else
            {
                await telegramBot.SendTextMessageAsync(character.TelegramId, $"Произошли технические шоколадки");
            }
        }

        public bool Contains(Message message)
        {
            if (message == null || message.Type != MessageType.Text)
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