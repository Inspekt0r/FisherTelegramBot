using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramAspBot.Models;
using TelegramAspBot.Models.Entity;

namespace TelegramAspBot.Job
{
    public class JobLure
    {
        private readonly ITelegramBotClient _telegramBot;
        public Character Character { get; set; }
        public DateTime LureTimeLeft { get; set; }
        public BackpackItem LureItem { get; set; }
        public bool IsCompleted { get; set; } = false;

        public JobLure(ITelegramBotClient telegramBot)
        {
            _telegramBot = telegramBot;
        }

        public async Task DoAction()
        {
            await _telegramBot.SendTextMessageAsync(Character.TelegramId, $"{LureItem.ItemName} закончил(а) своё действие");
            IsCompleted = true;
            
            await using var dbContext = new ApplicationContext();
            var character = dbContext.Characters.First(p => p.TelegramId == Character.TelegramId);
            
            dbContext.Remove(character.Lure);
            await dbContext.SaveChangesAsync();
        }
        
    }
}