using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramAspBot.Models.Interfaces
{
    public interface IBotService
    {
        public Task Initialize();
        public TelegramBotClient GetBotClient();
    }
}