using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramAspBot.Models.Commands
{
    public interface ICommand
    {
        List<string> Name { get; }
        public void Register(IServiceCollection services);
        Task ExecuteCommand(Message message, ITelegramBotClient telegramBot);
        bool Contains(Message message);
    }
}
