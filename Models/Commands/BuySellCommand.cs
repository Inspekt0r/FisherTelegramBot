using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramAspBot.Models.Commands
{
    public class BuySellCommand : ICommand
    {
        private readonly ILogger<BuySellCommand> _logger;

        public BuySellCommand(ILogger<BuySellCommand> logger)
        {
            _logger = logger;
        }

        public List<string> Name { get; } = new List<string>() { "/buy", "/sell", "/transfer" };
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, BuySellCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            await using var dbContext = new ApplicationContext();
            var character = dbContext.Characters.First(p => p.TelegramId == message.From.Id);
            
            var buySellSystem = new BuyAndSellSystem(character, telegramBot);
            
            if (message.Text.Split('_').First().Equals("/buy"))
            {
                var msg = message.Text.Split("/buy_").Last();
                if (int.TryParse(msg, out int itemId))
                {
                    var item = dbContext.Items.FirstOrDefault(p => p.Id == itemId && p.ShopPrice > 0 && p.IsForShopSale);
                    if (item == null)
                    {
                        await telegramBot.SendTextMessageAsync(character.TelegramId,
                            $"Продавец этого не может тебе предложить");
                    }
                    else
                    {
                        await buySellSystem.BuyItemAsync(item);   
                    }
                }
            }
            else if (message.Text.Split('_').First().Equals("/sell"))
            {
                var msg = message.Text.Split("_");
                if (msg.Length == 3)
                {
                    //sell many items
                    //todo рефакторинг убрать этот кусок из кода
                    if (int.TryParse(msg[1], out int backpackItemId) && int.TryParse(msg[2], out int count))
                    {
                        var backpackItem = character.Backpack.BackpackItems.FirstOrDefault(p => p.Id == backpackItemId);
                        if (backpackItem == null)
                        {
                            await telegramBot.SendTextMessageAsync(character.TelegramId, $"У тебя нет этого на продажу");
                        }
                        else if (backpackItem.Count < count)
                        {
                            await telegramBot.SendTextMessageAsync(character.TelegramId, $"У тебя нет такого числа предметов на продажу");
                        }
                        else
                        {
                            await buySellSystem.SellItemAsync(backpackItem, 1); 
                        }
                    }
                }
                else
                {
                    if (int.TryParse(msg[1], out int backpackItemId))
                    {
                        var backpackItem = character.Backpack.BackpackItems.FirstOrDefault(p => p.Id == backpackItemId && !p.IsDeleted);
                        if (backpackItem == null)
                        {
                            await telegramBot.SendTextMessageAsync(character.TelegramId, $"У тебя нет этого на продажу");
                        }
                        else
                        {
                            await buySellSystem.SellItemAsync(backpackItem, 1); 
                        }
                    }   
                }
            } 
            else if (message.Text.Split('_').First().Equals("/transfer"))
            {
                _logger.LogInformation($"Инициализация массового трансфера от игрока: {message.From}");
                var fishTypeToTransfer = message.Text.Split('_').Last();
                await buySellSystem.MassTransfer(fishTypeToTransfer);
            }
            else
            {
                _logger.LogError($"Команда /sell /buy /transfer не распознана");
                return;
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