using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models.Commands
{
    public class AddRandomItemCommand : ICommand
    {
        public List<string> Name { get; } = new List<string>() {"/additem"};
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, AddRandomItemCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            // /additem_userId_itemId
            var userString = message.Text.Split('_')[1];
            var itemString= message.Text.Split('_')[2];

            if (int.TryParse(userString, out int userId) && int.TryParse(itemString, out int itemId))
            {
                await using var dbContext = new ApplicationContext();
                var character = dbContext.Characters.FirstOrDefault(p => p.TelegramId == userId);

                var factory = new FactoryObject();
                var fish = factory.GetNewFish();
                character.Backpack.BackpackItems.Add(
                    new BackpackItem()
                    {
                        Count = 1,
                        ItemName = fish.ItemName,
                        Rarity = fish.Rarity,
                        ItemType = fish.ItemType
                    });

                await dbContext.SaveChangesAsync();
            }
        }

        public bool Contains(Message message)
        {
            if (message.Type != MessageType.Text)
            {
                return false;
                
            }
            
            var user = message.From;

            foreach (var command in Name)
            {
                if (message.Text.Contains(command) && user.Id == 137968009)
                {
                    return true;
                }
            }

            return false;
        }
    }
}