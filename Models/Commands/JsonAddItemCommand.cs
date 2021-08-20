using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramAspBot.Models.Entity;
using TelegramAspBot.Models.Enum;

namespace TelegramAspBot.Models.Commands
{
    public class JsonAddItemCommand : ICommand
    {
        public List<string> Name { get; } = new List<string>() {"/create_item", "/get_item_json", "/enum_info"};
        private const int AdminId = 137968009;
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, JsonAddItemCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            if (message.Text.Contains("/get_item_json"))
            {
                var testItem = new Item()
                {
                    Name = "Name",
                    IsForShopSale = false,
                    ItemType = ItemType.Bait,
                    Weight = 0.0,
                    Height = 0.0,
                    Rarity = Rarity.Common,
                    FishBiteType = FishType.Carnivorous,
                    IsEvent = false,
                    MaxTemperature = 100,
                    MinTemperature = 0,
                    EventPosition = EventPosition.None
                };
                var msgTest = JsonConvert.SerializeObject(testItem);
                await telegramBot.SendTextMessageAsync(message.From.Id, $"JSON шаблон для Item:\n{msgTest}");
                return;
            }

            if (message.Text.Contains("/enum_info"))
            {
                await telegramBot.SendTextMessageAsync(message.From.Id, $"Информация по enum'ам:\n" +
                                                                        $"ItemType: Fish - 0, FishingRod - 1, Bait - 2, Lure - 3\n" +
                                                                        $"Rarity: Common - 0, Uncommon - 1, Rare - 2, Elite - 3, Mythical - 4, Legendary - 5\n" +
                                                                        $"FishBiteType/FishType: Herbivorous - 0, Carnivorous - 1, Combo - 2\n" +
                                                                        $"WeatherType: None - 0, Cloudy - 1, PartyCloudy - 2, Clear - 3, Rainy - 4," +
                                                                        $"Snow - 5, Fog - 6, Windy - 7\n" +
                                                                        $"EventPosition: None - 0, First - 1, Second - 2, Third - 3, Other - 4");
                return;
            }
            
            var msg = message.Text.Split("/create_item").Last();
            var item = JsonConvert.DeserializeObject<Item>(msg);
            if (item != null)
            {
                await telegramBot.SendTextMessageAsync(message.From.Id, $"{item.Name} получен");
                await using var dbContext = new ApplicationContext();
                dbContext.Add(item);
                await dbContext.SaveChangesAsync();
            }
        }

        public bool Contains(Message message)
        {
            return message.From.Id == AdminId && Name.Any(command => message.Text.Contains(command));
        }
    }
}