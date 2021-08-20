using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramAspBot.Models.Commands
{
    public class HelpCommand : ICommand
    {
        public List<string> Name { get; } = new List<string>() { "/help" };
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommand, HelpCommand>();
        }

        public async Task ExecuteCommand(Message message, ITelegramBotClient telegramBot)
        {
            await telegramBot.SendTextMessageAsync(message.From.Id,
                $"Привет это микропроект по рыбалке, что-то возможно добавится, а что-то нет.\n" +
                $"Правила простые жми /fishing и жди когда нужно будет подсекай рыбу\n" +
                $"Сам процесс ловли составляет где-то 2 минуты, так что не проспи подсечку!\n" +
                $"<a href=\"https://t.me/joinchat/TUG3CUsp3Xhi7EZA\">Канал игровых событий</a>\n" +
                $"Барыга тут: /shop\n" +
                $"Рюкзак доступен по команде /backpack\n" +
                $"Рыбная энциклопедия здесь: /fishpedia\n" +
                $"Статистика и ачивки: /stats\n" +
                $"Все топы: /top\n" +
                $"Баги или идеи отправляйте в бота с тегом #баг или #идея соответственно\n" +
                $"FishingCock ver. alpha 0.2",
                ParseMode.Html);
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
            
            using var dbContext = new ApplicationContext();

            var character = dbContext.Characters.FirstOrDefault(p => p.TelegramId == userId);
            return character != null && Name.Any(p => p.Contains(message.Text));
        }
    }
}
