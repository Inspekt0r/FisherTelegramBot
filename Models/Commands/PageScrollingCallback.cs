using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoreLinq;
using MoreLinq.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Models.Commands
{
    public class PageScrollingCallback : ICallback
    {
        private readonly ILogger<PageScrollingCallback> _logger;

        public PageScrollingCallback(ILogger<PageScrollingCallback> logger)
        {
            _logger = logger;
        }
        
        public List<string> Name { get; } = new List<string>() { "page" };
        
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICallback, PageScrollingCallback>();
        }

        public async Task ExecuteCommand(CallbackQuery message, ITelegramBotClient telegramBot)
        {
            _logger.LogInformation($"Прилетел колбэк {message.From}, текст колбэка: {message.Data}");
            await using var dbContext = new ApplicationContext();
            var userId = message.From.Id;
            var character = dbContext.Characters.First(p => p.TelegramId == userId);
            var backpackItems = character.Backpack.BackpackItems
                .OrderBy(p => p.Rarity)
                .ThenBy(p => p.ItemName)
                .ToList();
            var pageItems = new List<BackpackItem>();
            var pageType = message.Data.Split('_')[1];
            pageItems = pageType switch
            {
                "fish" => backpackItems.Where(p => p.ItemType == ItemType.Fish && p.IsDeleted == false).ToList(),
                _ => pageItems
            };
            var rawPagePointer = message.Data.Split('_').Last();
            var pageSys = new PageSystem(character, pageItems);
            if (int.TryParse(rawPagePointer, out int pagePointer))
            {
                await telegramBot.AnswerCallbackQueryAsync(message.Id);
                await telegramBot.EditMessageTextAsync(character.TelegramId, 
                    message.Message.MessageId, 
                    pageSys.GenerateText(pagePointer), 
                    ParseMode.Html, 
                    replyMarkup: CallBackKeyboard.GetFishKeyboard(pageItems, pagePointer));
                return;
            }
            await telegramBot.AnswerCallbackQueryAsync(message.Id);
        }

        public bool Contains(CallbackQuery callback)
        {
            var chatId = callback.Message.Chat.Id;
            
            var hookBody = callback.Data.Split("_").First();
            using var dbContext = new ApplicationContext();
            var character = dbContext.Characters.FirstOrDefault(p => p.TelegramId == chatId);

            if (character == null || chatId != character.TelegramId) return false;

            return Name.Any(p => p.Equals(hookBody));
        }
    }
}